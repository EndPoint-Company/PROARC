using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.Diagnostics;
using PROARC.src.Models.Arquivos;
using PROARC.src.Models.Tipos;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PROARC.src.Views
{
    /// <summary>
    /// Página que lista os processos.
    /// </summary>
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

