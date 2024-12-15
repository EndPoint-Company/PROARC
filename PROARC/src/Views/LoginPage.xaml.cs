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
using PROARC.src.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PROARC.src.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            this.RequestedTheme = Microsoft.UI.Xaml.ElementTheme.Light;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HomeNavegationPage));
        }

        //private void LoginButton_Click(object sender, RoutedEventArgs e)
        //{
        //    // Suponha que o login foi bem-sucedido, ent�o mostramos o NavigationView
        //    var parentPage = (RegistrarProcesso01Page)this.Parent;
        //    parentPage.nvSample.Visibility = Visibility.Visible; // Restaura o NavigationView
        //    parentPage.contentFrame.Navigate(typeof(HomePage)); // Navega para a HomePage ou a p�gina desejada ap�s o login
        //}


    }
}
