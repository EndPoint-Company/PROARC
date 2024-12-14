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
                Margin = new Thickness(0, 10, 0, 10)
            };
        }

        private StackPanel CriarLinha(params TextBox[] textBoxes)
        {
            var linha = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 20
            };

            foreach (var textBox in textBoxes)
            {
                linha.Children.Add(textBox);
            }

            return linha;
        }

        private void AdicionarReclamado_Click(object sender, RoutedEventArgs e)
        {
            // Criar os campos da primeira linha
            var primeiraLinha = CriarLinha(
                CriarTextBox("Insira o nome da Instituição", 300),
                CriarTextBox("Insira o CNPJ", 200)
            );

            // Criar os campos da segunda linha
            var segundaLinha = CriarLinha(
                CriarTextBox("Insira a rua", 300),
                CriarTextBox("Insira o bairro", 280),
                CriarTextBox("Insira o número", 120),
                CriarTextBox("Insira a cidade", 200),
                CriarTextBox("Insira a UF", 100),
                CriarTextBox("Insira o CEP", 150)
            );

            // Criar uma nova seção de reclamado com os StackPanels dentro
            var novaSecao = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 229, 229, 229)),
                CornerRadius = new CornerRadius(10),
                Background = new SolidColorBrush(Colors.White),
                Margin = new Thickness(50, 0, 50, 0),
                Child = new StackPanel
                {
                    Padding = new Thickness(40),
                    Spacing = 10,
                    Children =
            {
                new TextBlock
                {
                    Text = "Reclamado",
                    FontSize = 18,
                    FontWeight = FontWeights.Bold
                },
                primeiraLinha,
                segundaLinha
            }
                }
            };

            // Adicionar ao container de reclamados
            ReclamadosContainer.Children.Add(novaSecao);
        }

        private void ContinuarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegistrarProcesso03Page));
        }
    }
}
