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

namespace PROARC.src.Views
{
    public sealed partial class ProcessosListaPage : Page
    {
        public ObservableCollection<ProcessoAdministrativo> Processos { get; set; } = new ObservableCollection<ProcessoAdministrativo>();


        public ProcessosListaPage()
        {
            this.InitializeComponent();

            this.DataContext = this;
            
            // Carregar os dados no início
            _ = CarregarProcessosAsync();
        }

        private async Task CarregarProcessosAsync()
        {
            try
            {
                // Supondo que você obtenha uma lista de strings representando os processos
                List<ProcessoAdministrativo> dadosProcessos = await ProcessoAdministrativoControl.GetAll();

                foreach (var dados in dadosProcessos)
                {
                    var processoString = dados.ToString(); // Aqui converta o objeto para string se necessário
                    var processoFormatado = ParseProcesso(processoString);
                    Processos.Add(processoFormatado);
                    new ProcessoAdministrativo("Caminho/Para/Processo1", "0001/2024", 2023, new Motivo("Juros abusivos"), new("Enel"), new("Jubiscreu"), DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                // Logar ou tratar erro
                Debug.WriteLine($"Erro ao carregar os processos: {ex.Message}");
            }
        }

        private ProcessoAdministrativo ParseProcesso(string dados)
        {
            var processo = new ProcessoAdministrativo();
            var linhas = dados.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var linha in linhas)
            {
                var partes = linha.Split(':', 2);
                if (partes.Length < 2) continue;

                var chave = partes[0].Trim();
                var valor = partes[1].Trim();

                switch (chave)
                {
                    case "Título":
                        processo.Titulo = valor;
                        break;
                    case "Caminho do Processo":
                        processo.CaminhoDoProcesso = valor;
                        break;
                    case "Ano":
                        processo.Ano = short.TryParse(valor, out var ano) ? ano : (short)0;
                        break;
                    case "Status":
                        processo.Status = valor;
                        break;
                    case "Motivo":
                        processo.Motivo = new Motivo (valor);
                        break;
                    case "Reclamado":
                        processo.Reclamado = new Reclamado(nome: valor);
                        break;
                    case "Reclamante":
                        var reclamanteInfo = valor.Split(',');
                        processo.Reclamante = new Reclamante(
                            nome: reclamanteInfo.Length > 0 ? reclamanteInfo[0].Split(':')[1].Trim() : "Não definido",
                            rg: reclamanteInfo.Length > 1 ? reclamanteInfo[1].Split(':')[1].Trim() : null,
                            cpf: reclamanteInfo.Length > 2 ? reclamanteInfo[2].Split(':')[1].Trim() : null
                        );
                        break;
                    case "Data da Audiência":
                        processo.DataDaAudiencia = DateTime.TryParse(valor, out var data) ? data : (DateTime?)null;
                        break;
                }
            }

            return processo;
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
