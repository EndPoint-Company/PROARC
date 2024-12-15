using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PROARC.src.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        private void CadastrarProcessoButton_Click(object sender, RoutedEventArgs e)
        {
            // Navega para outra p�gina (por exemplo, uma p�gina de cadastro de processos)
            // Exemplo de navega��o, caso voc� tenha uma p�gina chamada 'CadastroProcessoPage':
            Frame.Navigate(typeof(RegistrarProcesso01Page));
        }

        // Manipulador de clique para o bot�o "Cadastrar Processo Antigo"
        private void CadastrarProcessoAntigoButton_Click(object sender, RoutedEventArgs e)
        {
            // Similar ao anterior, navegar para outra p�gina
            Frame.Navigate(typeof(RegistrarProcesso01Page));
        }

        // Manipulador de clique para o bot�o "Listar Processos"
        private void ListarProcessosButton_Click(object sender, RoutedEventArgs e)
        {
            // Navegar para uma p�gina que liste os processos cadastrados
            Frame.Navigate(typeof(ProcessosListaPage));
        }

    }
}
