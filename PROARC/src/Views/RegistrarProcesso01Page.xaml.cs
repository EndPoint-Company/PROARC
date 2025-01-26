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

            ConfigureShadows();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ProcessoNovo_Click(object sender, RoutedEventArgs e)
        {

            // Alterar estilo do bot�o "Processo Novo" para selecionado
            btnProcessoNovo.Background = new SolidColorBrush(Microsoft.UI.Colors.DarkBlue); // Cor "#003366"
            btnProcessoNovo.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);

            // Alterar estilo do bot�o "Processo Antigo" para n�o selecionado
            btnProcessoAntigo.Background = new SolidColorBrush(Microsoft.UI.Colors.White);
            btnProcessoAntigo.Foreground = new SolidColorBrush(Microsoft.UI.Colors.DarkBlue); // Cor "#003366"
            btnProcessoAntigo.BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.CornflowerBlue); // Cor "#005BC1"

            // Permitir edi��o dos campos
            inputNProcesso.IsReadOnly = false;
            inputAnoProcesso.IsReadOnly = false;

            MainStackPanel.Opacity = 1; // Torna o painel totalmente vis�vel
            NumeroProcesso = ""; // Limpa os campos
            AnoProcesso = "";
        }

        private void ProcessoAntigo_Click(object sender, RoutedEventArgs e)
        {

            // Alterar estilo do bot�o "Processo Antigo" para selecionado
            btnProcessoAntigo.Background = new SolidColorBrush(Microsoft.UI.Colors.DarkBlue); // Cor "#003366"
            btnProcessoAntigo.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);

            // Alterar estilo do bot�o "Processo Novo" para n�o selecionado
            btnProcessoNovo.Background = new SolidColorBrush(Microsoft.UI.Colors.White);
            btnProcessoNovo.Foreground = new SolidColorBrush(Microsoft.UI.Colors.DarkBlue); // Cor "#003366"
            btnProcessoNovo.BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.CornflowerBlue); // Cor "#005BC1"

            // Tornar os campos somente leitura
            inputNProcesso.IsReadOnly = true;
            inputAnoProcesso.IsReadOnly = true;

            MainStackPanel.Opacity = 0.4; // Define opacidade de 40%
            NumeroProcesso = "12345"; // Preenche os campos com valores
            AnoProcesso = "2023";
        }



        private void ProcuradorCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Torna a se��o "Procurador" vis�vel quando o checkbox est� marcado
            ProcuradorSection1.Visibility = Visibility.Visible;
        }

        private void ProcuradorCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Oculta a se��o "Procurador" quando o checkbox est� desmarcado
            ProcuradorSection1.Visibility = Visibility.Collapsed;
        }

        private void ContinuarButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessoAdministrativoControl.Insert
                (new(@"dir/folder", "titulo", 2025, new("Cobran�a indevida"),
                null, new("Junin", "11122266677", "rgmassa"), DateTime.Now)); 
            // S� inserir os dados das caixas aqui.
            // A parte do Reclamado deve ser feita no servidor.
        }


        // Bot�o para abrir o seletor de arquivos
        private async void PickFileButton_Click(object sender, RoutedEventArgs e)
        {
            // Instancia o FileOpenPicker
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            // Adiciona os filtros de tipo de arquivo
            picker.FileTypeFilter.Add("*"); // Permite qualquer arquivo

            // Associa o FileOpenPicker � janela do aplicativo
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

            // Mostra o seletor de arquivos e espera a sele��o
            var files = await picker.PickMultipleFilesAsync();

            if (files != null && files.Any())
            {
                // Adiciona os arquivos selecionados � lista
                AdicionarArquivos(files.Select(file => file.Path));
            }
        }

        // M�todo para adicionar arquivos � lista visual
        private void AdicionarArquivos(IEnumerable<string> arquivos)
        {
            foreach (var arquivo in arquivos)
            {
                var nomeArquivo = System.IO.Path.GetFileName(arquivo);

                // Verifica se o arquivo j� foi adicionado
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

        // Atualiza a mensagem de "Nenhum arquivo selecionado"
        private void AtualizarMensagemNenhumArquivo()
        {
            MensagemNenhumArquivo.Visibility = ListaArquivos.Children.OfType<TextBlock>().Any()
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        // Suporte a Drag and Drop para arquivos
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
            // Cont�iner principal da se��o de Reclamado
            var reclamadoContainer = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 32, 0, 0)
            };

            // Cont�iner da borda azul
            reclamadoContainer.Children.Add(new StackPanel
            {
                Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 51, 102)),
                Width = 10,
                CornerRadius = new CornerRadius(10, 0, 0, 10)
            });

            // Grid principal para a se��o
            var reclamadoSection = new Grid
            {
                Background = new SolidColorBrush(Colors.White),
                CornerRadius = new CornerRadius(0, 10, 10, 0),
                Width = 1478,
                Padding = new Thickness(40),
                Shadow = new ThemeShadow()
            };

            reclamadoSection.Translation = new System.Numerics.Vector3(1, 1, 20);

            // Defini��o de linhas e colunas no Grid
            reclamadoSection.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Linha para o bot�o "X"
            reclamadoSection.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Linha para o conte�do
            reclamadoSection.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Bot�o "X" para remover a se��o
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
                Margin = new Thickness(0, -20, -20, 0) // Ajuste para posicionar fora do Grid
            };

            // Evento para remover o cont�iner
            removerBotao.Click += (s, e) =>
            {
                if (MainContainer.Children.Contains(reclamadoContainer))
                {
                    MainContainer.Children.Remove(reclamadoContainer);
                }
            };

            // Adicionar o bot�o "X" ao Grid (linha 0, coluna 0)
            Grid.SetRow(removerBotao, 0);
            reclamadoSection.Children.Add(removerBotao);

            // Conte�do da se��o
            var conteudoReclamado = new StackPanel
            {
                Spacing = 10
            };

            // T�tulo
            conteudoReclamado.Children.Add(new TextBlock
            {
                Text = "Reclamado",
                FontSize = 18,
                FontWeight = FontWeights.Bold
            });

            // Primeira linha de campos
            conteudoReclamado.Children.Add(CriarLinhaCampos(
                CriarCampo("Institui��o *", "Insira o nome da Institui��o", 300),
                CriarCampo("CNPJ/CPF *", "Insira o CNPJ/CPF", 280),
                CriarCampo("E-mail", "Insira o E-mail", 250)
            ));

            // Segunda linha de campos
            conteudoReclamado.Children.Add(CriarLinhaCampos(
                CriarCampo("Rua", "Insira a rua", 300),
                CriarCampo("Bairro", "Insira o bairro", 280),
                CriarCampo("N�mero", "Insira o n�mero", 120),
                CriarCampo("Cidade", "Insira a cidade", 180),
                CriarCampo("UF", "Insira a UF", 100),
                CriarCampo("CEP", "Insira o CEP", 150)
            ));

            // Adicionar o conte�do ao Grid (linha 1, coluna 0)
            Grid.SetRow(conteudoReclamado, 1);
            reclamadoSection.Children.Add(conteudoReclamado);

            // Adicionar a se��o ao cont�iner principal
            reclamadoContainer.Children.Add(reclamadoSection);

            return reclamadoContainer;
        }


        private void OnAddReclamadoClick(object sender, RoutedEventArgs e)
        {
            // Adicionar uma nova se��o ao MainContainer
            MainContainer.Children.Add(CriarSecaoReclamado());
        }
    }
}