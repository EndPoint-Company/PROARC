using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace PROARC.src.Views
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            this.RequestedTheme = Microsoft.UI.Xaml.ElementTheme.Light;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HomeNavigationPage));
        }

        //private void LoginButton_Click(object sender, RoutedEventArgs e)
        //{
        //    // Suponha que o login foi bem-sucedido, então mostramos o NavigationView
        //    var parentPage = (RegistrarProcesso01Page)this.Parent;
        //    parentPage.nvSample.Visibility = Visibility.Visible; // Restaura o NavigationView
        //    parentPage.contentFrame.Navigate(typeof(HomePage)); // Navega para a HomePage ou a página desejada após o login
        //}
    }
}
