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
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI;
using Windows.UI;                 // Color

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PROARC.src.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegistrarProcesso02Page : Page
    {
        public RegistrarProcesso02Page()
        {
            this.InitializeComponent();
            ReclamadoSection.Translation = new System.Numerics.Vector3(1, 1, 20);
        }

        private void BackPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegistrarProcesso01Page));
        }

        private TextBox CriarTextBox(string placeholder, double width)
        {
            return new TextBox
            {
                PlaceholderText = placeholder,
                Width = width,
                Padding = new Thickness(10),
                Margin = new Thickness(0, 5, 0, 5)
            };
        }

        private StackPanel CriarLinha(params (string placeholder, double width)[] caixas)
        {
            var linha = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 20
            };

            foreach (var (placeholder, width) in caixas)
            {
                linha.Children.Add(CriarTextBox(placeholder, width));
            }

            return linha;
        }

        private StackPanel CriarReclamadoSecao()
        {
            // Barra azul
            var barraAzul = new StackPanel
            {
                Background = new SolidColorBrush(Color.FromArgb(255, 0, 51, 102)),
                Width = 10,
                CornerRadius = new CornerRadius(10, 0, 0, 10)
            };

            // StackPanel da seção branca
            var reclamadoSecao = new StackPanel
            {
                Padding = new Thickness(40),
                Spacing = 10,
                Background = new SolidColorBrush(Colors.White),
                CornerRadius = new CornerRadius(0, 10, 10, 0),
                Width = 1200
            };

            // Aplicar sombra à seção usando ThemeShadow
            reclamadoSecao.Shadow = new ThemeShadow();
            reclamadoSecao.Translation = new System.Numerics.Vector3(0, 0, 32); // Distância da sombra

            // Adicionar título
            reclamadoSecao.Children.Add(new TextBlock
            {
                Text = "Reclamado",
                FontSize = 18,
                FontWeight = FontWeights.Bold
            });

            // Adicionar linhas de TextBoxes
            reclamadoSecao.Children.Add(CriarLinha(("Insira o nome da Instituição", 300), ("Insira o CNPJ", 200)));
            reclamadoSecao.Children.Add(CriarLinha(("Insira a rua", 300), ("Insira o bairro", 280), ("Insira o número", 120)));
            reclamadoSecao.Children.Add(CriarLinha(("Insira a cidade", 200), ("Insira a UF", 100), ("Insira o CEP", 150)));

            // Container principal horizontal
            var container = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(50, 0, 50, 0)
            };

            container.Children.Add(barraAzul);
            container.Children.Add(reclamadoSecao);

            return container;
        }


        private void AdicionarReclamado_Click(object sender, RoutedEventArgs e)
        {
            ReclamadosContainer.Children.Add(CriarReclamadoSecao());
        }

        private void ContinuarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegistrarProcesso03Page));
        }
    }
}
