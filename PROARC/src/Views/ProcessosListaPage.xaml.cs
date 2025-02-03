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
        public ObservableCollection<ProcessoAdministrativo> Processos { get; set; } = new ObservableCollection<ProcessoAdministrativo>();

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ProcessosListaPage()
        {
            try
            {
                this.InitializeComponent();
                this.DataContext = this;

                _ = CarregarProcessosPeriodicamente();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro na inicialização da página: {ex.Message}");
            }
        }

        private async Task CarregarProcessosPeriodicamente()
        {
            if (IsLoading) return; // Impede chamadas repetidas enquanto já está carregando

            carregando.Visibility = Visibility.Visible;
            carregando.IsActive = true;
            IsLoading = true;

            try
            {
                // Aguarda um intervalo de tempo antes de fazer a requisição
                while (true)
                {
                    // Obtém os processos do banco
                    var processos = await ProcessoAdministrativoControl.GetAllAsync();

                    if (processos != null && processos.Any())
                    {
                        foreach (var processo in processos)
                        {
                            // Verifica se o processo já está na lista, evitando duplicatas
                            if (!Processos.Any(p => p.Titulo == processo.Titulo))
                            {
                                // Enfileira a atualização na UI para adicionar o processo
                                DispatcherQueue.TryEnqueue(() =>
                                {
                                    Processos.Add(processo); // Adiciona o processo à ObservableCollection
                                });
                            }
                        }

                        Debug.WriteLine($"Carregados {processos.Count} processos.");
                        carregando.IsActive = false;
                        carregando.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        Debug.WriteLine("Nenhum novo processo foi retornado.");
                    }

                    // Espera um intervalo de tempo, antes de verificar novamente
                    await Task.Delay(10000);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao carregar os processos: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
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
            e.Handled = true; // Evita que o evento de arrasto se propague
        }

        private void EditarProcesso(ProcessoAdministrativo processo)
        {
            Frame.Navigate(typeof(RegistrarProcesso01Page));
        }



      

    }
}
