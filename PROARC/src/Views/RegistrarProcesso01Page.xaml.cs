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

namespace PROARC.src.Views
{
    public sealed partial class RegistrarProcesso01Page : Page, INotifyPropertyChanged
    {
        private string numeroProcesso;

        private List<string> arquivosSelecionados = new();
        public string NumeroProcesso
        {
            get => numeroProcesso;
            set
            {
                numeroProcesso = value;
                OnPropertyChanged(nameof(NumeroProcesso));
            }
        }

        private string anoProcesso;
        public string AnoProcesso
        {
            get => anoProcesso;
            set
            {
                anoProcesso = value;
                OnPropertyChanged(nameof(AnoProcesso));
            }
        }

        public object DragDropEffects { get; private set; }

        public RegistrarProcesso01Page()
        {
            this.InitializeComponent();
            DataContext = this;
            CarregarMotivosAsync();
            ConfigureShadows();
            calendario.Date = DateTime.Now.Date;
            ProcessoNovo_Click(btnProcessoNovo, null);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task CarregarMotivosAsync()
        {
            List<Motivo> motivos = await MotivoControl.GetAllMotivosAsync();
            cbMotivo.ItemsSource = motivos;
        }

        //private async void ProcessoNovo_Click(object sender, RoutedEventArgs e)
        //{
        //    // Marca o primeiro rádio como selecionado
        //    radio_agRealizacaoAudiencia.IsChecked = true;

        //    // Configura estilos dos botões
        //    btnProcessoNovo.Background = new SolidColorBrush(Microsoft.UI.Colors.DarkBlue);
        //    btnProcessoNovo.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);

        //    btnProcessoAntigo.Background = new SolidColorBrush(Microsoft.UI.Colors.White);
        //    btnProcessoAntigo.Foreground = new SolidColorBrush(Microsoft.UI.Colors.DarkBlue);
        //    btnProcessoAntigo.BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.CornflowerBlue);

        //    // Configura campos como somente leitura
        //    inputNProcesso.IsReadOnly = true;
        //    inputAnoProcesso.IsReadOnly = true;

        //    // Reduz a opacidade do painel principal
        //    MainStackPanel.Opacity = 0.4;

        //    // Obtém o número atual de processos, soma 1 e define como número do processo
        //    int count = await ProcessoAdministrativoControl.CountProcessosAsync();
        //    NumeroProcesso = (count + 1).ToString();

        //    // Define o ano do processo
        //    AnoProcesso = "2025";
        //}



        //private void ProcessoAntigo_Click(object sender, RoutedEventArgs e)
        //{
        //    radio_agRealizacaoAudiencia.IsChecked = false;
        //    btnProcessoAntigo.Background = new SolidColorBrush(Microsoft.UI.Colors.DarkBlue);
        //    btnProcessoAntigo.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);

        //    btnProcessoNovo.Background = new SolidColorBrush(Microsoft.UI.Colors.White);
        //    btnProcessoNovo.Foreground = new SolidColorBrush(Microsoft.UI.Colors.DarkBlue);
        //    btnProcessoNovo.BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.CornflowerBlue);

        //    inputNProcesso.IsReadOnly = false;
        //    inputAnoProcesso.IsReadOnly = false;

        //    MainStackPanel.Opacity = 1;
        //    NumeroProcesso = "";
        //    AnoProcesso = "";
        //}

        private void ProcessoNovo_Click(object sender, RoutedEventArgs e)
        {
            ConfigurarEstadoProcesso(isNovoProcesso: true);
        }

        private void ProcessoAntigo_Click(object sender, RoutedEventArgs e)
        {
            ConfigurarEstadoProcesso(isNovoProcesso: false);
        }


        private void ConfigurarEstadoProcesso(bool isNovoProcesso)
        {
            radio_agRealizacaoAudiencia.IsChecked = isNovoProcesso;

            btnProcessoNovo.Background = new SolidColorBrush(isNovoProcesso ? Microsoft.UI.Colors.DarkBlue : Microsoft.UI.Colors.White);
            btnProcessoNovo.Foreground = new SolidColorBrush(isNovoProcesso ? Microsoft.UI.Colors.White : Microsoft.UI.Colors.DarkBlue);

            btnProcessoAntigo.Background = new SolidColorBrush(isNovoProcesso ? Microsoft.UI.Colors.White : Microsoft.UI.Colors.DarkBlue);
            btnProcessoAntigo.Foreground = new SolidColorBrush(isNovoProcesso ? Microsoft.UI.Colors.DarkBlue : Microsoft.UI.Colors.White);
            btnProcessoAntigo.BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.CornflowerBlue);

            inputNProcesso.IsReadOnly = isNovoProcesso;
            inputAnoProcesso.IsReadOnly = isNovoProcesso;

            MainStackPanel.Opacity = isNovoProcesso ? 0.4 : 1;

            if (isNovoProcesso)
            {
                DefinirNovoProcesso();
            }
            else
            {
                LimparProcesso();
            }
        }

