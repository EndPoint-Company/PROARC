using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;
using Windows.ApplicationModel.DataTransfer;
using PROARC.src.Models;
using PROARC.src.Control;
using System.ComponentModel;
using Microsoft.UI.Xaml.Media;
using System.Drawing;

namespace PROARC.src.Views
{
    public sealed partial class RegistrarProcesso01Page : Page, INotifyPropertyChanged
    {
        private string numeroProcesso;
        public string NumeroProcesso
        {
            get => numeroProcesso;
            set
            {
                numeroProcesso = value;
                OnPropertyChanged(nameof(NumeroProcesso));
            }
        }

        private string anoProcesso;
        public string AnoProcesso
        {
            get => anoProcesso;
            set
            {
                anoProcesso = value;
                OnPropertyChanged(nameof(AnoProcesso));
            }
        }

        public RegistrarProcesso01Page()
        {
            this.InitializeComponent();
            DataContext = this;

            // Elevar o StackPanel para que a sombra seja exibida
            ProcuradorSection.Translation = new System.Numerics.Vector3(1, 1, 20);
            ReclamanteSection.Translation = new System.Numerics.Vector3(1, 1, 20);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ProcessoNovo_Click(object sender, RoutedEventArgs e)
        {

            // Alterar estilo do botão "Processo Novo" para selecionado
            btnProcessoNovo.Background = new SolidColorBrush(Microsoft.UI.Colors.DarkBlue); // Cor "#003366"
            btnProcessoNovo.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);

            // Alterar estilo do botão "Processo Antigo" para não selecionado
            btnProcessoAntigo.Background = new SolidColorBrush(Microsoft.UI.Colors.White);
            btnProcessoAntigo.Foreground = new SolidColorBrush(Microsoft.UI.Colors.DarkBlue); // Cor "#003366"
            btnProcessoAntigo.BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.CornflowerBlue); // Cor "#005BC1"

            // Permitir edição dos campos
            inputNProcesso.IsReadOnly = false;
            inputAnoProcesso.IsReadOnly = false;

            MainStackPanel.Opacity = 1; // Torna o painel totalmente visível
            NumeroProcesso = ""; // Limpa os campos
            AnoProcesso = "";
        }

        private void ProcessoAntigo_Click(object sender, RoutedEventArgs e)
        {

            // Alterar estilo do botão "Processo Antigo" para selecionado
            btnProcessoAntigo.Background = new SolidColorBrush(Microsoft.UI.Colors.DarkBlue); // Cor "#003366"
            btnProcessoAntigo.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);

            // Alterar estilo do botão "Processo Novo" para não selecionado
            btnProcessoNovo.Background = new SolidColorBrush(Microsoft.UI.Colors.White);
            btnProcessoNovo.Foreground = new SolidColorBrush(Microsoft.UI.Colors.DarkBlue); // Cor "#003366"
            btnProcessoNovo.BorderBrush = new SolidColorBrush(Microsoft.UI.Colors.CornflowerBlue); // Cor "#005BC1"

            // Tornar os campos somente leitura
            inputNProcesso.IsReadOnly = true;
            inputAnoProcesso.IsReadOnly = true;

            MainStackPanel.Opacity = 0.4; // Define opacidade de 40%
            NumeroProcesso = "12345"; // Preenche os campos com valores
            AnoProcesso = "2023";
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
            if (inputNome.Text == "" || inputRgReclamante.Text == "")
            {
                return;
            }

            Dictionary<string, object> dicionarioObjetos = new();
            Reclamante reclamante = new(inputNome.Text,
                                        inputCpfReclamante.Text,
                                        inputRgReclamante.Text);

            ReclamanteControl.AddReclamante(reclamante);

            dicionarioObjetos.Add("Reclamante", reclamante);

            Frame.Navigate(typeof(RegistrarProcesso02Page), dicionarioObjetos);
        }

        private void DragDropArea1_DragOver(object sender, DragEventArgs e)
        {
            // Define o efeito de arrastar como "Copiar"
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
        }

        private void DragDropArea2_DragOver(object sender, DragEventArgs e)
        {
            // Define o efeito de arrastar como "Copiar"
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
        }

        private void FilePickerDialog_Close(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Evento disparado ao clicar no botão "Fechar" do ContentDialog
        }


    }
}