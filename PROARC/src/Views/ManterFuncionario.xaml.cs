using System;
using System.Windows;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using System.Globalization;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Popups;
using Windows.ApplicationModel.DataTransfer;
using Microsoft.UI.Xaml.Input;
using PROARC.src.Control;
using System.Threading.Tasks;
using PROARC.src.Models;


namespace PROARC.src.Views
{

    public sealed partial class ManterFuncionario : Page
    {
        public ObservableCollection<Usuario> ListaDeUsuarios { get; set; } = new ObservableCollection<Usuario>();

        public ManterFuncionario()
        {
            this.InitializeComponent();
            DataContext = this;
            _ = ManterFuncionarios_Loaded();
            
            Debug.Write("dentro aqui:");
            _ = GetAllTest();
            
        }

        public async Task GetAllTest()
        {
            var usuarios = await UsuarioControl.GetAll();
        }

        private async Task ManterFuncionarios_Loaded()
        {
            var usuarios = await UsuarioControl.GetAll();
            Debug.WriteLine("manter funcionaros loaded foi chamado");

            if (usuarios != null)
            {
                Debug.Write("Entrou no if");
                
                foreach (var usuario in usuarios)
                {
                    if (usuario != null)
                    {
                        ListaDeUsuarios.Add(usuario);
                        Debug.WriteLine(usuario);
                        Debug.WriteLine(ListaDeUsuarios); ;
                    }
                }
            }
            else
            {
                Console.WriteLine("Lista de usuários retornou null.");
            }
            
            Debug.Write("terminou o manterFuncionario_loaded");
        }

        
        
        /*
        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            Usuario item = (Usuario)((Button)sender).DataContext;

            txtEditarNome.Text = item.Nome;

            switch (item.Cargo)
            {
                case "Atendente":
                    radio_Editar_Atendente.IsChecked = true;
                    break;
                case "Consiliadora":
                    radio_Editar_Consiliadora.IsChecked = true;
                    break;
                case "Escrivão":
                    radio_Editar_Escrivao.IsChecked = true;
                    break;
                default:
                    break;
            }

            EditarFuncionarioSection.Visibility = Visibility.Visible;
        }


        // Variável para armazenar o item a ser excluído
        private Usuario itemParaExcluir;

        private void Excluir_Click(object sender, RoutedEventArgs e)
        {
            // Armazena o item que será excluído
            itemParaExcluir = (Usuario)((Button)sender).DataContext;

            // Exibe o popup de confirmação
            popupEdicao.IsOpen = true;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            // Fecha o popup sem fazer nada
            popupEdicao.IsOpen = false;
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            // Remove o item da lista após confirmação
            if (itemParaExcluir != null)
            {
                ListaDeItens.Remove(itemParaExcluir);
                itemParaExcluir = null; // Limpa a variável após a exclusão
            }

            // Fecha o popup
            popupEdicao.IsOpen = false;
        }




        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {

            // 1. Obter o texto da barra de pesquisa
            string searchText = SearchBar.Text;

            // 2. Verificar se o texto não está vazio
            if (!string.IsNullOrEmpty(searchText))
            {
                // 3. Filtrar os resultados da pesquisa
                List<string> resultadosFiltrados = FiltrarResultados(searchText);

                // 4. Atualizar a exibição dos resultados
                ExibirResultados(resultadosFiltrados);
            }
            else
            {
                // 5. Exibir todos os resultados ou uma mensagem informativa
                ExibirTodosResultados();
            }
        }

        // Método auxiliar para filtrar os resultados
        private static List<string> FiltrarResultados(string searchText)
        {
            return new List<string>();

            // Lógica para filtrar os resultados com base em searchText
            // ...
        }

        // Métodos auxiliares para exibir os resultados
        private void ExibirResultados(List<string> resultadosFiltrados)
        {
            // Lógica para exibir os resultados filtrados na interface do usuário
            // ...
        }

        private void ExibirTodosResultados()
        {
            // Lógica para exibir todos os resultados na interface do usuário
            // ...
        }




        private void Identificador_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (textBlock != null)
            {
                // Copia o texto para a área de transferência
                var dataPackage = new DataPackage();
                dataPackage.SetText(textBlock.Text);
                Clipboard.SetContent(dataPackage);
            }
        }

        private void Identificador_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Identificador_Tapped(sender, new TappedRoutedEventArgs());
        }




        private async void CadastrarButton_Click(object sender, RoutedEventArgs e)
        {
            // Coletar os dados do formulário
            string nome = NomeTextBox.Text;
            string cargo = string.Empty;

            // Verificar qual tipo de funcionário foi selecionado
            if (radio_Atendente.IsChecked == true)
            {
                cargo = "Atendente";
            }
            else if (radio_Consiliadora.IsChecked == true)
            {
                cargo = "Consiliadora";
            }
            else if (radio_Escrivao.IsChecked == true)
            {
                cargo = "Escrivão";
            }

            // Verificar se todos os campos estão preenchidos
            if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(cargo))
            {
                // não deu certo aqui await new MessageDialog("Por favor, preencha todos os campos.").ShowAsync();
                return;
            }

            // Criar uma nova instância do funcionário
            Usuario novoFuncionario = new Usuario
            {
                Nome = nome,
                Cargo = cargo,
            };

            // Adicionar o novo funcionário à lista
            ListaDeItens.Add(novoFuncionario);

            // Limpar os campos após o cadastro
            NomeTextBox.Text = string.Empty;
            radio_Atendente.IsChecked = false;
            radio_Consiliadora.IsChecked = false;
            radio_Escrivao.IsChecked = false;

            // Exibir uma mensagem de sucesso
            //não deu certo aqui await new MessageDialog("Funcionário cadastrado com sucesso!").ShowAsync();
        }


        private void btnCriarChaveAcesso_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("btnCriarChaveAcesso_Click: Método iniciado."); // LOG: Início do método
            
                // 1. Obter o Identificador do Funcionário do TextBox
                string identificadorFuncionario = txtIdentificadorFuncionarioChave.Text;

                // 2. Obter a Chave de Acesso do TextBox
                string chaveDigitada = txtChaveAcesso.Text;

                // 3. Encontrar o Funcionário na Lista usando o Identificador
               // Usuario funcionarioParaAtualizar = ListaDeItens.FirstOrDefault(f => f.Identificador == identificadorFuncionario);

                
        }

        private void SalvarAlteracoes_Click(object sender, RoutedEventArgs e)
        {
            // Obter o item que está sendo editado (você pode armazená-lo em uma variável de instância)
            Usuario item = (Usuario)((Button)sender).DataContext; // Ajuste se necessário

            // Atualizar os dados do item
            item.Nome = txtEditarNome.Text;

            // Verificar qual RadioButton está selecionado e atualizar o cargo
            if (radio_Editar_Atendente.IsChecked == true)
            {
                item.Cargo = "Atendente";
            }
            else if (radio_Editar_Consiliadora.IsChecked == true)
            {
                item.Cargo = "Consiliadora";
            }
            else if (radio_Editar_Escrivao.IsChecked == true)
            {
                item.Cargo = "Escrivão";
            }

            // Opcionalmente, você pode ocultar a seção de edição
            EditarFuncionarioSection.Visibility = Visibility.Collapsed;

            // Se necessário, exiba uma mensagem de confirmação
        }
        */

    }
}

