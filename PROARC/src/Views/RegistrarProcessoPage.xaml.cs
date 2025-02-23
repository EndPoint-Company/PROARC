using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;
using Windows.ApplicationModel.DataTransfer;
using PROARC.src.Models;
using PROARC.src.Control;
using System.ComponentModel;
using Microsoft.UI.Xaml.Media;
using System.Drawing;
using System.Numerics;
using Windows.Storage;
using Microsoft.UI.Text;
using Microsoft.UI;
using PROARC.src.Models.Tipos;
using static PROARC.src.Control.MotivoControl;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.UI.Popups;
using Microsoft.UI.Xaml.Controls.Primitives;
using Windows.Globalization;
using System.Text.RegularExpressions;
using PROARC.src.Models.Arquivos;
using PROARC.src.Converters;
using System.IO;
using PROARC.src.Models.Arquivos.FabricaReclamacao;
using PROARC.src.Models.FabricaEntidadesProcuradoras;
using PROARC.src.Control.Strategies.StrategyValidacao;
using PROARC.src.Control.Strategies.StrategyField;

namespace PROARC.src.Views
{
    public sealed partial class RegistrarProcessoPage : Page, INotifyPropertyChanged
    {
        private string numeroProcesso;
        private string anoProcesso;
        private List<string> arquivosSelecionados = new();
        private static readonly IFabricaReclamacao fabricaReclamacao = new FabricaReclamacao();
        private static readonly IFabricaEntidadeProcuradora fabricaPessoa = new FabricaEntidadeProcuradora();

        public string NumeroProcesso
        {
            get => numeroProcesso;
            set
            {
                if (int.TryParse(value, out int numero))
                {
                    value = numero.ToString("D3"); // Garante três dígitos
                }
                SetProperty(ref numeroProcesso, value);
            }
        }

        public string AnoProcesso
        {
            get => anoProcesso;
            set => SetProperty(ref anoProcesso, value);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public RegistrarProcessoPage()
        {
            InitializeComponent();
            DataContext = this;
            CarregarMotivosAsync();
            ConfigureShadows();
            calendario.Date = DateTime.Now.Date;


            // Define o primeiro item ("01") como selecionado nos reclamados (vlw marquin)
            comboBoxQuantidadeReclamado.SelectedIndex = 0;

            // Chama manualmente o evento SelectionChanged
            OnQuantidadeReclamados(comboBoxQuantidadeReclamado, null);


            // Preenche os ComboBox de UF para todos os reclamados
            PreencherComboBoxUfTodosReclamados();

        }

        protected void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }
        private async Task CarregarMotivosAsync()
         => cbMotivo.ItemsSource = await MotivoControl.GetAllAsync();


        private void ProcessoNovo_Click(object sender, RoutedEventArgs e)
        => ConfigurarEstadoProcesso(true);

        private void ProcessoAntigo_Click(object sender, RoutedEventArgs e)
            => ConfigurarEstadoProcesso(false);


        private void ConfigurarEstadoProcesso(bool isNovoProcesso)
        {
            radio_agFazerNotificacao.IsChecked = isNovoProcesso;
            AtualizarEstilosBotoes(isNovoProcesso);
            AjustarCamposProcesso(isNovoProcesso);
        }

        private void AtualizarEstilosBotoes(bool isNovoProcesso)
        {
            btnProcessoNovo.Background = new SolidColorBrush(isNovoProcesso ? Microsoft.UI.Colors.DarkBlue : Microsoft.UI.Colors.White);
            btnProcessoNovo.Foreground = new SolidColorBrush(isNovoProcesso ? Microsoft.UI.Colors.White : Microsoft.UI.Colors.DarkBlue);

            btnProcessoAntigo.Background = new SolidColorBrush(isNovoProcesso ? Microsoft.UI.Colors.White : Microsoft.UI.Colors.DarkBlue);
            btnProcessoAntigo.Foreground = new SolidColorBrush(isNovoProcesso ? Microsoft.UI.Colors.DarkBlue : Microsoft.UI.Colors.White);
            btnProcessoAntigo.BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.CornflowerBlue);
        }

        private async void AjustarCamposProcesso(bool isNovoProcesso)
        {
            inputNProcesso.IsReadOnly = isNovoProcesso;
            inputAnoProcesso.IsReadOnly = isNovoProcesso;
            MainStackPanel.Opacity = isNovoProcesso ? 0.4 : 1;

            if (isNovoProcesso)
            {
                await DefinirNovoProcesso();
                calendario.MinDate = DateTimeOffset.Now;
                calendario.Date = DateTimeOffset.Now;
            }
            else
            {
                NumeroProcesso = string.Empty;
                AnoProcesso = string.Empty;
                calendario.ClearValue(CalendarDatePicker.MinDateProperty);
                calendario.Date = DateTimeOffset.Now;
            }
        }

