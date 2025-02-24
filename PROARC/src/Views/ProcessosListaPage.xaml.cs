using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Newtonsoft.Json.Linq;
using PROARC.src.Control;
using PROARC.src.Models;
using PROARC.src.Models.Arquivos;
using PROARC.src.Models.Tipos;
using PROARC.src.ViewModels;

namespace PROARC.src.Views
{
    public sealed partial class ProcessosListaPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<ReclamacaoViewModel> Processos { get; set; } = new();
        private bool _isLoading;
        private int _limit = 15;
        private int _offset = 0;
        private int _paginaAtual = 1;
        private int _totalProcessos = 0;

        public int PaginaAtual
        {
            get => _paginaAtual;
            set
            {
                if (_paginaAtual != value)
                {
                    _paginaAtual = value;
                    OnPropertyChanged(nameof(PaginaAtual));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            });
        }

        private async void Processo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Status")
            {
                if (sender is ReclamacaoViewModel processoViewModel)
                {
                    var modeloOriginal = processoViewModel.ObterModeloOriginal();


                    if (modeloOriginal.Situacao == processoViewModel.Status)
                    {
                        Debug.WriteLine(" Nenhuma alteração detectada, ignorando atualização.");
                        return;
                    }

                    modeloOriginal.Situacao = processoViewModel.Status; // Atualiza o modelo original aqui

                    if (modeloOriginal is ReclamacaoEnel reclamacaoEnel)
                    {
                        await ReclamacaoControl.UpdateAsync(processoViewModel.Titulo, reclamacaoEnel);
                    }
                    else if (modeloOriginal is ReclamacaoGeral reclamacaoGeral)
                    {
                        await ReclamacaoControl.UpdateAsync(processoViewModel.Titulo, reclamacaoGeral);
                    }
                }
            }
        }
        public ProcessosListaPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            _ = CarregarProcessos();
        }


        private async void PesquisarProcesso(object sender, TextChangedEventArgs e)
        {

        }

        private async Task CarregarProcessos()
        {
            if (_isLoading) return;

            carregando.Visibility = Visibility.Visible;
            carregando.IsActive = true;
            _isLoading = true;

            try
            {
                if (_totalProcessos == 0)
                {
                    _totalProcessos = await ReclamacaoControl.CountAsync();
                }

                var processos = await ReclamacaoControl.GetNRows(_limit, _offset);
                DispatcherQueue.TryEnqueue(() => Processos.Clear());

                if (processos != null && processos.Any())
                {
                    foreach (var processo in processos)
                    {
                        if (processo != null)
                        {
                            var processoViewModel = new ReclamacaoViewModel(processo);
                            processoViewModel.PropertyChanged += Processo_PropertyChanged; // Escutar mudanças

                            DispatcherQueue.TryEnqueue(() => Processos.Add(processoViewModel));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Erro ao carregar processos: {ex.Message}");
            }
            finally
            {
                carregando.IsActive = false;
                carregando.Visibility = Visibility.Collapsed;
                _isLoading = false;
                AtualizarEstadoDosBotoes();
            }
        }

        private void AtualizarEstadoDosBotoes()
        {
            BotaoPaginaAnterior.IsEnabled = _paginaAtual > 1;
            BotaoProximaPagina.IsEnabled = _offset + _limit < _totalProcessos;
        }

        private async void ProximaPagina_Click(object sender, RoutedEventArgs e)
        {
            if (_isLoading || _offset + _limit >= _totalProcessos) return;

            _paginaAtual++;
            _offset = (_paginaAtual - 1) * _limit;
            OnPropertyChanged(nameof(PaginaAtual));
            await CarregarProcessos();
        }

        private async void PaginaAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (_paginaAtual > 1)
            {
                _paginaAtual--;
                _offset = (_paginaAtual - 1) * _limit;
                OnPropertyChanged(nameof(PaginaAtual));
                await CarregarProcessos();
            }
        }

        private void _NovoProcessoBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                FlyoutBase.ShowAttachedFlyout(element);
            }
        }

        private void ProcessoItem_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var stackPanel = sender as StackPanel;
            FlyoutBase.ShowAttachedFlyout(stackPanel);
        }

        private void ProcessoItem_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            // Aqui você pode ocultar o Flyout se necessário (geralmente feito automaticamente pelo sistema)
        }

        private void Processo_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is ReclamacaoViewModel reclamacao)
            {
                var menuFlyout = new MenuFlyout();

                var visualizarItem = new MenuFlyoutItem { Text = "Visualizar Processo" };
                visualizarItem.Click += (s, args) => VisualizarProcesso(reclamacao);

                var excluirItem = new MenuFlyoutItem
                {
                    Text = "Excluir Processo",
                    Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red)
                };
                excluirItem.Click += (s, args) => ExcluirReclamacao(reclamacao);

                menuFlyout.Items.Add(visualizarItem);
                //menuFlyout.Items.Add(editarItem);
                menuFlyout.Items.Add(new MenuFlyoutSeparator());
                menuFlyout.Items.Add(excluirItem);

                menuFlyout.ShowAt(element, e.GetPosition(element));
            }
        }

        private async void ExcluirReclamacao(ReclamacaoViewModel reclamacao)
        {
            //Popup de confirmação
           ContentDialog confirmDialog = new ContentDialog
           {
               Title = "Confirmar Exclusão",
               Content = "Tem certeza de que deseja excluir esta reclamação? Essa ação não pode ser desfeita.",
               PrimaryButtonText = "Excluir",
               CloseButtonText = "Cancelar",
               DefaultButton = ContentDialogButton.Close,
               XamlRoot = this.XamlRoot,
               PrimaryButtonStyle = new Style(typeof(Button))
               {
                   Setters =
           {
                new Setter(Button.BackgroundProperty, new SolidColorBrush(Colors.Red)),
                new Setter(Button.ForegroundProperty, new SolidColorBrush(Colors.White)),
                new Setter(Button.CornerRadiusProperty, new CornerRadius(5))
           }
               }
           };

            ContentDialogResult result = await confirmDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                // Executa a exclusão
                await ReclamacaoControl.DeleteAsync(reclamacao.Titulo);

                // Popup de sucesso
                ContentDialog sucessoDialog = new ContentDialog
                {
                    Title = "Sucesso",
                    Content = "O processo foi excluído com sucesso!",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                await sucessoDialog.ShowAsync();
            }
        }


        private async void PesquisarProcesso_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            // Verifica se a tecla pressionada foi "Enter"
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                var textBox = sender as TextBox;
                string pesquisa = textBox.Text;

                if (string.IsNullOrWhiteSpace(pesquisa))
                {
                    await CarregarProcessos();
                    MensagemFeedback.Visibility = Visibility.Collapsed; // Oculta a mensagem de feedback
                    return;
                }

                try
                {
                    var processo = await ReclamacaoControl.GetAsync(pesquisa);
                    Processos.Clear();

                    if (processo != null)
                    {
                        var processoViewModel = new ReclamacaoViewModel(processo);
                        processoViewModel.PropertyChanged += Processo_PropertyChanged;
                        Processos.Add(processoViewModel);
                        MensagemFeedback.Visibility = Visibility.Collapsed; // Oculta a mensagem de feedback
                    }
                    else
                    {
                        MensagemFeedback.Visibility = Visibility.Visible; // Exibe a mensagem de feedback
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"❌ Erro ao pesquisar processo: {ex.Message}");
                    MensagemFeedback.Text = "Erro ao pesquisar processo.";
                    MensagemFeedback.Visibility = Visibility.Visible; // Exibe a mensagem de erro
                }
            }
        }


        private void VisualizarProcesso(ReclamacaoViewModel reclamacao)
        {
            if (reclamacao.Titulo.StartsWith("E"))
            {
                Frame.Navigate(typeof(RegistrarProcessoEnelPage), reclamacao);
            }
            else if (reclamacao.Titulo.StartsWith("G"))
            {
                Frame.Navigate(typeof(RegistrarProcesso01Page), reclamacao);
            }
        }

        private void OnDragStarting(UIElement sender, DragStartingEventArgs args)
        {
            args.Cancel = true; // Cancela qualquer tentativa de arrasto
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.None;
        }
    }
}
