using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PROARC.src.Control;
using PROARC.src.Models.Tipos;
using PROARC.src.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace PROARC.src.Views
{
    public sealed partial class ControleEmpresas : Page
    {
        public ControleEmpresas()
        {
            this.InitializeComponent();
            ConfigureShadows();
        }

        // CADASTRAR EMPRESA
        private async void CadastrarEmpresaBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!CamposPreenchidos())
            {
                ShowError("Preencha todos os campos obrigatórios (*) antes de continuar.");
                return;
            }

            string nome = inputNome.Text;
            string? tipoDocumento = (comboBoxTipoDocumento.SelectedItem as ComboBoxItem)?.Content.ToString();
            string cnpjCpf = RemoverCaracteresEspeciais(inputCnpjCpf.Text);
            string? email = string.IsNullOrWhiteSpace(inputEmail.Text) ? null : inputEmail.Text;
            string? telefone = string.IsNullOrWhiteSpace(inputTelefone.Text) ? null : inputTelefone.Text;
            string logradouro = inputRua.Text;
            short? numero = short.TryParse(inputNumero.Text, out short num) ? num : (short?)null;
            string bairro = inputBairro.Text;
            string cidade = inputCidade.Text;
            string uf = inputUf.Text;
            string cep = RemoverCaracteresEspeciais(inputCep.Text);

            var reclamado = new Reclamado(
                nome: nome,
                cpf: tipoDocumento == "CPF" ? cnpjCpf : null,
                cnpj: tipoDocumento == "CNPJ" ? cnpjCpf : null,
                numero: numero,
                logradouro: logradouro,
                bairro: bairro,
                cidade: cidade,
                uf: uf,
                cep: cep,
                telefone : telefone,
                email: email
            );

            bool success = await ReclamadoControl.InsertAsync(reclamado);

            if( success )
            {
                var successDialog = new ContentDialog
                {
                    Title = "Sucesso",
                    Content = "Reclamado cadastrado com sucesso!",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };

                await successDialog.ShowAsync();
                this.Frame.Navigate(typeof(ControleEmpresas));
            }
            else
            {
                ShowError("Falha ao cadastrar reclamado. Tente novamente.");
            }
        }

        private bool CamposPreenchidos()
            => new[] { inputNome, inputNumero, inputRua, inputBairro, inputCidade, inputUf, inputCep }
                .All(campo => !string.IsNullOrWhiteSpace(campo.Text));


        // POP-UP DE ERRO
        private async void ShowError(string mensagemErro)
        {
            var dialog = new ContentDialog
            {
                Title = "Erro de Validação",
                Content = mensagemErro,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await dialog.ShowAsync();
        }


        // FORMATAÇÕES CPF, CNPJ E CEP
        private void OnCepTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox textBox) return;
            string rawText = Regex.Replace(textBox.Text, @"\D", "");
            textBox.Text = FormatDocument(rawText, "CEP");
            textBox.SelectionStart = textBox.Text.Length;
        }

        private string RemoverCaracteresEspeciais(string documento)
        {
            return Regex.Replace(documento, @"\D", "");
        }

        private void OnTipoDocumentoChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxTipoDocumento.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedText = selectedItem.Content.ToString();

                if (inputCnpjCpf != null)
                {
                    inputCnpjCpf.IsEnabled = true;

                    if (selectedText == "CPF")
                    {
                        inputCnpjCpf.MaxLength = 14;
                        inputCnpjCpf.PlaceholderText = "Insira o CPF";
                    }
                    else if (selectedText == "CNPJ")
                    {
                        inputCnpjCpf.MaxLength = 18;
                        inputCnpjCpf.PlaceholderText = "Insira o CNPJ";
                    }

                    inputCnpjCpf.Text = string.Empty;
                }
            }
        }

        private void OnCpfCnpjTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox textBox) return;

            string selectedType = ((ComboBoxItem)comboBoxTipoDocumento.SelectedItem).Content.ToString();
            string text = Regex.Replace(textBox.Text, @"\D", "");


            int maxLength = selectedType == "CPF" ? 11 : 14;
            if (text.Length > maxLength) text = text.Substring(0, maxLength);

            textBox.Text = FormatDocument(text, selectedType);
            textBox.SelectionStart = textBox.Text.Length;
        }

        private string FormatDocument(string text, string type)
        {
            if (type == "CPF")
                return FormatWithMask(text, new[] { 3, 7, 11 }, new[] { '.', '.', '-' });
            else if (type == "CNPJ")
                return FormatWithMask(text, new[] { 2, 6, 10, 15 }, new[] { '.', '.', '/', '-' });
            else if (type == "CEP")
                return FormatWithMask(text, new[] { 5 }, new[] { '-' });
            return text;
        }

        private string FormatWithMask(string text, int[] positions, char[] separators)
        {
            if (text.Length == 0) return text;

            string formattedText = text;

            for (int i = 0; i < positions.Length; i++)
            {
                if (formattedText.Length > positions[i])
                    formattedText = formattedText.Insert(positions[i], separators[i].ToString());
            }

            return formattedText;
        }

        private void OnCpfTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is not TextBox textBox) return;
            string rawText = Regex.Replace(textBox.Text, @"\D", "");
            textBox.Text = FormatWithMask(rawText, new[] { 3, 7, 11 }, new[] { '.', '.', '-' });
            textBox.SelectionStart = textBox.Text.Length;
        }

        // SOMBRAS
        private void ConfigureShadows()
        {
            CadastrarEmpresaSection.Translation = new Vector3(1, 1, 20);
        }
    }
}
