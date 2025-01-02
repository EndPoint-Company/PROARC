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
    public sealed partial class HomeNavigationPage : Page
    {
        public HomeNavigationPage()
        {
            this.InitializeComponent();

            contentFrame.Navigate(typeof(HomePage));
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = args.SelectedItem as NavigationViewItem;

            if (selectedItem != null)
            {
                string tag = selectedItem.Tag as string;

                // Navegar para a página correspondente
                switch (tag)
                {
                    case "SamplePage1":
                        contentFrame.Navigate(typeof(HomePage));
                        break;
                    case "SamplePage2":
                        contentFrame.Navigate(typeof(ProcessosListaPage));
                        break;
                    case "SamplePage3":
                        contentFrame.Navigate(typeof(RegistrarProcesso01Page));
                        break;
                    case "SamplePage4":
                        Frame.Navigate(typeof(LoginPage));
                        break;
                }
            }
        }


    }
}
