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
using Windows.Storage.Pickers;
using Windows.ApplicationModel.DataTransfer;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PROARC.src.Views
{
    public sealed partial class RegistrarProcesso01Page : Page
    {
        public RegistrarProcesso01Page()
        {
            this.InitializeComponent();

            // Elevar o StackPanel para que a sombra seja exibida
            ProcuradorSection.Translation = new System.Numerics.Vector3(1, 1, 20);
            ReclamanteSection.Translation = new System.Numerics.Vector3(1, 1, 20);
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

        private async void PickFileButton1_Click(object sender, RoutedEventArgs e)
        {
            // Abre o diálogo para seleção de arquivo para a primeira seção
            await FilePickerDialog1.ShowAsync();
        }

        private async void PickFileButton2_Click(object sender, RoutedEventArgs e)
        {
            // Abre o diálogo para seleção de arquivo para a segunda seção
            await FilePickerDialog2.ShowAsync();
        }

        private void FilePickerDialog_Close(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Evento disparado ao clicar no botão "Fechar" do ContentDialog
        }

        private async void DragDropArea1_Drop(object sender, DragEventArgs e)
        {
            // Verifica se o conteúdo solto é um arquivo
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Any())
                {
                    // Obtém o arquivo arrastado
                    var storageFile = items[0] as Windows.Storage.StorageFile;
                    if (storageFile != null)
                    {
                        PickAFileOutputTextBlock1.Text = $"Arquivo selecionado: {storageFile.Name}";
                        FilePickerDialog1.Hide(); // Fecha o ContentDialog
                    }
                }
            }
        }

        private async void DragDropArea2_Drop(object sender, DragEventArgs e)
        {
            // Verifica se o conteúdo solto é um arquivo
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Any())
                {
                    // Obtém o arquivo arrastado
                    var storageFile = items[0] as Windows.Storage.StorageFile;
                    if (storageFile != null)
                    {
                        PickAFileOutputTextBlock2.Text = $"Arquivo selecionado: {storageFile.Name}";
                        FilePickerDialog2.Hide(); // Fecha o ContentDialog
                    }
                }
            }
        }


        private async void OpenFilePickerButton1_Click(object sender, RoutedEventArgs e)
        {
            // Cria um FileOpenPicker para a primeira seção
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*"); // permite selecionar qualquer tipo de arquivo

            // Inicializa o picker com a janela atual
            var window = (Application.Current as App)?.MainWindow;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Abre o seletor de arquivos
            var file = await openPicker.PickSingleFileAsync();

            // Verifica se o arquivo foi selecionado
            if (file != null)
            {
                // Exibe o nome do arquivo selecionado na ContentDialog1
                SelectedFileNameTextBlock1.Text = "Arquivo selecionado: " + file.Name;
            }
            else
            {
                // Se o usuário cancelar a operação
                SelectedFileNameTextBlock1.Text = "Nenhum arquivo selecionado.";
            }
        }

        private async void OpenFilePickerButton2_Click(object sender, RoutedEventArgs e)
        {
            // Cria um FileOpenPicker para a segunda seção
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*"); // permite selecionar qualquer tipo de arquivo

            // Inicializa o picker com a janela atual
            var window = (Application.Current as App)?.MainWindow;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Abre o seletor de arquivos
            var file = await openPicker.PickSingleFileAsync();

            // Verifica se o arquivo foi selecionado
            if (file != null)
            {
                // Exibe o nome do arquivo selecionado na ContentDialog2
                SelectedFileNameTextBlock2.Text = "Arquivo selecionado: " + file.Name;
            }
            else
            {
                // Se o usuário cancelar a operação
                SelectedFileNameTextBlock2.Text = "Nenhum arquivo selecionado.";
            }
        }
    }
}
