using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT.Interop;

namespace PROARC
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            string assetsFolder = AppDomain.CurrentDomain.BaseDirectory + @"Assets\";


            this.InitializeComponent();
            mainWindowStoryboard.Begin();

            this.AppWindow.SetIcon(assetsFolder + @"proarc-dark-logo.ico");
        }
    }
}
