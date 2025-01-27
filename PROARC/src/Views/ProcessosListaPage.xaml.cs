using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using PROARC.src.Models.Arquivos;
using PROARC.src.Models.Tipos;
using PROARC.src.Control;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using PROARC.src.Control;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using PROARC.src.Models;
using System.Linq;

namespace PROARC.src.Views
{
    public sealed partial class ProcessosListaPage : Page
    {
        public ObservableCollection<ProcessoAdministrativo> Processos { get; set; } = new ObservableCollection<ProcessoAdministrativo>();

        public ProcessosListaPage()
        {
            try
            {

                this.InitializeComponent();
                this.DataContext = this;

                //new ProcessoAdministrativo("Caminho/Para/Processo1", "0001/2024", 2023, new Motivo("Juros abusivos"), new("Enel"), new("Jubiscreu"), DateTime.Now, "Em Tramitação", DateTime.Now, DateTime.Now),
                //new ProcessoAdministrativo("Caminho/Para/Processo2", "0002/2024", 2023, new Motivo("Cobrança indevida"), new("Enel"), new("Jubiscreu"), DateTime.Now, "Em Tramitação", DateTime.Now, DateTime.Now),
                //new ProcessoAdministrativo("Caminho/Para/Processo3", "0003/2024", 2023, new Motivo("Juros abusivos"), new("Enel"), new("Jubiscreu"), DateTime.Now, "Em Tramitação", DateTime.Now, DateTime.Now),
            


                Processos.Add(new ProcessoAdministrativo
                {
                    Titulo = "Processo 1",
                    Reclamante = new Reclamante("Carlos Silva", "123.456.789-00", "12345678"),
                    Reclamado = new Reclamado("Empresa XYZ", 123, "Rua das Flores", "Centro", "empresa@xyz.com", "São Paulo", "SP", "12.345.678/0001-00", null),
                    DataDaAudiencia = DateTime.Now,
                    Motivo = new Motivo("Cobrança indevida", "Cobrança feita sem justificativa válida"),
                    Status = "Em andamento"
                });

                Processos.Add(new ProcessoAdministrativo
                {
                    Titulo = "Processo 2",
                    Reclamante = new Reclamante("Maria Oliveira", "987.654.321-00", "87654321"),
                    Reclamado = new Reclamado("Loja ABC", null, null, "Bairro Verde", null, "Rio de Janeiro", "RJ", null, "987.654.321-00"),
                    DataDaAudiencia = DateTime.Now.AddDays(7),
                    Motivo = new Motivo("Juros abusivos"),
                    Status = "Concluído"
                });

                // Carregar os dados do banco
                _ = CarregarProcessosAsync();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro na inicialização da página: {ex.Message}");
            }
        }

        private async Task CarregarProcessosAsync()
        {
            try
            {
                var processos = await ProcessoAdministrativoControl.GetAll();

                if (processos != null && processos.Any())
                {
                    foreach (var processo in processos)
                    {
                        Processos.Add(processo);
                    }
                    Debug.WriteLine($"Carregados {processos.Count} processos.");
                }
                else
                {
                    Debug.WriteLine("Nenhum processo foi retornado.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao carregar os processos: {ex.Message}");
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
