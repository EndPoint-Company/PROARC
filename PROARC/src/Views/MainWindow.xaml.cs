using System;
using System.Runtime.InteropServices;
using Microsoft.UI.Xaml;

namespace PROARC
{
    public sealed partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_MAXIMIZE = 3;

        public MainWindow()
        {
            string assetsFolder = AppDomain.CurrentDomain.BaseDirectory + @"Assets\";

            this.InitializeComponent();
            MaximizeWindow();

            this.AppWindow.SetIcon(assetsFolder + @"proarc-white-logo.ico");

            mainWindowStoryboard.Begin();
        }

        private void MaximizeWindow()
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

            ShowWindow(hwnd, SW_MAXIMIZE);
        }
    }
}
