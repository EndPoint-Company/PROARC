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
        public ObservableCollection<Processo> Processos { get; set; }

        public ProcessosListaPage()
        {
            this.InitializeComponent();

            // Inicializando a coleção de processos para testes
            Processos = new ObservableCollection<Processo>
            {
                new Processo
                {
                    NumeroProcesso = "N0001 / 2014",
                    Reclamante = "Yasmin da Silva",
                    CPFReclamante = "123.456.789-00",
                    Reclamado = "UFC - Quixada",
                    Criado = "01/01/2024",
                    Audiencia = "01/01/2024",
                    Motivo = "Juros Abusibos",
                    Status = "Em andamento"
                },
                new Processo
                {
                    NumeroProcesso = "N0002 / 2014",
                    Reclamante = "Yasmin da Silva",
                    CPFReclamante = "987.654.321-00",
                    Reclamado = "UFC - Quixada",
                    Criado = "01/01/2024",
                    Audiencia = "01/01/2024",
                    Motivo = "Juros Abusibos",
                    Status = "Concluído"
                },

                new Processo
                {
                    NumeroProcesso = "0N0003 / 2014",
                    Reclamante = "Yasmin da Silva",
                    CPFReclamante = "987.654.321-00",
                    Reclamado = "UFC - Quixada",
                    Criado = "02/01/2024",
                    Audiencia = "15/01/2024",
                    Motivo = "Pagamento atrasado",
                    Status = "Concluído"
                },
                // Adicione mais objetos conforme necessário
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

    // Modelo de dados representando um processo
    public class Processo
    {
        public string NumeroProcesso { get; set; }
        public string Reclamante { get; set; }
        public string CPFReclamante { get; set; }
        public string Reclamado { get; set; }
        public string Criado { get; set; }
        public string Audiencia { get; set; }
        public string Motivo { get; set; }
        public string Status { get; set; }
    }
}

