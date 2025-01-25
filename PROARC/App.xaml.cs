using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using PROARC.src.Views;

namespace PROARC
{
    public partial class App : Application
    {
        private static App _instance;
        private Window? _mainWindow;

        public App()
        {
            _instance = this;
            this.InitializeComponent();
        }

        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _mainWindow = new MainWindow();
            Frame rootFrame = new Frame();
            rootFrame.NavigationFailed += OnNavigationFailed;
            rootFrame.Navigate(typeof(LoginPage), args.Arguments);

            _mainWindow.Activate();

            await Task.Delay(1280);

            _mainWindow.Content = rootFrame;
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Falha ao carregar página " + e.SourcePageType.FullName);
        }

        public static Window MainWindow => _instance._mainWindow;
    }
}
