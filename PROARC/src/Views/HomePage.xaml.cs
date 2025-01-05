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
