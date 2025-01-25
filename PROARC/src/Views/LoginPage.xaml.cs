using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

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

        private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                LoginButton_Click(sender, e);
            }
        }
    }
}
