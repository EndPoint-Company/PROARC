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

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs e)
        {
            var selectedItem = e.SelectedItem as NavigationViewItem;
            if (selectedItem != null)
            {
                string tag = selectedItem.Tag as string;

                // Lógica para a navegação do menu
                if (tag == "SamplePage1")
                {
                    Frame.Navigate(typeof(HomePage));
                }
                else if (tag == "SamplePage2")
                {
                    Frame.Navigate(typeof(ProcessosListaPage));
                }
                // Lógica para o login (tanto no menu quanto no rodapé)
                else if (tag == "SamplePage3")
                {
                    // Escondendo o NavigationView e exibindo o LoginPage
                    nvSample.Visibility = Visibility.Collapsed; // Oculta o NavigationView
                    Frame.Navigate(typeof(LoginPage)); // Navega para a página de Login
                }
            }
        }

    }
}
