using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PROARC.src.Control;
using PROARC.src.Models.Tipos;

namespace PROARC.src.Views
{
    public sealed partial class EditMotivoDialog : ContentDialog
    {
        private List<Motivo> motivos;

        public EditMotivoDialog()
        {
            this.InitializeComponent();

            if (App.MainWindow?.Content is FrameworkElement rootElement)
            {
                this.XamlRoot = rootElement.XamlRoot;
            }

            this.PrimaryButtonClick += ContentDialog_PrimaryButtonClick;
            this.MotivosComboBox.SelectionChanged += MotivosComboBox_SelectionChanged;

            _ = CarregarMotivosAsync();
        }

        private async Task CarregarMotivosAsync()
        {
            try
            {
                motivos = await MotivoControl.GetAllAsync();
                foreach (var motivo in motivos)
                {
                    MotivosComboBox.Items.Add(motivo.Nome);
                }
            }
            catch (Exception ex)
            {
                await new ContentDialog
                {
                    Title = "Erro",
                    Content = $"Falha ao carregar motivos: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }
        }

        private void MotivosComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MotivosComboBox.SelectedItem != null)
            {
                NomeTextBox.Text = MotivosComboBox.SelectedItem.ToString();
            }
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (MotivosComboBox.SelectedItem == null || string.IsNullOrWhiteSpace(NomeTextBox.Text))
            {
                args.Cancel = true;
                this.Hide(); 
                await ExibirMensagemAsync("Erro", "Selecione um motivo e forneça um novo nome.");
                return;
            }

            try
            {
                string nomeAtual = MotivosComboBox.SelectedItem.ToString();
                string novoNome = NomeTextBox.Text;

                if (nomeAtual == novoNome)
                {
                    args.Cancel = true;
                    this.Hide();
                    await ExibirMensagemAsync("Aviso", "O novo nome é idêntico ao nome atual.");
                    return;
                }

                await MotivoControl.UpdateAsync(nomeAtual, novoNome);

                    this.Hide();
                    await ExibirMensagemAsync("Sucesso", "Motivo atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                args.Cancel = true;
                this.Hide();
                await ExibirMensagemAsync("Erro", $"Falha ao atualizar motivo: {ex.Message}");
            }
        }

        private async Task ExibirMensagemAsync(string titulo, string conteudo)
        {
            var dialog = new ContentDialog
            {
                Title = titulo,
                Content = conteudo,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }

    }
}
