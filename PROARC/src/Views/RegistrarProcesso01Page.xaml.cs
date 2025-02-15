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
            set => SetProperty(ref numeroProcesso, value);
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
            int count = await ReclamacaoControl.CountAsync();
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

        private void OnTipoDocumentoChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxTipoDocumento.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedText = selectedItem.Content.ToString();

                // Verifica se o inputCnpjCpfReclamado já foi inicializado antes de acessá-lo
                if (inputCnpjCpfReclamado != null)
                {
                    inputCnpjCpfReclamado.IsEnabled = true;

                    if (selectedText == "CPF")
                    {
                        inputCnpjCpfReclamado.MaxLength = 14;
                        inputCnpjCpfReclamado.PlaceholderText = "Insira o CPF";
                    }
                    else if (selectedText == "CNPJ")
                    {
                        inputCnpjCpfReclamado.MaxLength = 18;
                        inputCnpjCpfReclamado.PlaceholderText = "Insira o CNPJ";
                    }

                    // Limpa o campo ao mudar a opção
                    inputCnpjCpfReclamado.Text = string.Empty;
                }
            }
        }

       private void OnCpfCnpjTextChanged(object sender, TextChangedEventArgs e)
       {
            if (sender is not TextBox textBox) return;

            string selectedType = ((ComboBoxItem)comboBoxTipoDocumento.SelectedItem).Content.ToString();
            string text = Regex.Replace(textBox.Text, @"\D", "");


            int maxLength = selectedType == "CPF" ? 11 : 14;
            if (text.Length > maxLength) text = text.Substring(0, maxLength);

            textBox.Text = FormatDocument(text, selectedType);
            textBox.SelectionStart = textBox.Text.Length;
        }

        private string FormatDocument(string text, string type)
        {
            if (type == "CPF")
                return FormatWithMask(text, new[] {3, 7, 11}, new[] {'.', '.', '-'});
            else if (type == "CNPJ")
                return FormatWithMask(text, new[] {2, 6, 10, 15}, new[] {'.', '.', '/', '-'});
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
            textBox.Text = FormatWithMask(rawText, new[] {3, 7, 11}, new[] {'.', '.', '-'});
            textBox.SelectionStart = textBox.Text.Length;
        }

        private UIElement CreateDialogContent()
        {
            return new TextBox
            {
                PlaceholderText = "Digite o motivo"
            };
        }


        private LinkedList<Reclamado> CriarListaReclamados()
        {
            var listaReclamados = new LinkedList<Reclamado>();

            // Primeiro reclamado (XAML)
            string selectedTipoDocumento = comboBoxTipoDocumento?.SelectedItem?.ToString() ?? string.Empty;
            string cpfCnpjXaml = selectedTipoDocumento == "CPF" ? inputCnpjCpfReclamado?.Text : null;
            string cnpjXaml = selectedTipoDocumento == "CNPJ" ? inputCnpjCpfReclamado?.Text : null;

            var reclamadoXaml = new Reclamado(
                nome: inputInstituicao?.Text ?? throw new ArgumentException("Nome não pode ser nulo"),
                cpf: cpfCnpjXaml,
                cnpj: cnpjXaml,
                numero: short.TryParse(inputNumero?.Text, out short num) ? num : (short)0,
                logradouro: inputRua?.Text ?? throw new ArgumentException("Rua não pode ser nula"),
                bairro: inputBairro?.Text,
                cidade: inputCidade?.Text ?? throw new ArgumentException("Cidade não pode ser nula"),
                uf: inputUf?.Text ?? throw new ArgumentException("UF não pode ser nula"),
                cep: inputCep?.Text ?? throw new ArgumentException("CEP não pode ser nulo"),
                telefone: string.IsNullOrWhiteSpace(inputTelefoneReclamado?.Text) ? null : inputTelefoneReclamado.Text,
                email: string.IsNullOrWhiteSpace(inputEmail?.Text) ? null : inputEmail.Text
            );

            listaReclamados.AddFirst(reclamadoXaml);

            // Reclamados adicionados dinamicamente
            foreach (var elemento in MainContainer.Children)
            {
                if (elemento is StackPanel reclamadoPanel)
                {
                    var campos = reclamadoPanel.Children;

                    if (campos.Count < 12)
                    {
                        Console.WriteLine("Erro: Campos insuficientes no StackPanel.");
                        continue;
                    }

                    string GetTextBoxValue(int index, bool isRequired = false)
                    {
                        if (index >= campos.Count)
                        {
                            Console.WriteLine($"Erro: Campo {index} não encontrado.");
                            return isRequired ? throw new ArgumentException($"Campo {index} é obrigatório.") : null;
                        }

                        if (campos[index] is StackPanel panel && panel.Children.Count > 1 && panel.Children[1] is TextBox textBox)
                        {
                            string value = textBox.Text.Trim();
                            return (isRequired && string.IsNullOrEmpty(value)) ? throw new ArgumentException($"O campo {index} é obrigatório.") : value;
                        }

                        Console.WriteLine($"Erro: Elemento {index} não é um StackPanel com TextBox.");
                        return isRequired ? throw new ArgumentException($"O campo {index} é obrigatório.") : null;
                    }

                    string tipoSelecionado = string.Empty;
                    if (campos[1] is StackPanel stackPanel && stackPanel.Children.Count > 1 && stackPanel.Children[1] is ComboBox tipoDocumentoComboBox)
                    {
                        tipoSelecionado = tipoDocumentoComboBox.SelectedItem?.ToString() ?? string.Empty;
                    }

                    string cpfDinamico = (tipoSelecionado == "CPF") ? GetTextBoxValue(2) : null;
                    string cnpjDinamico = (tipoSelecionado == "CNPJ") ? GetTextBoxValue(3) : null;

                    try
                    {
                        var novoReclamado = new Reclamado(
                            nome: GetTextBoxValue(0, true), // Nome obrigatório
                            cpf: cpfDinamico,               // Opcional
                            cnpj: cnpjDinamico,             // Opcional
                            numero: short.TryParse(GetTextBoxValue(5, true), out short numDinamico) ? numDinamico : throw new ArgumentException("Número inválido"),
                            logradouro: GetTextBoxValue(4, true), // Rua obrigatória
                            bairro: GetTextBoxValue(6),     // Opcional
                            cidade: GetTextBoxValue(7, true), // Cidade obrigatória
                            uf: GetTextBoxValue(8, true),   // UF obrigatória
                            cep: GetTextBoxValue(9, true),  // CEP obrigatório
                            telefone: GetTextBoxValue(10),  // Opcional
                            email: GetTextBoxValue(11)      // Opcional
                        );

                        listaReclamados.AddLast(novoReclamado);
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Erro ao adicionar reclamado: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Erro: Elemento dentro de MainContainer não é um StackPanel.");
                }
            }

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

            Motivo? motivoSelecionado = cbMotivo.SelectedItem != null ? new Motivo(cbMotivo.SelectedItem.ToString()) : null;

            string cpfLimpoReclamante = new string(inputCpfReclamante.Text.Where(char.IsDigit).ToArray());

            var reclamante = new Reclamante(
                inputNome.Text,
                cpfLimpoReclamante,
                string.IsNullOrWhiteSpace(inputRgReclamante.Text) ? null : inputRgReclamante.Text,
                string.IsNullOrWhiteSpace(inputEmailReclamante.Text) ? null : inputEmailReclamante.Text,
                string.IsNullOrWhiteSpace(inputNumeroReclamante.Text) ? null : inputNumeroReclamante.Text
            );

            string dataAtual = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") ?? "null";
            DateTime? dataSelecionada = calendario.Date?.DateTime;
            string dataFormatada = dataSelecionada.HasValue
                ? dataSelecionada.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")
                : "0001-01-01 00:00:00.000";

            string caminhoPasta = $"dir/folder{NumeroProcesso}";
            string nProcesso = NumeroProcesso;
            short anoProcesso = short.TryParse(AnoProcesso, out short parsedAno) ? parsedAno : (short)DateTime.Now.Year;
            string titulo = "G" + nProcesso + "/" + anoProcesso;

            LinkedList<Reclamado> reclamados = CriarListaReclamados();

            var reclamacao = new ReclamacaoGeral(
                motivoSelecionado,
                reclamante,
                null, // Procurador opcional
                reclamados,
                titulo,
                "Em andamento",
                caminhoPasta,
                DateOnly.FromDateTime(DateTime.Now),
                "Sistema",
                dataSelecionada,
                null // Conciliador opcional
            );

            ButtonContinuar.IsEnabled = false;
            bool success = await ReclamacaoControl.InsertAsync(reclamacao);
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



        private void HighlightEmptyFields()
            {
                HighlightField(null, inputNProcesso, TextBlockNProcesso);
                HighlightField(null, inputAnoProcesso, TextBlockAnoProcesso);
                HighlightField(TextBlockReclamante, inputNome, TextBlockNome);
                HighlightField(TextBlockReclamante, inputCpfReclamante, TextBlockCpfReclamante);
                HighlightField(TextBlockConciliador, inputNomeConciliador, TextBlockNomeConciliador);
                HighlightField(TextBlockReclamado, inputInstituicao, TextBlockInstituicao);
                HighlightField(TextBlockReclamado, inputRua, TextBlockRua);
                HighlightField(TextBlockReclamado, inputBairro, TextBlockBairro);
                HighlightField(TextBlockReclamado, inputNumero, TextBlockNumero);
                HighlightField(TextBlockReclamado, inputCidade, TextBlockCidade);
                HighlightField(TextBlockReclamado, inputUf, TextBlockUf);
                HighlightField(TextBlockReclamado, inputCep, TextBlockCep);

                // 🟢 Validação do Motivo (ComboBox)
                if (cbMotivo.SelectedItem == null)
                {
                    cbMotivo.BorderBrush = new SolidColorBrush(Colors.Red);
                    TextBlockMotivo.Foreground = new SolidColorBrush(Colors.Red);
                }

                // 🟢 Validação do Status (Se nenhum RadioButton foi selecionado)
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
            ResetFieldStyle(inputNProcesso, TextBlockNProcesso);
            ResetFieldStyle(inputAnoProcesso, TextBlockAnoProcesso);
            ResetFieldStyle(inputNome, TextBlockNome, TextBlockReclamante);
            ResetFieldStyle(inputCpfReclamante, TextBlockCpfReclamante, TextBlockReclamante);
            ResetFieldStyle(inputInstituicao, TextBlockReclamado, TextBlockInstituicao);
            ResetFieldStyle(inputRua, TextBlockRua, TextBlockReclamado);
            ResetFieldStyle(inputBairro, TextBlockBairro, TextBlockReclamado);
            ResetFieldStyle(inputNumero, TextBlockNumero, TextBlockReclamado);
            ResetFieldStyle(inputCidade, TextBlockCidade, TextBlockReclamado);
            ResetFieldStyle(inputUf, TextBlockUf, TextBlockReclamado);
            ResetFieldStyle(inputCep, TextBlockCep, TextBlockReclamado);

            // 🔵 Resetando o ComboBox do Motivo
            cbMotivo.BorderBrush = new SolidColorBrush(Colors.Gray);
            TextBlockMotivo.Foreground = new SolidColorBrush(Colors.Black);

            // 🔵 Resetando a Seção de Status
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
        private void ResetFieldStyle(TextBox textBox, TextBlock textBlock, TextBlock? titulo = null)
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
        => new[] { inputNome, inputCpfReclamante, inputInstituicao }
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
            ReclamadoSection.Translation = new Vector3(1, 1, 20);
            ConciliadorSection.Translation = new Vector3(1, 1, 20);
        }

        private TextBox CriarTextBox(string placeholder, double width, bool isEnabled = true)
        {
            return new TextBox
            {
                PlaceholderText = placeholder,
                Width = width,
                IsEnabled = isEnabled
            };
        }

        private StackPanel CriarCampo(string titulo, string placeholder, double width, bool isEnabled = true)
        {
            return new StackPanel
            {
                Orientation = Orientation.Vertical,
                Children =
        {
            new TextBlock
            {
                Text = titulo,
                FontSize = 14
            },
            CriarTextBox(placeholder, width, isEnabled)
        }
            };
        }

        private StackPanel CriarCampoComponente(string titulo, UIElement componente)
        {
            return new StackPanel
            {
                Orientation = Orientation.Vertical,
                Children =
        {
            new TextBlock
            {
                Text = titulo,
                FontSize = 14
            },
            componente
        }
            };
        }

        private StackPanel CriarLinhaCampos(params StackPanel[] campos)
        {
            var linha = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 20
            };
            foreach (var campo in campos)
            {
                linha.Children.Add(campo);
            }
            return linha;
        }

        private StackPanel CriarSecaoReclamado()
        {
            var reclamadoContainer = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 32, 0, 0)
            };

            reclamadoContainer.Children.Add(new StackPanel
            {
                Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 51, 102)),
                Width = 10,
                CornerRadius = new CornerRadius(10, 0, 0, 10)
            });

            var reclamadoSection = new Grid
            {
                Background = new SolidColorBrush(Colors.White),
                CornerRadius = new CornerRadius(0, 10, 10, 0),
                Width = 1478,
                Padding = new Thickness(40),
                Shadow = new ThemeShadow()
            };

            reclamadoSection.Translation = new System.Numerics.Vector3(1, 1, 20);

            reclamadoSection.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            reclamadoSection.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            reclamadoSection.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var removerBotao = new Button
            {
                Content = "X",
                Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 51, 102)),
                Foreground = new SolidColorBrush(Colors.White),
                Width = 35,
                Height = 35,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(0, -20, -20, 0)
            };

            removerBotao.Click += (s, e) =>
            {
                if (MainContainer.Children.Contains(reclamadoContainer))
                {
                    MainContainer.Children.Remove(reclamadoContainer);
                }
            };

            Grid.SetRow(removerBotao, 0);
            reclamadoSection.Children.Add(removerBotao);

            var conteudoReclamado = new StackPanel
            {
                Spacing = 10
            };

            conteudoReclamado.Children.Add(new TextBlock
            {
                Text = "Reclamado",
                FontSize = 18,
                FontWeight = FontWeights.Bold
            });

            // Criando ComboBox para CPF/CNPJ
            var comboBoxTipoDocumento = new ComboBox
            {
                PlaceholderText = "Selecione",
                Width = 140,
                FontSize = 14,
                Padding = new Thickness(10)
            };

            comboBoxTipoDocumento.Items.Add(new ComboBoxItem { Content = "CPF", FontSize = 14 });
            comboBoxTipoDocumento.Items.Add(new ComboBoxItem { Content = "CNPJ", FontSize = 14 });

            var inputCnpjCpf = CriarTextBox("Insira CNPJ ou CPF", 188, false);

            comboBoxTipoDocumento.SelectionChanged += (sender, e) =>
            {
                inputCnpjCpf.IsEnabled = comboBoxTipoDocumento.SelectedIndex != -1;
            };

            conteudoReclamado.Children.Add(CriarLinhaCampos(
                CriarCampo("Instituição *", "Insira o nome da Instituição", 300),
                CriarCampoComponente("Tipo de Documento", comboBoxTipoDocumento),
                CriarCampoComponente("CNPJ/CPF *", inputCnpjCpf),
                CriarCampo("E-mail", "Insira o E-mail", 250),
                CriarCampo("Telefone", "Insira o Telefone", 250)
            ));

            conteudoReclamado.Children.Add(CriarLinhaCampos(
                CriarCampo("Rua *", "Insira a rua", 300),
                CriarCampo("Bairro *", "Insira o bairro", 280),
                CriarCampo("Número *", "Insira o número", 120),
                CriarCampo("Cidade *", "Insira a cidade", 180),
                CriarCampo("UF *", "Insira a UF", 100),
                CriarCampo("CEP *", "Insira o CEP", 150)
            ));

            Grid.SetRow(conteudoReclamado, 1);
            reclamadoSection.Children.Add(conteudoReclamado);

            reclamadoContainer.Children.Add(reclamadoSection);

            return reclamadoContainer;
        }

        private void OnAddReclamadoClick(object sender, RoutedEventArgs e)
        {
            MainContainer.Children.Add(CriarSecaoReclamado());
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is bool isNovoProcesso)
            {
                ConfigurarEstadoProcesso(isNovoProcesso);
            }
        }
    }
}