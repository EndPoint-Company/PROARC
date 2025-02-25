using Microsoft.UI.Xaml.Controls;

namespace PROARC.src.Views
{
    public sealed partial class HomeNavigationPage : Page
    {
        public HomeNavigationPage()
        {
            this.InitializeComponent();

            contentFrame.Navigate(typeof(NewHomePage));
            this.NavigationCacheMode = Microsoft.UI.Xaml.Navigation.NavigationCacheMode.Enabled; // Correção aqui
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
                    
                    case "HomePage":
                        contentFrame.Navigate(typeof(NewHomePage));
                        args.SelectedItemContainer.IsSelected = false;
                        break;
                    case "ListPageGeral":
                        contentFrame.Navigate(typeof(ProcessosListaPage));
                        args.SelectedItemContainer.IsSelected = false;
                        break;
                    case "CadastrarPageGeral":
                        contentFrame.Navigate(typeof(RegistrarProcesso01Page), true);
                        args.SelectedItemContainer.IsSelected = false;
                        break;
                    case "LoginPage":
                        Frame.Navigate(typeof(LoginPage));
                        args.SelectedItemContainer.IsSelected = false;
                        break;
                    case "ControleEmpresas":
                        contentFrame.Navigate(typeof(ControleEmpresas));
                        args.SelectedItemContainer.IsSelected = false;
                        break;
                    case "CadastrarPageEnel":
                        contentFrame.Navigate(typeof(RegistrarProcessoEnelPage), true);
                        args.SelectedItemContainer.IsSelected = false;
                        break;
                }
            }
        }
    }
}
