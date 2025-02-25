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
using System.Net.Sockets;
using System.Text;

namespace PROARC.src.Views
{
    public sealed partial class RegistrarProcesso01Page : Page, INotifyPropertyChanged
    {
        private string numeroProcesso;
        private string anoProcesso;
        private List<string> arquivosSelecionados = new();

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

        public RegistrarProcesso01Page()
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
            int count = await ReclamacaoControl.CountReclamacoesGeralPorAnoAsync();
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

        private async void OnNovoMotivoClick(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Adicionar Novo Motivo",
                Content = CreateDialogContent(),
                PrimaryButtonText = "Salvar",
                CloseButtonText = "Cancelar",
                XamlRoot = this.Content.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var motivoTexto = ((TextBox)dialog.Content).Text;

                if (!string.IsNullOrWhiteSpace(((TextBox)dialog.Content).Text))
                {
                    var motivosExistentes = await MotivoControl.GetAllAsync();

                    if (motivosExistentes.Any(m => m.Nome.Equals(motivoTexto, StringComparison.OrdinalIgnoreCase)))
                    {
                        var errorDialog = new ContentDialog
                        {
                            Title = "Erro",
                            Content = "Este motivo já existe.",
                            CloseButtonText = "Ok",
                            XamlRoot = this.Content.XamlRoot
                        };
                        await errorDialog.ShowAsync();
                    }
                    else
                    {
                        var motivo = new Motivo(motivoTexto, null);

                        try
                        {
                            await MotivoControl.InsertAsync(motivo);
                            await CarregarMotivosAsync();

                            var successDialog = new ContentDialog
                            {
                                Title = "Sucesso",
                                Content = "Motivo salvo com sucesso!",
                                CloseButtonText = "Ok",
                                XamlRoot = this.Content.XamlRoot
                            };

                            await successDialog.ShowAsync();
                        }
                        catch (Exception ex)
                        {
                            var errorDialog = new ContentDialog
                            {
                                Title = "Erro",
                                Content = $"Falha ao salvar motivo: {ex.Message}",
                                CloseButtonText = "Ok",
                                XamlRoot = this.Content.XamlRoot
                            };

                            await errorDialog.ShowAsync();
                        }
                    }
                }
                else
                {
                    var errorDialog = new ContentDialog
                    {
                        Title = "Erro",
                        Content = "O motivo não pode estar vazio.",
                        CloseButtonText = "Ok",
                        XamlRoot = this.Content.XamlRoot
                    };

                    await errorDialog.ShowAsync();
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


        private UIElement CreateDialogContent()
        {
            return new TextBox
            {
                PlaceholderText = "Digite o motivo"
            };
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
                    //Debug.WriteLine($"Reclamado {i}: Nome encontrado: {inputNomeReclamado.Text}");

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
                    //Debug.WriteLine($"Reclamado {i} adicionado à lista.");
                }
                else
                {
                    //Debug.WriteLine($"Reclamado {i}: Nome não preenchido ou controle não encontrado.");
                }
            }

            // Retorna a lista de reclamados
            //Debug.WriteLine($"Total de reclamados criados: {listaReclamados.Count}");
            return listaReclamados;
        }





