using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Newtonsoft.Json.Linq;
using PROARC.src.Control;
using PROARC.src.Models;
using PROARC.src.Models.Arquivos;
using PROARC.src.Models.Tipos;

namespace PROARC.src.Views
{
    public sealed partial class ProcessosListaPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<Reclamacao?> Processos { get; set; } = new();
        private bool _isLoading;
        private int _limit = 4;
        private int _offset = 0;
        private int _paginaAtual = 1;

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



        public ProcessosListaPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            _ = CarregarProcessos();
        }

        private int _totalProcessos = 0; // Guarda o total de processos disponíveis

        private async Task CarregarProcessos()
        {
            if (_isLoading) return;

            carregando.Visibility = Visibility.Visible;
            carregando.IsActive = true;
            _isLoading = true;

            try
            {
                Debug.WriteLine($"🔄 Buscando processos... Página {_paginaAtual}");

                // Primeiro, pegamos o total de processos se ainda não tivermos essa informação
                if (_totalProcessos == 0)
                {
                    _totalProcessos = await ReclamacaoControl.CountAsync(); // Método fictício que retorna o total
                }

                var processos = await ReclamacaoControl.GetNRows(_limit, _offset);

                // Limpa os processos para carregar a nova página corretamente
                DispatcherQueue.TryEnqueue(() => Processos.Clear());

                if (processos == null || !processos.Any())
                {
                    Debug.WriteLine($"⚠ Nenhum processo encontrado na página {_paginaAtual}.");
                    return;
                }

                foreach (var processo in processos)
                {
                    if (processo != null)
                    {
                        DispatcherQueue.TryEnqueue(() => Processos.Add(processo));
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
            // Se a página for 1, desativa o botão "Página Anterior"
            BotaoPaginaAnterior.IsEnabled = _paginaAtual > 1;

            // Se não houver mais processos para exibir, desativa o botão "Próxima Página"
            BotaoProximaPagina.IsEnabled = _offset + _limit < _totalProcessos;
        }

        private async void ProximaPagina_Click(object sender, RoutedEventArgs e)
        {
            if (_isLoading || _offset + _limit >= _totalProcessos) return; // Impede ir além da última página

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
            if (sender is FrameworkElement element && element.DataContext is ProcessoAdministrativo processo)
            {
                var menuFlyout = new MenuFlyout();

                var visualizarItem = new MenuFlyoutItem { Text = "Visualizar Processo" };
                var editarItem = new MenuFlyoutItem { Text = "Editar Processo" };
                editarItem.Click += (s, args) => EditarProcesso(processo);

                var excluirItem = new MenuFlyoutItem
                {
                    Text = "Excluir Processo",
                    Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red)
                };

                menuFlyout.Items.Add(visualizarItem);
                menuFlyout.Items.Add(editarItem);
                menuFlyout.Items.Add(new MenuFlyoutSeparator());
                menuFlyout.Items.Add(excluirItem);

                menuFlyout.ShowAt(element, e.GetPosition(element));
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


        private void EditarProcesso(ProcessoAdministrativo processo)
        {
            Frame.Navigate(typeof(RegistrarProcesso01Page));
        }
    }
}