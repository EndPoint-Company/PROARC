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
        public App()
        {
            this.InitializeComponent();
        }

        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            Frame rootFrame = new Frame();
            rootFrame.NavigationFailed += OnNavigationFailed;
            rootFrame.Navigate(typeof(LoginPage), args.Arguments);

            m_window.Activate();

            await Task.Delay(1280);

            m_window.Content = rootFrame;
        }
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Falha ao carregar página " + e.SourcePageType.FullName);
        }

        private Window? m_window;
        public Window MainWindow => m_window;
    }
}
