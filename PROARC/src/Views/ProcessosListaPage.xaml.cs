using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using PROARC.src.Models.Arquivos;
using PROARC.src.Models.Tipos;

namespace PROARC.src.Views
{
    public sealed partial class ProcessosListaPage : Page
    {
        // Propriedade pública para a lista de processos
        public ObservableCollection<ProcessoAdministrativo> Processos { get; set; }

        public ProcessosListaPage()
        {
            this.InitializeComponent();

            // Inicializando a coleção de processos para testes
            Processos = new ObservableCollection<ProcessoAdministrativo>
            {
                new ProcessoAdministrativo("Caminho/Para/Processo1", "0001/2024", 2023, new Motivo("Juros abusivos"), new("Enel"), new("Jubiscreu"), DateTime.Now, Status.EmTramitacaoAguardandoEnvioDaNotificacao, DateTime.Now, DateTime.Now),
                new ProcessoAdministrativo("Caminho/Para/Processo2", "0002/2024", 2023, new Motivo("Cobrança indevida"), new("Enel"), new("Jubiscreu"), DateTime.Now, Status.ArquivadoAtendido, DateTime.Now, DateTime.Now),
                new ProcessoAdministrativo("Caminho/Para/Processo3", "0003/2024", 2023, new Motivo("Juros abusivos"), new("Enel"), new("Jubiscreu"), DateTime.Now, Status.ArquivadoNaoAtendido, DateTime.Now, DateTime.Now),
            };

            // Definir o contexto de dados para vincular à interface
            this.DataContext = this;
        }

        // Método para navegar de volta
        private void BackPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HomePage));
        }
    }
}

