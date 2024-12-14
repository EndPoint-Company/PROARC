using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Numerics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PROARC.src.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegistrarProcesso01Page : Page
    {
        public RegistrarProcesso01Page()
        {
            this.InitializeComponent();

        }

        private void ProcuradorCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Torna a seção "Procurador" visível quando o checkbox está marcado
            ProcuradorSection.Visibility = Visibility.Visible;
        }

        private void ProcuradorCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Oculta a seção "Procurador" quando o checkbox está desmarcado
            ProcuradorSection.Visibility = Visibility.Collapsed;
        }

        private void ContinuarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegistrarProcesso02Page));
        }


    }
}
