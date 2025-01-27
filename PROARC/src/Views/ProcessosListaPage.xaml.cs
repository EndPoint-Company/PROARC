using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using PROARC.src.Models.Arquivos;
using PROARC.src.Models.Tipos;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;

namespace PROARC.src.Views
{
    public sealed partial class ProcessosListaPage : Page
    {
        public ObservableCollection<ProcessoAdministrativo> Processos { get; set; }

        public ProcessosListaPage()
        {
            this.InitializeComponent();

            // Inicializando a coleção de processos
            Processos = new ObservableCollection<ProcessoAdministrativo>
            {
                //new ProcessoAdministrativo("Caminho/Para/Processo1", "0001/2024", 2023, new Motivo("Juros abusivos"), new("Enel"), new("Jubiscreu"), DateTime.Now, "Em Tramitação", DateTime.Now, DateTime.Now),
                //new ProcessoAdministrativo("Caminho/Para/Processo2", "0002/2024", 2023, new Motivo("Cobrança indevida"), new("Enel"), new("Jubiscreu"), DateTime.Now, "Em Tramitação", DateTime.Now, DateTime.Now),
                //new ProcessoAdministrativo("Caminho/Para/Processo3", "0003/2024", 2023, new Motivo("Juros abusivos"), new("Enel"), new("Jubiscreu"), DateTime.Now, "Em Tramitação", DateTime.Now, DateTime.Now),
            };

            this.DataContext = this;
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

        private void Processo_RightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is ProcessoAdministrativo processo)
            {
                // Crie um MenuFlyout
                var menuFlyout = new MenuFlyout();

                // Adicione opções ao MenuFlyout
                var visualizarItem = new MenuFlyoutItem { Text = "Visualizar Processo" };
                //visualizarItem.Click += (s, args) => VisualizarProcesso(processo);

                var editarItem = new MenuFlyoutItem { Text = "Editar Processo" };
                editarItem.Click += (s, args) => EditarProcesso(processo);

                var excluirItem = new MenuFlyoutItem
                {
                    Text = "Excluir Processo",
                    Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Red) // Define a cor vermelha
                };
                //excluirItem.Click += (s, args) => ExcluirProcesso(processo);

                menuFlyout.Items.Add(visualizarItem);
                menuFlyout.Items.Add(editarItem);
                menuFlyout.Items.Add(new MenuFlyoutSeparator());
                menuFlyout.Items.Add(excluirItem);

                // Exiba o menu no ponto clicado
                menuFlyout.ShowAt(element, e.GetPosition(element));
            }
        }


        private void EditarProcesso(ProcessoAdministrativo processo)
        {
            Frame.Navigate(typeof(RegistrarProcesso01Page));
        }
    }
}
