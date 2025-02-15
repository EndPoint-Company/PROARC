using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using PROARC.src.Views;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;

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
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(_mainWindow);
            var windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            // Remover barra de título
            appWindow.TitleBar.ExtendsContentIntoTitleBar = true;

            await Task.Delay(500); // Pequeno atraso para capturar corretamente o fundo

            // Obter cor do fundo da janela
            var backgroundColor = (_mainWindow.Content as Panel)?.Background as SolidColorBrush;
            bool isDarkBackground = backgroundColor != null &&
                                    (backgroundColor.Color.R < 128 && backgroundColor.Color.G < 128 && backgroundColor.Color.B < 128);

            // Ajustar cor dos ícones de acordo com o fundo
            var iconColor = isDarkBackground ? Colors.White : Colors.Black;
            var hoverColor = isDarkBackground ? Colors.Gray : Colors.LightGray;
            var pressedColor = isDarkBackground ? Colors.Gray : Colors.DarkGray;

            // Ajuste das cores dos ícones e fundo da barra de título
            appWindow.TitleBar.ButtonForegroundColor = iconColor;
            appWindow.TitleBar.ButtonHoverForegroundColor = hoverColor;
            appWindow.TitleBar.ButtonPressedForegroundColor = pressedColor;

            // Fundo dos botões transparente
            appWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            appWindow.TitleBar.ButtonHoverBackgroundColor = Colors.Transparent;
            appWindow.TitleBar.ButtonPressedBackgroundColor = Colors.Transparent;

            Frame rootFrame = new Frame();
            rootFrame.NavigationFailed += OnNavigationFailed;
            rootFrame.Navigate(typeof(NewHomePage), args.Arguments);

            _mainWindow.Content = rootFrame;
            _mainWindow.Activate();
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Falha ao carregar página " + e.SourcePageType.FullName);
        }

        public static Window MainWindow => _instance._mainWindow;
    }
}