        private async void DefinirNovoProcesso()
        {
            int count = await ProcessoAdministrativoControl.CountProcessosAsync();
            NumeroProcesso = (count + 1).ToString();
            AnoProcesso = "2025";
        }

        private void LimparProcesso()
        {
            NumeroProcesso = string.Empty;
            AnoProcesso = string.Empty;
        }


        private void ProcuradorCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ProcuradorSection1.Visibility = Visibility.Visible;
        }

        private void ProcuradorCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ProcuradorSection1.Visibility = Visibility.Collapsed;
        }

        private async void ContinuarButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NumeroProcesso))
            {
                int count = await ProcessoAdministrativoControl.CountProcessosAsync();
                NumeroProcesso = (count + 1).ToString();
            }

            if (!CamposPreenchidos())
            {
                ShowError("Preencha todos os campos obrigatórios antes de continuar.");
                return;
            }

            string motivo = cbMotivo.SelectedItem?.ToString();

            string cpfLimpo1 = new string(inputCnpjCpfReclamado.Text.Where(char.IsDigit).ToArray());
            var reclamado = new Reclamado(
                inputInstituicao.Text ?? "null",
                short.TryParse(inputNumero.Text, out short numero) ? numero : (short?)null,
                inputRua.Text ?? "null",
                inputBairro.Text ?? "null",
                inputEmail.Text ?? "null",
                inputCidade.Text ?? "null",
                inputUf.Text ?? "null",
                cpfLimpo1,
                cpfLimpo1
            );

            string cpfLimpo = new string(inputCpfReclamante.Text.Where(char.IsDigit).ToArray());
            var reclamante = new Reclamante(
                inputNome.Text ?? "null",
                cpfLimpo,
                inputRgReclamante.Text ?? "null"
            );

            string dataAtual = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") ?? "null";
            DateTime? dataSelecionada = calendario.Date?.DateTime;

            string dataFormatada = dataSelecionada.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string caminhoPasta = $"dir/folder{NumeroProcesso}";

            string nProcesso;
            short anoProcesso;

            if (btnProcessoAntigo.IsEnabled)
            {
                nProcesso = inputNProcesso.Text;
                anoProcesso = short.Parse(inputAnoProcesso.Text);
            } else
            {
                nProcesso = NumeroProcesso;
                anoProcesso = 2025;
            }

            ProcessoAdministrativoControl.InsertAsync(
                new(caminhoPasta, nProcesso, anoProcesso, GetSelectedRadioButton(),
                new(motivo),
                reclamado,
                reclamante,
                DateTime.Parse(dataFormatada))
            );

        }

        private bool CamposPreenchidos()
        {
            return !string.IsNullOrWhiteSpace(inputNome.Text) &&
                   !string.IsNullOrWhiteSpace(inputCpfReclamante.Text) &&
                   !string.IsNullOrWhiteSpace(inputRgReclamante.Text) &&
                   !string.IsNullOrWhiteSpace(inputInstituicao.Text) &&
                   !string.IsNullOrWhiteSpace(inputCnpjCpfReclamado.Text) &&
                   cbMotivo.SelectedItem != null &&
                   calendario.Date != null;
        }

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


        private void OnCpfTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            string rawText = new string(textBox.Text.Where(char.IsDigit).ToArray());

            if (rawText.Length > 3)
                rawText = rawText.Insert(3, ".");
            if (rawText.Length > 7)
                rawText = rawText.Insert(7, ".");
            if (rawText.Length > 11)
                rawText = rawText.Insert(11, "-");

            textBox.Text = rawText;
            textBox.SelectionStart = textBox.Text.Length;
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
        }

        private TextBox CriarTextBox(string placeholder, double width)
        {
            return new TextBox
            {
                PlaceholderText = placeholder,
                Width = width
            };
        }

        private StackPanel CriarCampo(string titulo, string placeholder, double width)
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
            CriarTextBox(placeholder, width)
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

            conteudoReclamado.Children.Add(CriarLinhaCampos(
                CriarCampo("Instituição *", "Insira o nome da Instituição", 300),
                CriarCampo("CNPJ/CPF *", "Insira o CNPJ/CPF", 280),
                CriarCampo("E-mail", "Insira o E-mail", 250)
            ));

            conteudoReclamado.Children.Add(CriarLinhaCampos(
                CriarCampo("Rua", "Insira a rua", 300),
                CriarCampo("Bairro", "Insira o bairro", 280),
                CriarCampo("Número", "Insira o número", 120),
                CriarCampo("Cidade", "Insira a cidade", 180),
                CriarCampo("UF", "Insira a UF", 100),
                CriarCampo("CEP", "Insira o CEP", 150)
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
    }
}