        private async void ContinuarButton_Click(object sender, RoutedEventArgs e)
        {
            // Resetando estilos de erro
            ResetErrorStyles();

            NumeroProcesso = inputNProcesso.Text.Trim();
            AnoProcesso = inputAnoProcesso.Text.Trim();

            if (string.IsNullOrEmpty(NumeroProcesso))
            {
                int count = await ReclamacaoControl.CountAsync();
                NumeroProcesso = (count + 1).ToString();
            }

            if (!CamposPreenchidos())
            {
                ShowError("Preencha todos os campos obrigatórios antes de continuar.");
                HighlightEmptyFields(); // 🔴 Adiciona bordas vermelhas nos campos vazios
                return;
            }

            // Coletar os arquivos anexados (caminhos completos)
            var arquivosAnexados = ListaArquivos.Children.OfType<StackPanel>()
                .Select(stackPanel => stackPanel.Children.OfType<TextBlock>().FirstOrDefault()?.Tag?.ToString())
                .Where(path => !string.IsNullOrEmpty(path))
                .ToList();

            // Log dos arquivos a serem enviados
            if (arquivosAnexados.Any())
            {
                foreach (var arquivo in arquivosAnexados)
                {
                    Debug.WriteLine($"Arquivo a ser enviado: {arquivo}");
                }

                // Enviar cada arquivo para o servidor
                foreach (var caminhoArquivo in arquivosAnexados)
                {
                    if (File.Exists(caminhoArquivo))
                    {
                        string tituloPasta = $"G{NumeroProcesso}-{AnoProcesso}"; // Título da pasta
                        Debug.WriteLine($"Enviando arquivo: {caminhoArquivo} para a pasta: {tituloPasta}");
                        await FileNetworkControl.SendFile(caminhoArquivo, tituloPasta); // Chama o método do controlador
                    }
                    else
                    {
                        Debug.WriteLine($"Arquivo não encontrado: {caminhoArquivo}");
                    }
                }
            }
            else
            {
                Debug.WriteLine("Nenhum arquivo encontrado para enviar.");
            }

            // Restante da lógica para salvar o processo...
            Motivo? motivoSelecionado = cbMotivo.SelectedItem != null ? new Motivo(cbMotivo.SelectedItem.ToString()) : null;

            string cpfLimpoReclamante = new string(inputCpfReclamante.Text.Where(char.IsDigit).ToArray());

            var reclamante = new Reclamante(
                inputNome.Text,
                cpfLimpoReclamante,
                string.IsNullOrWhiteSpace(inputRgReclamante.Text) ? null : inputRgReclamante.Text,
                string.IsNullOrWhiteSpace(inputNumeroReclamante.Text) ? null : inputNumeroReclamante.Text,
                string.IsNullOrWhiteSpace(inputEmailReclamante.Text) ? null : inputEmailReclamante.Text
            );

            Procurador procurador = null;

            if (ProcuradorCheckBox.IsChecked == true)
            {
                string cpfLimpoProcurador = new string(inputCpfProcurador.Text.Where(char.IsDigit).ToArray());
                procurador = new Procurador(
                inputNomeProcurador.Text,
                cpfLimpoProcurador,
                string.IsNullOrWhiteSpace(inputRgProcurador.Text) ? null : inputRgProcurador.Text,
                string.IsNullOrWhiteSpace(inputNumeroProcurador.Text) ? null : inputNumeroProcurador.Text,
                string.IsNullOrWhiteSpace(inputEmailProcurador.Text) ? null : inputEmailProcurador.Text
                );
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

            var reclamacao = new ReclamacaoGeral(
                motivoSelecionado,
                reclamante,
                procurador, // procurador
                reclamados,
                titulo,
                situacao,
                caminhoPasta,
                DateOnly.FromDateTime(DateTime.Now),
                "Sistema",
                dataSelecionada,
                conciliador
            );

            ButtonContinuar.IsEnabled = false;
            bool success = await ReclamacaoControl.InsertAsyncG(reclamacao);
            ButtonContinuar.IsEnabled = true;

            if (success)
            {
                var successDialog = new ContentDialog
                {
                    Title = "Sucesso",
                    Content = "O processo foi cadastrado com sucesso!",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };

                await successDialog.ShowAsync();
                Frame.Navigate(typeof(RegistrarProcesso01Page), true);
            }
            else
            {
                ShowError("Falha ao cadastrar o processo. Tente novamente.");
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





        private void HighlightEmptyFields()
        {
            // Obtém a quantidade de reclamados selecionada no ComboBox
            int quantidadeReclamados = ObterQuantidadeReclamadosSelecionada();

            // Validação dos campos comuns (não relacionados aos reclamados)
            HighlightField(null, inputNProcesso, TextBlockNProcesso);
            HighlightField(null, inputAnoProcesso, TextBlockAnoProcesso);
            HighlightField(TextBlockReclamante, inputNome, TextBlockNome);
            HighlightField(TextBlockReclamante, inputCpfReclamante, TextBlockCpfReclamante);
            HighlightField(TextBlockConciliador, inputNomeConciliador, TextBlockNomeConciliador);

            // Validação dos campos dos reclamados (apenas os visíveis)
            for (int i = 1; i <= quantidadeReclamados; i++)
            {
                // Obtém os controles do reclamado atual
                var textBlockReclamado = FindName($"textBlockReclamado{i:00}") as TextBlock;
                var inputNomeReclamado = FindName($"inputNomeReclamado{i:00}") as TextBox;
                var inputRuaReclamado = FindName($"inputRuaReclamado{i:00}") as TextBox;
                var inputNumeroReclamado = FindName($"inputNumeroReclamado{i:00}") as TextBox;
                var inputBairroReclamado = FindName($"inputBairroReclamado{i:00}") as TextBox;
                var inputCidadeReclamado = FindName($"inputCidadeReclamado{i:00}") as TextBox;
                var inputUfReclamado = FindName($"comboBoxUfReclamado{i:00}") as TextBox;
                var inputCepReclamado = FindName($"inputCepReclamado{i:00}") as TextBox;

                // Valida os campos do reclamado atual
                HighlightField(textBlockReclamado, inputNomeReclamado, FindName($"TextBlockNomeReclamado{i:00}") as TextBlock);
                HighlightField(textBlockReclamado, inputRuaReclamado, FindName($"TextBlockRuaReclamado{i:00}") as TextBlock);
                HighlightField(textBlockReclamado, inputNumeroReclamado, FindName($"TextBlockNumeroReclamado{i:00}") as TextBlock);
                HighlightField(textBlockReclamado, inputBairroReclamado, FindName($"TextBlockBairroReclamado{i:00}") as TextBlock);
                HighlightField(textBlockReclamado, inputCidadeReclamado, FindName($"TextBlockCidadeReclamado{i:00}") as TextBlock);
                //HighlightField(textBlockReclamado, inputUfReclamado, FindName($"TextBlockUfReclamado{i:00}") as TextBlock);
                HighlightField(textBlockReclamado, inputCepReclamado, FindName($"TextBlockCepReclamado{i:00}") as TextBlock);
            }

            // Validação do Motivo (ComboBox)
            if (cbMotivo.SelectedItem == null)
            {
                cbMotivo.BorderBrush = new SolidColorBrush(Colors.Red);
                TextBlockMotivo.Foreground = new SolidColorBrush(Colors.Red);
            }

            // Validação do Status (Se nenhum RadioButton foi selecionado)
            if (!IsStatusSelected())
            {
                StatusSection.BorderBrush = new SolidColorBrush(Colors.Red);
                TextBlockStatus.Foreground = new SolidColorBrush(Colors.Red);
                TextBlockTramitacao.Foreground = new SolidColorBrush(Colors.Red);

                radio_agFazerNotificacao.Foreground = new SolidColorBrush(Colors.Red);
                radio_agRealizacaoAudiencia.Foreground = new SolidColorBrush(Colors.Red);
                radio_agResposta.Foreground = new SolidColorBrush(Colors.Red);
                radio_agEnvioNotificacao.Foreground = new SolidColorBrush(Colors.Red);
                radio_agDocumentacao.Foreground = new SolidColorBrush(Colors.Red);

                TextBlockArquivado.Foreground = new SolidColorBrush(Colors.Red);
                radio_atendido.Foreground = new SolidColorBrush(Colors.Red);
                radio_naoAtendido.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void HighlightField(TextBlock? titulo, TextBox textBox, TextBlock textBlock)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                textBox.PlaceholderForeground = new SolidColorBrush(Colors.Red);
                textBlock.Foreground = new SolidColorBrush(Colors.Red);

                if (titulo != null)
                {
                    titulo.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
        }

        // 🔵 Método para Resetar os Estilos de Erro
        private void ResetErrorStyles()
        {
            // Reseta os campos comuns
            ResetFieldStyle(null, inputNProcesso, TextBlockNProcesso);
            ResetFieldStyle(null, inputAnoProcesso, TextBlockAnoProcesso);
            ResetFieldStyle(TextBlockReclamante, inputNome, TextBlockNome);
            ResetFieldStyle(TextBlockReclamante, inputCpfReclamante, TextBlockCpfReclamante);

            // Obtém a quantidade de reclamados selecionada no ComboBox
            int quantidadeReclamados = ObterQuantidadeReclamadosSelecionada();

            // Reseta os campos dos reclamados (apenas os visíveis)
            for (int i = 1; i <= quantidadeReclamados; i++)
            {
                // Obtém os controles do reclamado atual
                var textBlockReclamado = FindName($"textBlockReclamado{i:00}") as TextBlock;
                var inputNomeReclamado = FindName($"inputNomeReclamado{i:00}") as TextBox;
                var inputRuaReclamado = FindName($"inputRuaReclamado{i:00}") as TextBox;
                var inputNumeroReclamado = FindName($"inputNumeroReclamado{i:00}") as TextBox;
                var inputBairroReclamado = FindName($"inputBairroReclamado{i:00}") as TextBox;
                var inputCidadeReclamado = FindName($"inputCidadeReclamado{i:00}") as TextBox;
                var inputUfReclamado = FindName($"comboBoxUfReclamado{i:00}") as TextBox;
                var inputCepReclamado = FindName($"inputCepReclamado{i:00}") as TextBox;

                // Reseta os campos do reclamado atual
                ResetFieldStyle(textBlockReclamado, inputNomeReclamado, FindName($"TextBlockNomeReclamado{i:00}") as TextBlock);
                ResetFieldStyle(textBlockReclamado, inputRuaReclamado, FindName($"TextBlockRuaReclamado{i:00}") as TextBlock);
                ResetFieldStyle(textBlockReclamado, inputNumeroReclamado, FindName($"TextBlockNumeroReclamado{i:00}") as TextBlock);
                ResetFieldStyle(textBlockReclamado, inputBairroReclamado, FindName($"TextBlockBairroReclamado{i:00}") as TextBlock);
                ResetFieldStyle(textBlockReclamado, inputCidadeReclamado, FindName($"TextBlockCidadeReclamado{i:00}") as TextBlock);
                //ResetFieldStyle(textBlockReclamado, inputUfReclamado, FindName($"TextBlockUfReclamado{i:00}") as TextBlock);
                ResetFieldStyle(textBlockReclamado, inputCepReclamado, FindName($"TextBlockCepReclamado{i:00}") as TextBlock);
            }

            // Reseta o ComboBox do Motivo
            cbMotivo.BorderBrush = new SolidColorBrush(Colors.Gray);
            TextBlockMotivo.Foreground = new SolidColorBrush(Colors.Black);

            // Reseta a Seção de Status
            StatusSection.BorderBrush = new SolidColorBrush(Colors.Transparent);
            TextBlockStatus.Foreground = new SolidColorBrush(Colors.Black);
            TextBlockTramitacao.Foreground = new SolidColorBrush(Colors.Black);

            radio_agFazerNotificacao.Foreground = new SolidColorBrush(Colors.Black);
            radio_agRealizacaoAudiencia.Foreground = new SolidColorBrush(Colors.Black);
            radio_agResposta.Foreground = new SolidColorBrush(Colors.Black);
            radio_agEnvioNotificacao.Foreground = new SolidColorBrush(Colors.Black);
            radio_agDocumentacao.Foreground = new SolidColorBrush(Colors.Black);

            TextBlockArquivado.Foreground = new SolidColorBrush(Colors.Black);
            radio_atendido.Foreground = new SolidColorBrush(Colors.Black);
            radio_naoAtendido.Foreground = new SolidColorBrush(Colors.Black);
        }

        // 🔵 Método Genérico para Resetar o Estilo de um Campo
        private void ResetFieldStyle(TextBlock? titulo, TextBox textBox, TextBlock textBlock)
        {
            textBox.BorderBrush = new SolidColorBrush(Colors.Gray);
            textBox.PlaceholderForeground = new SolidColorBrush(Colors.DarkGray);
            textBlock.Foreground = new SolidColorBrush(Colors.Black);

            if (titulo != null)
            {
                titulo.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        // ✅ Verifica se algum RadioButton do Status foi selecionado
        private bool IsStatusSelected()
        {
            return radio_agFazerNotificacao.IsChecked == true ||
                   radio_agRealizacaoAudiencia.IsChecked == true ||
                   radio_agResposta.IsChecked == true ||
                   radio_agEnvioNotificacao.IsChecked == true ||
                   radio_agDocumentacao.IsChecked == true ||
                   radio_atendido.IsChecked == true ||
                   radio_naoAtendido.IsChecked == true;
        }

        private bool CamposPreenchidos()
        => new[] { inputNome, inputCpfReclamante }
            .All(campo => !string.IsNullOrWhiteSpace(campo.Text))
            && cbMotivo.SelectedItem != null
            && calendario.Date != null
            && GetSelectedRadioButton() != "Nenhum status selecionado";

        private async void ShowError(string mensagemErro)
        {
            var dialog = new ContentDialog
            {
                Title = "Erro de Validação",
                Content = mensagemErro,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await dialog.ShowAsync();
        }

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

            // Adiciona filtros para tipos de arquivo permitidos
            picker.FileTypeFilter.Add(".pdf");
            picker.FileTypeFilter.Add(".docx");

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

                if (!ListaArquivos.Children.OfType<StackPanel>().Any(sp => sp.Children.OfType<TextBlock>().Any(tb => tb.Text == nomeArquivo)))
                {
                    var textBlock = new TextBlock
                    {
                        Text = nomeArquivo,
                        Tag = arquivo, // Armazena o caminho completo no Tag
                        FontSize = 14,
                        Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green)
                    };

                    var button = new Button
                    {
                        Content = "X",
                        Background = new SolidColorBrush(Microsoft.UI.Colors.Blue),
                        Foreground = new SolidColorBrush(Microsoft.UI.Colors.White),
                        Margin = new Thickness(10, 0, 0, 0),
                        Padding = new Thickness(5),
                        CornerRadius = new CornerRadius(5)
                    };

                    button.Click += (sender, e) =>
                    {
                        var stackPanel = (StackPanel)((Button)sender).Parent;
                        ListaArquivos.Children.Remove(stackPanel);
                        AtualizarMensagemNenhumArquivo();
                    };

                    var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
                    stackPanel.Children.Add(textBlock);
                    stackPanel.Children.Add(button);

                    ListaArquivos.Children.Add(stackPanel);

                    // Log para verificar o arquivo adicionado
                    Debug.WriteLine($"Arquivo adicionado: {nomeArquivo}, Caminho: {arquivo}");
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

            CadastrarEmpresaSectionReclamado01.Translation = new Vector3(1, 1, 20);
            CadastrarEmpresaSectionReclamado02.Translation = new Vector3(1, 1, 20);
            CadastrarEmpresaSectionReclamado03.Translation = new Vector3(1, 1, 20);
            CadastrarEmpresaSectionReclamado04.Translation = new Vector3(1, 1, 20);
            CadastrarEmpresaSectionReclamado05.Translation = new Vector3(1, 1, 20);
            CadastrarEmpresaSectionReclamado06.Translation = new Vector3(1, 1, 20);
            CadastrarEmpresaSectionReclamado07.Translation = new Vector3(1, 1, 20);
            CadastrarEmpresaSectionReclamado08.Translation = new Vector3(1, 1, 20);
            CadastrarEmpresaSectionReclamado09.Translation = new Vector3(1, 1, 20);
            CadastrarEmpresaSectionReclamado10.Translation = new Vector3(1, 1, 20);
            CadastrarEmpresaSectionReclamado11.Translation = new Vector3(1, 1, 20);
            CadastrarEmpresaSectionReclamado12.Translation = new Vector3(1, 1, 20);
            CadastrarEmpresaSectionReclamado13.Translation = new Vector3(1, 1, 20);
            CadastrarEmpresaSectionReclamado14.Translation = new Vector3(1, 1, 20);
            CadastrarEmpresaSectionReclamado15.Translation = new Vector3(1, 1, 20);
            CadastrarEmpresaSectionReclamado16.Translation = new Vector3(1, 1, 20);

            CadastrarEmpresaSectionReclamado01.Translation = new Vector3(1, 1, 20);

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