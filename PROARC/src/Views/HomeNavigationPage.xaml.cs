using Microsoft.UI.Xaml.Controls;

namespace PROARC.src.Views
{
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
                        args.SelectedItemContainer.IsSelected = false;
                        break;
                    case "SamplePage2":
                        contentFrame.Navigate(typeof(ProcessosListaPage));
                        args.SelectedItemContainer.IsSelected = false;
                        break;
                    case "SamplePage3":
                        contentFrame.Navigate(typeof(RegistrarProcesso01Page));
                        args.SelectedItemContainer.IsSelected = false;
                        break;
                    case "SamplePage4":
                        Frame.Navigate(typeof(LoginPage));
                        args.SelectedItemContainer.IsSelected = false;
                        break;
                }
            }
        }
    }
}
