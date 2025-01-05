using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace PROARC.src.Views
{
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        private void CadastrarProcessoButton_Click(object sender, RoutedEventArgs e)
        {
            // Navega para outra página (por exemplo, uma página de cadastro de processos)
            // Exemplo de navegação, caso você tenha uma página chamada 'CadastroProcessoPage':
            Frame.Navigate(typeof(RegistrarProcesso01Page));
        }

        // Manipulador de clique para o botão "Cadastrar Processo Antigo"
        private void CadastrarProcessoAntigoButton_Click(object sender, RoutedEventArgs e)
        {
            // Similar ao anterior, navegar para outra página
            Frame.Navigate(typeof(RegistrarProcesso01Page));
        }

        // Manipulador de clique para o botão "Listar Processos"
        private void ListarProcessosButton_Click(object sender, RoutedEventArgs e)
        {
            // Navegar para uma página que liste os processos cadastrados
            Frame.Navigate(typeof(ProcessosListaPage));
        }

    }
}
