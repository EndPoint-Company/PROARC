using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;
using Windows.ApplicationModel.DataTransfer;
using PROARC.src.Models;
using PROARC.src.Control;
using PROARC.src.Converters;
using System.ComponentModel;
using Microsoft.UI.Xaml.Media;
using System.Drawing;
using System.Numerics;
using Windows.Storage;
using Microsoft.UI.Text;
using Microsoft.UI;
using PROARC.src.Models.Tipos;
using PROARC.src.Models.Arquivos;
using static PROARC.src.Control.MotivoControl;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.UI.Popups;
using Microsoft.UI.Xaml.Controls.Primitives;
using Windows.Globalization;
using System.Text.RegularExpressions;
using System.Globalization;

namespace PROARC.src.Views
{
    public sealed partial class RegistrarProcessoEnelPage : Page, INotifyPropertyChanged
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

        public RegistrarProcessoEnelPage()
        {
            InitializeComponent();
            DataContext = this;
            CarregarMotivosAsync();
            ConfigureShadows();
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

            }
            else
            {
                NumeroProcesso = string.Empty;
                AnoProcesso = string.Empty;
            }
        }

        private async Task DefinirNovoProcesso()
        {
            int count = await ReclamacaoControl.CountEAsync();
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

        //public bool Validacaos()
        //{
        //    if (!Validacoes.ValidarCPF(inputCpfReclamante.Text))
        //    {
        //        ShowError("CPF inválido do Reclamante é Invalido.");
        //        return false;
        //    }
        //    else if (!Validacoes.ValidarEmail(inputEmailReclamante.Text))
        //    {
        //        ShowError("E-mail inválido.");
        //        return false;
        //    }
        //    else if (!Validacoes.ValidarTelefone(inputNumeroReclamante.Text))
        //    {
        //        ShowError("Telefone inválido.");
        //        return false;
        //    }
        //    return true;
        //}

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
                        string tituloPasta = $"E{NumeroProcesso}-{AnoProcesso}"; // Título da pasta
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
                inputNomeReclamante.Text,
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

            string caminhoPasta = $"dir/folder{NumeroProcesso}";
            string nProcesso = NumeroProcesso;
            short anoProcesso = short.TryParse(AnoProcesso, out short parsedAno) ? parsedAno : (short)DateTime.Now.Year;
            string titulo = "E" + nProcesso + "/" + anoProcesso;

            string atendente = inputNomeAtendente.Text;
            string situacao = GetSelectedRadioButton();
            string observacao = inputObservacao.Text;

            Reclamado? enel = await ReclamadoControl.GetAsync(74);

            if (enel != null)
            {
                LinkedList<Reclamado> reclamados = new LinkedList<Reclamado>();
                reclamados.AddLast(enel);

                var reclamacaoEnel = new ReclamacaoEnel
                (
                    motivoSelecionado,
                    reclamante,
                    procurador,
                    reclamados,
                    titulo,
                    situacao,
                    caminhoPasta,
                    DateOnly.FromDateTime(DateTime.Now),
                    "Sistema",
                    atendente,
                    enel.Telefone,
                    enel.Email,
                    observacao
                );

                ButtonContinuar.IsEnabled = false;
                bool success = await ReclamacaoControl.InsertAsync(reclamacaoEnel);
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
        }

        private void HighlightEmptyFields()
        {
            HighlightField(null, inputNProcesso, TextBlockNProcesso);
            HighlightField(null, inputAnoProcesso, TextBlockAnoProcesso);
            HighlightField(TextBlockReclamante, inputNomeReclamante, TextBlockNomeReclamante);
            HighlightField(TextBlockReclamante, inputCpfReclamante, TextBlockCpfReclamante);

            // ?? Validação do Motivo (ComboBox)
            if (cbMotivo.SelectedItem == null)
            {
                cbMotivo.BorderBrush = new SolidColorBrush(Colors.Red);
                TextBlockMotivo.Foreground = new SolidColorBrush(Colors.Red);
            }

            // ?? Validação do Status (Se nenhum RadioButton foi selecionado)
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

        // ?? Método para Resetar os Estilos de Erro
        private void ResetErrorStyles()
        {
            ResetFieldStyle(inputNProcesso, TextBlockNProcesso);
            ResetFieldStyle(inputAnoProcesso, TextBlockAnoProcesso);
            ResetFieldStyle(inputNomeReclamante, TextBlockNomeReclamante, TextBlockReclamante);
            ResetFieldStyle(inputCpfReclamante, TextBlockCpfReclamante, TextBlockReclamante);

            // ?? Resetando o ComboBox do Motivo
            cbMotivo.BorderBrush = new SolidColorBrush(Colors.Gray);
            TextBlockMotivo.Foreground = new SolidColorBrush(Colors.Black);

            // ?? Resetando a Seção de Status
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

        // ?? Método Genérico para Resetar o Estilo de um Campo
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

        // ? Verifica se algum RadioButton do Status foi selecionado
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
        => new[] { inputNomeReclamante, inputCpfReclamante }
            .All(campo => !string.IsNullOrWhiteSpace(campo.Text))
            && cbMotivo.SelectedItem != null
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

                if (!ListaArquivos.Children.OfType<TextBlock>().Any(tb => tb.Text == nomeArquivo))
                {
                    var textBlock = new TextBlock
                    {
                        Text = nomeArquivo,
                        Tag = arquivo, // Armazena o caminho completo no Tag
                        FontSize = 14,
                        Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green)
                    };

                    // Adiciona um ícone de sucesso (opcional)
                    var icon = new SymbolIcon(Symbol.Accept);
                    var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
                    stackPanel.Children.Add(icon);
                    stackPanel.Children.Add(textBlock);

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
            StatusSection.Translation = new Vector3(1, 1, 20);
            ReclamanteSection.Translation = new Vector3(1, 1, 20);
            ProcuradorSection2.Translation = new Vector3(1, 1, 20);
            AnexarArquivosSection.Translation = new Vector3(1, 1, 20);
            AtendenteSection.Translation = new Vector3(1, 1, 20);
            ObservacaoSection.Translation = new Vector3(1, 1, 20);
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