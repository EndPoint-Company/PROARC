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
            Frame.Navigate(typeof(RegistrarProcessoPage), true);
        }

        // Manipulador de clique para o botão "Cadastrar Processo Antigo"
        private void CadastrarProcessoAntigoButton_Click(object sender, RoutedEventArgs e)
        {
            // Passando um parâmetro indicando que se trata de um processo antigo
            Frame.Navigate(typeof(RegistrarProcessoPage), false); // false indica que não é um novo processo
        }

        // Manipulador de clique para o botão "Listar Processos"
        private void ListarProcessosButton_Click(object sender, RoutedEventArgs e)
        {
            // Navegar para uma página que liste os processos cadastrados
            Frame.Navigate(typeof(ProcessosListaPage));
        }

    }
}