        private void SelecionarHora_Click(object sender, RoutedEventArgs e)
        {
            var timePickerFlyout = new TimePickerFlyout();

            if (sender is Button botao)
            {
                timePickerFlyout.Placement = FlyoutPlacementMode.Bottom;
                timePickerFlyout.Time = DateTime.Now.TimeOfDay;
                timePickerFlyout.ClockIdentifier = "24HourClock";

                // Quando o usuário selecionar um horário, o nome do botão será atualizado
                timePickerFlyout.Closed += (s, args) =>
                {
                    botao.Content = $"{timePickerFlyout.Time.Hours:D2}:{timePickerFlyout.Time.Minutes:D2}";
                };

                timePickerFlyout.ShowAt(botao);
            }
        }

        private async Task DefinirNovoProcesso()
        {
            int count = await ReclamacaoControl.CountGAsync();
            NumeroProcesso = (count + 1).ToString();
            AnoProcesso = DateTime.Now.Year.ToString();
        }

        private void ProcuradorCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ProcuradorSection1.Visibility = Visibility.Visible;
        }

        private void ProcuradorCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ProcuradorSection1.Visibility = Visibility.Collapsed;
        }

        // POP-UPS
        private async Task<ContentDialogResult> MostrarContentDialogAsync(string title, object content, string primaryButtonText = null, string closeButtonText = "Ok")
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                PrimaryButtonText = primaryButtonText,
                CloseButtonText = closeButtonText,
                XamlRoot = this.Content.XamlRoot
            };

            return await dialog.ShowAsync();
        }

        private async Task ShowError(string mensagemErro)
        {
            await MostrarContentDialogAsync("Erro", mensagemErro);
        }

        private async Task ShowSuccess(string mensagemSucesso)
        {
            await MostrarContentDialogAsync("Sucesso", mensagemSucesso);
        }


        private async void OnNovoMotivoClick(object sender, RoutedEventArgs e)
        {
            var textBox = new TextBox
            {
                PlaceholderText = "Digite o motivo"
            };

            var result = await MostrarContentDialogAsync(
                "Adicionar Novo Motivo",
                textBox,
                "Salvar",
                "Cancelar");

            if (result == ContentDialogResult.Primary)
            {
                var motivoTexto = textBox.Text;

                if (!string.IsNullOrWhiteSpace(motivoTexto))
                {
                    var motivosExistentes = await MotivoControl.GetAllAsync();

                    if (motivosExistentes.Any(m => m.Nome.Equals(motivoTexto, StringComparison.OrdinalIgnoreCase)))
                    {
                        await ShowError("Este motivo já existe.");
                    }
                    else
                    {
                        var motivo = new Motivo(motivoTexto, null);

                        try
                        {
                            await MotivoControl.InsertAsync(motivo);
                            await CarregarMotivosAsync();

                            await ShowSuccess("Motivo salvo com sucesso!");
                        }
                        catch (Exception ex)
                        {
                            await ShowError($"Falha ao salvar motivo: {ex.Message}");
                        }
                    }
                }
                else
                {
                    await ShowError("O motivo não pode estar vazio.");
                }
            }
        }

        private void OnQuantidadeReclamados(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxQuantidadeReclamado.SelectedItem is ComboBoxItem selectedItem)
            {
                int quantidade = int.Parse(selectedItem.Content.ToString());

                Reclamado01.Visibility = quantidade >= 1 ? Visibility.Visible : Visibility.Collapsed;
                Reclamado02.Visibility = quantidade >= 2 ? Visibility.Visible : Visibility.Collapsed;
                Reclamado03.Visibility = quantidade >= 3 ? Visibility.Visible : Visibility.Collapsed;
                Reclamado04.Visibility = quantidade >= 4 ? Visibility.Visible : Visibility.Collapsed;
                Reclamado05.Visibility = quantidade >= 5 ? Visibility.Visible : Visibility.Collapsed;
                Reclamado06.Visibility = quantidade >= 6 ? Visibility.Visible : Visibility.Collapsed;
                Reclamado07.Visibility = quantidade >= 7 ? Visibility.Visible : Visibility.Collapsed;
                Reclamado08.Visibility = quantidade >= 8 ? Visibility.Visible : Visibility.Collapsed;
                Reclamado09.Visibility = quantidade >= 9 ? Visibility.Visible : Visibility.Collapsed;
                Reclamado10.Visibility = quantidade >= 10 ? Visibility.Visible : Visibility.Collapsed;
                Reclamado11.Visibility = quantidade >= 11 ? Visibility.Visible : Visibility.Collapsed;
                Reclamado12.Visibility = quantidade >= 12 ? Visibility.Visible : Visibility.Collapsed;
                Reclamado13.Visibility = quantidade >= 13 ? Visibility.Visible : Visibility.Collapsed;
                Reclamado14.Visibility = quantidade >= 14 ? Visibility.Visible : Visibility.Collapsed;
                Reclamado15.Visibility = quantidade >= 15 ? Visibility.Visible : Visibility.Collapsed;
                Reclamado16.Visibility = quantidade >= 16 ? Visibility.Visible : Visibility.Collapsed;

                return;
            }

            Reclamado01.Visibility = Visibility.Collapsed;
            Reclamado02.Visibility = Visibility.Collapsed;
            Reclamado03.Visibility = Visibility.Collapsed;
            Reclamado04.Visibility = Visibility.Collapsed;
            Reclamado05.Visibility = Visibility.Collapsed;
            Reclamado06.Visibility = Visibility.Collapsed;
            Reclamado07.Visibility = Visibility.Collapsed;
            Reclamado08.Visibility = Visibility.Collapsed;
            Reclamado09.Visibility = Visibility.Collapsed;
            Reclamado10.Visibility = Visibility.Collapsed;
            Reclamado11.Visibility = Visibility.Collapsed;
            Reclamado12.Visibility = Visibility.Collapsed;
            Reclamado13.Visibility = Visibility.Collapsed;
            Reclamado14.Visibility = Visibility.Collapsed;
            Reclamado15.Visibility = Visibility.Collapsed;
            Reclamado16.Visibility = Visibility.Collapsed;
        }


        private void OnTipoDocumentoChanged(object sender, SelectionChangedEventArgs e, ComboBox comboBoxTipoDocumento, TextBox inputCnpjCpf)
        {
            if (comboBoxTipoDocumento.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedText = selectedItem.Content.ToString();

                // Verifica se o inputCnpjCpf já foi inicializado antes de acessá-lo
                if (inputCnpjCpf != null)
                {
                    inputCnpjCpf.IsEnabled = true;

                    if (selectedText == "CPF")
                    {
                        inputCnpjCpf.MaxLength = 14; // CPF formatado tem 14 caracteres (incluindo pontos e traço)
                        inputCnpjCpf.PlaceholderText = "Insira o CPF";
                    }
                    else if (selectedText == "CNPJ")
                    {
                        inputCnpjCpf.MaxLength = 18; // CNPJ formatado tem 18 caracteres (incluindo pontos, barra e traço)
                        inputCnpjCpf.PlaceholderText = "Insira o CNPJ";
                    }

                    // Limpa o campo ao mudar a opção
                    inputCnpjCpf.Text = string.Empty;
                }
            }
        }

        private void OnCpfCnpjTextChanged(object sender, TextChangedEventArgs e, ComboBox comboBoxTipoDocumento, TextBox textBoxCnpjCpf)
        {
            if (sender is not TextBox textBox) return;

            // Verifica se há um item selecionado no ComboBox
            if (comboBoxTipoDocumento.SelectedItem is not ComboBoxItem selectedItem) return;

            string selectedType = selectedItem.Content.ToString(); // Obtém o tipo selecionado (CPF ou CNPJ)
            string text = Regex.Replace(textBox.Text, @"\D", ""); // Remove todos os caracteres não numéricos

            // Define o comprimento máximo com base no tipo de documento
            int maxLength = selectedType == "CPF" ? 11 : 14;
            if (text.Length > maxLength) text = text.Substring(0, maxLength);

            // Formata o texto com base no tipo de documento
            textBoxCnpjCpf.Text = FormatDocument(text, selectedType);
            textBoxCnpjCpf.SelectionStart = textBoxCnpjCpf.Text.Length; // Posiciona o cursor no final
        }



        private string FormatDocument(string text, string type)
        {
            if (type == "CPF")
            {
                if (text.Length <= 3) return text;
                if (text.Length <= 6) return $"{text.Substring(0, 3)}.{text.Substring(3)}";
                if (text.Length <= 9) return $"{text.Substring(0, 3)}.{text.Substring(3, 3)}.{text.Substring(6)}";
                return $"{text.Substring(0, 3)}.{text.Substring(3, 3)}.{text.Substring(6, 3)}-{text.Substring(9)}";
            }
            else if (type == "CNPJ")
            {
                if (text.Length <= 2) return text;
                if (text.Length <= 5) return $"{text.Substring(0, 2)}.{text.Substring(2)}";
                if (text.Length <= 8) return $"{text.Substring(0, 2)}.{text.Substring(2, 3)}.{text.Substring(5)}";
                if (text.Length <= 12) return $"{text.Substring(0, 2)}.{text.Substring(2, 3)}.{text.Substring(5, 3)}/{text.Substring(8)}";
                return $"{text.Substring(0, 2)}.{text.Substring(2, 3)}.{text.Substring(5, 3)}/{text.Substring(8, 4)}-{text.Substring(12)}";
            }
            return text;
        }

        private string FormatWithMask(string text, int[] positions, char[] separators)
        {
            if (text.Length == 0) return text;

            string formattedText = text;

            for (int i = 0; i < positions.Length; i++)
            {
                if (formattedText.Length > positions[i])
                    formattedText = formattedText.Insert(positions[i], separators[i].ToString());
            }

            return formattedText;
        }

        private void OnCpfTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox textBox) return;
            string rawText = Regex.Replace(textBox.Text, @"\D", "");
            textBox.Text = FormatWithMask(rawText, new[] { 3, 7, 11 }, new[] { '.', '.', '-' });
            textBox.SelectionStart = textBox.Text.Length;
        }

        private void PreencherComboBoxUfTodosReclamados()
        {
            // Lista de UFs do Brasil
            List<string> ufs = new List<string>
    {
        "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA",
        "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN",
        "RS", "RO", "RR", "SC", "SP", "SE", "TO"
    };

            // Itera sobre os 16 reclamados
            for (int i = 1; i <= 16; i++)
            {
                // Obtém o ComboBox do reclamado atual
                var comboBoxUf = FindName($"comboBoxUfReclamado{i:00}") as ComboBox;

                // Verifica se o ComboBox foi encontrado
                if (comboBoxUf != null)
                {
                    // Preenche o ComboBox com as UFs
                    comboBoxUf.ItemsSource = ufs;
                }
            }
        }

        public LinkedList<Reclamado> CriarListaReclamados()
        {
            // Cria uma nova LinkedList para armazenar os reclamados
            LinkedList<Reclamado> listaReclamados = new LinkedList<Reclamado>();

            // Obtém a quantidade de reclamados selecionada no ComboBox
            int quantidadeReclamados = ObterQuantidadeReclamadosSelecionada();
            Debug.WriteLine($"Quantidade de reclamados selecionada: {quantidadeReclamados}");

            // Itera sobre os reclamados visíveis
            for (int i = 1; i <= quantidadeReclamados; i++)
            {
                Debug.WriteLine($"Processando reclamado {i}...");


                var comboBox = FindName($"comboBoxUfReclamado{i:00}") as ComboBox;
                // Obtém os controles do reclamado atual


                var inputNomeReclamado = FindName($"inputNomeReclamado{i:00}") as TextBox;
                var inputCpfReclamado = FindName($"inputCpfReclamado{i:00}") as TextBox;
                var inputCnpjReclamado = FindName($"inputCnpjReclamado{i:00}") as TextBox;
                var inputNumeroReclamado = FindName($"inputNumeroReclamado{i:00}") as TextBox;
                var inputRuaReclamado = FindName($"inputRuaReclamado{i:00}") as TextBox;
                var inputBairroReclamado = FindName($"inputBairroReclamado{i:00}") as TextBox;
                var inputCidadeReclamado = FindName($"inputCidadeReclamado{i:00}") as TextBox;
                var inputUfReclamado = comboBox?.SelectedItem?.ToString() ?? string.Empty;
                var inputCepReclamado = FindName($"inputCepReclamado{i:00}") as TextBox;
                var inputTelefoneReclamado = FindName($"inputTelefoneReclamado{i:00}") as TextBox;
                var inputEmailReclamado = FindName($"inputEmailReclamado{i:00}") as TextBox;

                // Verifica se os campos obrigatórios estão preenchidos
                if (inputNomeReclamado != null && !string.IsNullOrWhiteSpace(inputNomeReclamado.Text))
                {
                    Debug.WriteLine($"Reclamado {i}: Nome encontrado: {inputNomeReclamado.Text}");

                    // Cria um novo objeto Reclamado com os dados dos campos
                    Reclamado reclamado = new Reclamado(
                        nome: inputNomeReclamado.Text,
                        cpf: string.IsNullOrWhiteSpace(inputCpfReclamado?.Text) ? null : inputCpfReclamado.Text,
                        cnpj: string.IsNullOrWhiteSpace(inputCnpjReclamado?.Text) ? null : inputCnpjReclamado.Text,
                        numero: short.TryParse(inputNumeroReclamado?.Text, out short numero) ? numero : (short?)null,
                        logradouro: string.IsNullOrWhiteSpace(inputRuaReclamado?.Text) ? null : inputRuaReclamado.Text,
                        bairro: string.IsNullOrWhiteSpace(inputBairroReclamado?.Text) ? null : inputBairroReclamado.Text,
                        cidade: string.IsNullOrWhiteSpace(inputCidadeReclamado?.Text) ? null : inputCidadeReclamado.Text,
                        uf: string.IsNullOrWhiteSpace(inputUfReclamado) ? null : inputUfReclamado,
                        cep: string.IsNullOrWhiteSpace(inputCepReclamado?.Text) ? null : inputCepReclamado.Text,
                        telefone: string.IsNullOrWhiteSpace(inputTelefoneReclamado?.Text) ? null : inputTelefoneReclamado.Text,
                        email: string.IsNullOrWhiteSpace(inputEmailReclamado?.Text) ? null : inputEmailReclamado.Text
                    );

                    // Adiciona o reclamado à LinkedList
                    listaReclamados.AddLast(reclamado);
                    Debug.WriteLine($"Reclamado {i} adicionado à lista.");
                }
                else
                {
                    Debug.WriteLine($"Reclamado {i}: Nome não preenchido ou controle não encontrado.");
                }
            }

            // Retorna a lista de reclamados
            Debug.WriteLine($"Total de reclamados criados: {listaReclamados.Count}");
            return listaReclamados;
        }


        private async void ContinuarButton_Click(object sender, RoutedEventArgs e)
        {
            int quantidadeReclamados = ObterQuantidadeReclamadosSelecionada();

            var resetProcessor = new FieldProcessor(new ResetFieldStrategy());
            resetProcessor.ProcessMultipleFields(GetFieldsToProcess(quantidadeReclamados));

            NumeroProcesso = inputNProcesso.Text.Trim();
            AnoProcesso = inputAnoProcesso.Text.Trim();

            bool isNovoProcesso = radio_agResposta.IsChecked == true;

            if (isNovoProcesso)
            {
                if (string.IsNullOrEmpty(NumeroProcesso))
                {
                    int count = await ReclamacaoControl.CountAsync();
                    NumeroProcesso = (count + 1).ToString();
                }

                if (string.IsNullOrEmpty(AnoProcesso))
                {
                    AnoProcesso = DateTime.Now.Year.ToString();
                }
            }

            if (!CamposPreenchidos())
            {
                await ShowError("Preencha todos os campos obrigatórios antes de continuar.");

                var highlightProcessor = new FieldProcessor(new HighlightFieldStrategy());
                highlightProcessor.ProcessMultipleFields(GetFieldsToProcess(quantidadeReclamados));
                return;
            }

            if (!ValidarCampos()) return;

            Motivo? motivoSelecionado = cbMotivo.SelectedItem != null ? new Motivo(cbMotivo.SelectedItem.ToString()) : null;

            string cpfLimpoReclamante = new string(inputCpfReclamante.Text.Where(char.IsDigit).ToArray());

            var reclamante = fabricaPessoa.CriarEntidadeProcuradora(EnumEntidadeProcuradora.Reclamante, inputNome.Text,
                cpfLimpoReclamante,
                string.IsNullOrWhiteSpace(inputRgReclamante.Text) ? null : inputRgReclamante.Text,
                string.IsNullOrWhiteSpace(inputNumeroReclamante.Text) ? null : inputNumeroReclamante.Text,
                string.IsNullOrWhiteSpace(inputEmailReclamante.Text) ? null : inputEmailReclamante.Text);

            Procurador procurador = null;

            if (ProcuradorCheckBox.IsChecked == true)
            {
             string cpfLimpoProcurador = new string(inputCpfProcurador.Text.Where(char.IsDigit).ToArray());

             procurador = (Procurador?)fabricaPessoa.CriarEntidadeProcuradora(EnumEntidadeProcuradora.Procurador, inputNomeProcurador.Text,
                    cpfLimpoProcurador,
                    string.IsNullOrWhiteSpace(inputRgProcurador.Text) ? null : inputRgProcurador.Text,
                    string.IsNullOrWhiteSpace(inputNumeroProcurador.Text) ? null : inputNumeroProcurador.Text,
                    string.IsNullOrWhiteSpace(inputEmailProcurador.Text) ? null : inputEmailProcurador.Text);
            }

            string dataAtual = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") ?? "null";
            DateTime? dataSelecionada = calendario.Date?.DateTime;
            string dataFormatada = dataSelecionada.HasValue
                ? dataSelecionada.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")
                : "0001-01-01 00:00:00.000";

            string caminhoPasta = Path.Combine("/home/~/recl", $"G{NumeroProcesso}", AnoProcesso);

            string nProcesso = NumeroProcesso;
            short anoProcesso = short.TryParse(AnoProcesso, out short parsedAno) ? parsedAno : (short)DateTime.Now.Year;
            string titulo = "G" + nProcesso + "/" + anoProcesso;

            // Coleta os reclamados
            LinkedList<Reclamado> reclamados = CriarListaReclamados();

            string conciliador = inputNomeConciliador.Text;
            string situacao = GetSelectedRadioButton();

            var reclamacaoGeral = fabricaReclamacao.CriarReclamacao(
                EnumReclamacao.ReclamacaoGeral,
                motivoSelecionado,
                (Reclamante?)reclamante,
                procurador,
                reclamados,
                titulo,
                situacao,
                caminhoPasta,
                DateOnly.FromDateTime(DateTime.Now),
                "Sistema",
                null,
                null,
                null,
                null,
                dataSelecionada,
                conciliador
                );

            ButtonContinuar.IsEnabled = false;
            bool success = await ReclamacaoControl.InsertAsyncG((ReclamacaoGeral)reclamacaoGeral);
            ButtonContinuar.IsEnabled = true;

            if (success)
            {
                await ShowSuccess("O processo foi cadastrado com sucesso!");
                Frame.Navigate(typeof(RegistrarProcessoPage), true);
            }
            else
            {
                await ShowError("Falha ao cadastrar o processo. Tente novamente.");
            }
        }


        private int ObterQuantidadeReclamadosSelecionada()
        {
            if (comboBoxQuantidadeReclamado.SelectedItem is ComboBoxItem selectedItem)
            {
                return int.Parse(selectedItem.Content.ToString());
            }
            return 1; // Se nenhum item estiver selecionado
        }

        private IEnumerable<(FrameworkElement Field, TextBlock Label, TextBlock? Title)> GetFieldsToProcess(int quantidadeReclamados)
        {
            yield return (inputNProcesso, TextBlockNProcesso, null);
            yield return (inputAnoProcesso, TextBlockAnoProcesso, null);
            yield return (inputNome, TextBlockNome, TextBlockReclamante);
            yield return (inputCpfReclamante, TextBlockCpfReclamante, TextBlockReclamante);
            yield return (cbMotivo, TextBlockMotivo, null);
            yield return (calendario, TextBlockCalendario, null);

            for (int i = 1; i <= quantidadeReclamados; i++)
            {
                var textBlockReclamado = FindName($"textBlockReclamado{i:00}") as TextBlock;
                yield return (FindName($"inputNomeReclamado{i:00}") as TextBox, textBlockReclamado, FindName($"TextBlockNomeReclamado{i:00}") as TextBlock);
                yield return (FindName($"inputRuaReclamado{i:00}") as TextBox, textBlockReclamado, FindName($"TextBlockRuaReclamado{i:00}") as TextBlock);
                yield return (FindName($"inputNumeroReclamado{i:00}") as TextBox, textBlockReclamado, FindName($"TextBlockNumeroReclamado{i:00}") as TextBlock);
                yield return (FindName($"inputBairroReclamado{i:00}") as TextBox, textBlockReclamado, FindName($"TextBlockBairroReclamado{i:00}") as TextBlock);
                yield return (FindName($"inputCidadeReclamado{i:00}") as TextBox, textBlockReclamado, FindName($"TextBlockCidadeReclamado{i:00}") as TextBlock);
                yield return (FindName($"inputCepReclamado{i:00}") as TextBox, textBlockReclamado, FindName($"TextBlockCepReclamado{i:00}") as TextBlock);
                yield return (FindName($"comboBoxUfReclamado{i:00}") as ComboBox, textBlockReclamado, FindName($"TextBlockUfReclamado{i:00}") as TextBlock);
            }

            if (GetSelectedRadioButton() == "Nenhum status selecionado")
            {
                yield return (radio_agFazerNotificacao, TextBlockTramitacao, TextBlockStatus);
                yield return (radio_agRealizacaoAudiencia, TextBlockTramitacao, TextBlockStatus);
                yield return (radio_agResposta, TextBlockTramitacao, TextBlockStatus);
                yield return (radio_agEnvioNotificacao, TextBlockTramitacao, TextBlockStatus);
                yield return (radio_agDocumentacao, TextBlockTramitacao, TextBlockStatus);
                yield return (radio_atendido, TextBlockArquivado, TextBlockStatus);
                yield return (radio_naoAtendido, TextBlockArquivado, TextBlockStatus);
            }
        }

        private bool CamposPreenchidos()
        => new[] { inputNome, inputCpfReclamante}
            .All(campo => !string.IsNullOrWhiteSpace(campo.Text))
            && cbMotivo.SelectedItem != null
            && calendario.Date != null
            && GetSelectedRadioButton() != "Nenhum status selecionado";

        private string GetSelectedRadioButton()
        {
            if (radio_agFazerNotificacao.IsChecked == true)
                return "Aguardando fazer notificação";
            if (radio_agRealizacaoAudiencia.IsChecked == true)
                return "Aguardando realização da audiência";
            if (radio_agResposta.IsChecked == true)
                return "Aguardando resposta da empresa";
            if (radio_agEnvioNotificacao.IsChecked == true)
                return "Aguardando envio da notificação";
            if (radio_agDocumentacao.IsChecked == true)
                return "Aguardando documentação";
            if (radio_atendido.IsChecked == true)
                return "Atendido";
            if (radio_naoAtendido.IsChecked == true)
                return "Não Atendido";

            return "Nenhum status selecionado";
        }

        private async void PickFileButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            picker.FileTypeFilter.Add("*");

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            var files = await picker.PickMultipleFilesAsync();

            if (files != null && files.Any())
            {
                AdicionarArquivos(files.Select(file => file.Path));
            }
        }

        private void AdicionarArquivos(IEnumerable<string> arquivos)
        {
            foreach (var arquivo in arquivos)
            {
                var nomeArquivo = System.IO.Path.GetFileName(arquivo);

                if (!ListaArquivos.Children.OfType<TextBlock>().Any(tb => tb.Text == nomeArquivo))
                {
                    ListaArquivos.Children.Add(new TextBlock
                    {
                        Text = nomeArquivo,
                        FontSize = 14,
                        Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green)
                    });
                }
            }

            AtualizarMensagemNenhumArquivo();
        }

        private void AtualizarMensagemNenhumArquivo()
        {
            MensagemNenhumArquivo.Visibility = ListaArquivos.Children.OfType<TextBlock>().Any()
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        private async void DragDropArea_Drop(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                var arquivos = items.OfType<StorageFile>().Select(file => file.Path);

                if (arquivos.Any())
                {
                    AdicionarArquivos(arquivos);
                }
            }
        }

        private bool ValidarCampos()
        {
            var erros = new List<string>();

            var validacoes = new (IValidacaoStrategy estrategia, string valor, string mensagemErro)[]
            {
                (new ValidacaoCPF(), inputCpfReclamante.Text, "CPF do Reclamante inválido."),
                (new ValidacaoEmail(), inputEmailReclamante.Text, "E-mail inválido."),
                (new ValidacaoTelefone(), inputNumeroReclamante.Text, "Telefone inválido.")
            };

            foreach (var (estrategia, valor, mensagemErro) in validacoes)
            {
                if (!string.IsNullOrWhiteSpace(valor))
                {
                    var validador = new Validador(estrategia);
                    if (!validador.Validar(valor))
                    {
                        erros.Add(mensagemErro);
                    }
                }
            }

            if (erros.Count > 0)
            {
                ShowError(string.Join("\n", erros));
                return false;
            }

            return true;
        }

        private void DragDropArea_DragOver(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
        }

        private void ConfigureShadows()
        {
            MotivoSection.Translation = new Vector3(1, 1, 20);
            NProcessoSection.Translation = new Vector3(1, 1, 20);
            AudienciaSection.Translation = new Vector3(1, 1, 20);
            StatusSection.Translation = new Vector3(1, 1, 20);
            ReclamanteSection.Translation = new Vector3(1, 1, 20);
            ProcuradorSection2.Translation = new Vector3(1, 1, 20);
            AnexarArquivosSection.Translation = new Vector3(1, 1, 20);
            ConciliadorSection.Translation = new Vector3(1, 1, 20);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is bool isNovoProcesso)
            {
                ConfigurarEstadoProcesso(isNovoProcesso);
            }
        }

        // Reclamado 01
        private void OnTipoDocumentoChangedReclamado01(object sender, SelectionChangedEventArgs e)
        {
            OnTipoDocumentoChanged(sender, e, comboBoxTipoDocumentoReclamado01, inputCnpjCpfReclamado01);
        }

        private void OnCpfCnpjTextChangedReclamado01(object sender, TextChangedEventArgs e)
        {
            OnCpfCnpjTextChanged(sender, e, comboBoxTipoDocumentoReclamado01, inputCnpjCpfReclamado01);
        }

        // Reclamado 02
        private void OnTipoDocumentoChangedReclamado02(object sender, SelectionChangedEventArgs e)
        {
            OnTipoDocumentoChanged(sender, e, comboBoxTipoDocumentoReclamado02, inputCnpjCpfReclamado02);
        }

        private void OnCpfCnpjTextChangedReclamado02(object sender, TextChangedEventArgs e)
        {
            OnCpfCnpjTextChanged(sender, e, comboBoxTipoDocumentoReclamado02, inputCnpjCpfReclamado02);
        }

        // Reclamado 03
        private void OnTipoDocumentoChangedReclamado03(object sender, SelectionChangedEventArgs e)
        {
            OnTipoDocumentoChanged(sender, e, comboBoxTipoDocumentoReclamado03, inputCnpjCpfReclamado03);
        }

        private void OnCpfCnpjTextChangedReclamado03(object sender, TextChangedEventArgs e)
        {
            OnCpfCnpjTextChanged(sender, e, comboBoxTipoDocumentoReclamado03, inputCnpjCpfReclamado03);
        }

        // Reclamado 04
        private void OnTipoDocumentoChangedReclamado04(object sender, SelectionChangedEventArgs e)
        {
            OnTipoDocumentoChanged(sender, e, comboBoxTipoDocumentoReclamado04, inputCnpjCpfReclamado04);
        }

        private void OnCpfCnpjTextChangedReclamado04(object sender, TextChangedEventArgs e)
        {
            OnCpfCnpjTextChanged(sender, e, comboBoxTipoDocumentoReclamado04, inputCnpjCpfReclamado04);
        }

        // Reclamado 05
        private void OnTipoDocumentoChangedReclamado05(object sender, SelectionChangedEventArgs e)
        {
            OnTipoDocumentoChanged(sender, e, comboBoxTipoDocumentoReclamado05, inputCnpjCpfReclamado05);
        }

        private void OnCpfCnpjTextChangedReclamado05(object sender, TextChangedEventArgs e)
        {
            OnCpfCnpjTextChanged(sender, e, comboBoxTipoDocumentoReclamado05, inputCnpjCpfReclamado05);
        }

        // Reclamado 06
        private void OnTipoDocumentoChangedReclamado06(object sender, SelectionChangedEventArgs e)
        {
            OnTipoDocumentoChanged(sender, e, comboBoxTipoDocumentoReclamado06, inputCnpjCpfReclamado06);
        }

        private void OnCpfCnpjTextChangedReclamado06(object sender, TextChangedEventArgs e)
        {
            OnCpfCnpjTextChanged(sender, e, comboBoxTipoDocumentoReclamado06, inputCnpjCpfReclamado06);
        }

        // Reclamado 07
        private void OnTipoDocumentoChangedReclamado07(object sender, SelectionChangedEventArgs e)
        {
            OnTipoDocumentoChanged(sender, e, comboBoxTipoDocumentoReclamado07, inputCnpjCpfReclamado07);
        }

        private void OnCpfCnpjTextChangedReclamado07(object sender, TextChangedEventArgs e)
        {
            OnCpfCnpjTextChanged(sender, e, comboBoxTipoDocumentoReclamado07, inputCnpjCpfReclamado07);
        }

        // Reclamado 08
        private void OnTipoDocumentoChangedReclamado08(object sender, SelectionChangedEventArgs e)
        {
            OnTipoDocumentoChanged(sender, e, comboBoxTipoDocumentoReclamado08, inputCnpjCpfReclamado08);
        }

        private void OnCpfCnpjTextChangedReclamado08(object sender, TextChangedEventArgs e)
        {
            OnCpfCnpjTextChanged(sender, e, comboBoxTipoDocumentoReclamado08, inputCnpjCpfReclamado08);
        }

        // Reclamado 09
        private void OnTipoDocumentoChangedReclamado09(object sender, SelectionChangedEventArgs e)
        {
            OnTipoDocumentoChanged(sender, e, comboBoxTipoDocumentoReclamado09, inputCnpjCpfReclamado09);
        }

        private void OnCpfCnpjTextChangedReclamado09(object sender, TextChangedEventArgs e)
        {
            OnCpfCnpjTextChanged(sender, e, comboBoxTipoDocumentoReclamado09, inputCnpjCpfReclamado09);
        }

        // Reclamado 10
        private void OnTipoDocumentoChangedReclamado10(object sender, SelectionChangedEventArgs e)
        {
            OnTipoDocumentoChanged(sender, e, comboBoxTipoDocumentoReclamado10, inputCnpjCpfReclamado10);
        }

        private void OnCpfCnpjTextChangedReclamado10(object sender, TextChangedEventArgs e)
        {
            OnCpfCnpjTextChanged(sender, e, comboBoxTipoDocumentoReclamado10, inputCnpjCpfReclamado10);
        }

        // Reclamado 11
        private void OnTipoDocumentoChangedReclamado11(object sender, SelectionChangedEventArgs e)
        {
            OnTipoDocumentoChanged(sender, e, comboBoxTipoDocumentoReclamado11, inputCnpjCpfReclamado11);
        }

        private void OnCpfCnpjTextChangedReclamado11(object sender, TextChangedEventArgs e)
        {
            OnCpfCnpjTextChanged(sender, e, comboBoxTipoDocumentoReclamado11, inputCnpjCpfReclamado11);
        }

        // Reclamado 12
        private void OnTipoDocumentoChangedReclamado12(object sender, SelectionChangedEventArgs e)
        {
            OnTipoDocumentoChanged(sender, e, comboBoxTipoDocumentoReclamado12, inputCnpjCpfReclamado12);
        }

        private void OnCpfCnpjTextChangedReclamado12(object sender, TextChangedEventArgs e)
        {
            OnCpfCnpjTextChanged(sender, e, comboBoxTipoDocumentoReclamado12, inputCnpjCpfReclamado12);
        }

        // Reclamado 13
        private void OnTipoDocumentoChangedReclamado13(object sender, SelectionChangedEventArgs e)
        {
            OnTipoDocumentoChanged(sender, e, comboBoxTipoDocumentoReclamado13, inputCnpjCpfReclamado13);
        }

        private void OnCpfCnpjTextChangedReclamado13(object sender, TextChangedEventArgs e)
        {
            OnCpfCnpjTextChanged(sender, e, comboBoxTipoDocumentoReclamado13, inputCnpjCpfReclamado13);
        }

        // Reclamado 14
        private void OnTipoDocumentoChangedReclamado14(object sender, SelectionChangedEventArgs e)
        {
            OnTipoDocumentoChanged(sender, e, comboBoxTipoDocumentoReclamado14, inputCnpjCpfReclamado14);
        }

        private void OnCpfCnpjTextChangedReclamado14(object sender, TextChangedEventArgs e)
        {
            OnCpfCnpjTextChanged(sender, e, comboBoxTipoDocumentoReclamado14, inputCnpjCpfReclamado14);
        }

        // Reclamado 15
        private void OnTipoDocumentoChangedReclamado15(object sender, SelectionChangedEventArgs e)
        {
            OnTipoDocumentoChanged(sender, e, comboBoxTipoDocumentoReclamado15, inputCnpjCpfReclamado15);
        }

        private void OnCpfCnpjTextChangedReclamado15(object sender, TextChangedEventArgs e)
        {
            OnCpfCnpjTextChanged(sender, e, comboBoxTipoDocumentoReclamado15, inputCnpjCpfReclamado15);
        }

        // Reclamado 16
        private void OnTipoDocumentoChangedReclamado16(object sender, SelectionChangedEventArgs e)
        {
            OnTipoDocumentoChanged(sender, e, comboBoxTipoDocumentoReclamado16, inputCnpjCpfReclamado16);
        }

        private void OnCpfCnpjTextChangedReclamado16(object sender, TextChangedEventArgs e)
        {
            OnCpfCnpjTextChanged(sender, e, comboBoxTipoDocumentoReclamado16, inputCnpjCpfReclamado16);
        }



    }
}