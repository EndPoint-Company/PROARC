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
                new ProcessoAdministrativo("Caminho/Para/Processo1", "0001/2024", 2023, new Motivo("Juros abusivos"), new("Enel"), new("Jubiscreu"), DateTime.Now, Status.EmTramitacaoAguardandoEnvioDaNotificacao, DateTime.Now, DateTime.Now),
                new ProcessoAdministrativo("Caminho/Para/Processo2", "0002/2024", 2023, new Motivo("Cobrança indevida"), new("Enel"), new("Jubiscreu"), DateTime.Now, Status.ArquivadoAtendido, DateTime.Now, DateTime.Now),
                new ProcessoAdministrativo("Caminho/Para/Processo3", "0003/2024", 2023, new Motivo("Juros abusivos"), new("Enel"), new("Jubiscreu"), DateTime.Now, Status.ArquivadoNaoAtendido, DateTime.Now, DateTime.Now),
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
    }
}
