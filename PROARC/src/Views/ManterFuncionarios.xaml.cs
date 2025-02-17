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


namespace PROARC.src.Views
{

    public class FuncionarioMoki
    {
        public string Identificador { get; set; }
        public string Nome { get; set; }
        public string Cargo { get; set; }
        public string DataCriacao { get; set; }

        private string chaveAcesso = null;  // 🔒 Mantida privada

        // Método para definir a chave de forma segura
        public void DefinirChaveAcesso(string novaChave)
        {
            Debug.WriteLine($"Tentando definir chave: {novaChave}");
            this.chaveAcesso = novaChave;
        }

        // Método para verificar a chave sem expô-la
        public String ReturnarChave()
        {   if (chaveAcesso != null)
            {
                return chaveAcesso;
            } else {
                return "Chave está nula";
               }
        }
    }


    public sealed partial class ManterFuncionarios : Page
    {
        public ObservableCollection<FuncionarioMoki> ListaDeItens { get; set; } = new ObservableCollection<FuncionarioMoki>();

        public ManterFuncionarios()
        {
            this.InitializeComponent();
            DataContext = this;
            this.Loaded += ManterFuncionarios_Loaded; // Adiciona o evento Loaded

        }

        private void ManterFuncionarios_Loaded(object sender, RoutedEventArgs e)
        {
            // Adicione os itens aqui, após a página ser carregada
            ListaDeItens.Add(new FuncionarioMoki { Identificador = "nf784f4", Nome="Maria De Jesus Nascimento Dos Santos", Cargo = "Escrivão", DataCriacao="23-12-2024"  });
            ListaDeItens.Add(new FuncionarioMoki { Identificador = "nf77844", Nome = "Maria De Jesus Nascimento Dos Santos", Cargo = "Escrivão", DataCriacao = "23-12-2024" });
            ListaDeItens.Add(new FuncionarioMoki { Identificador = "nf784f4", Nome = "Maria De Jesus Nascimento Dos Santos", Cargo = "Escrivão", DataCriacao = "23-12-2024" });
            ListaDeItens.Add(new FuncionarioMoki { Identificador = "nf78w44", Nome = "Maria De Jesus Nascimento Dos Santos", Cargo = "Escrivão", DataCriacao = "23-12-2024" });
            ListaDeItens.Add(new FuncionarioMoki { Identificador = "nf7q844", Nome = "Maria De Jesus Nascimento Dos Santos", Cargo = "Escrivão", DataCriacao = "23-12-2024" });
            ListaDeItens.Add(new FuncionarioMoki { Identificador = "nf47844", Nome = "Maria De Jesus Nascimento Dos Santos", Cargo = "Escrivão", DataCriacao = "23-12-2024" });
            ListaDeItens.Add(new FuncionarioMoki { Identificador = "nf7q844", Nome = "Maria De Jesus Nascimento Dos Santos", Cargo = "Escrivão", DataCriacao = "23-12-2024" });
            ListaDeItens.Add(new FuncionarioMoki { Identificador = "nf782844", Nome = "Maria De Jesus Nascimento Dos Santos", Cargo = "Escrivão", DataCriacao = "23-12-2024" });
            ListaDeItens.Add(new FuncionarioMoki { Identificador = "nf78344", Nome = "Maria De Jesus Nascimento Dos Santos", Cargo = "Escrivão", DataCriacao = "23-12-2024" });
            ListaDeItens.Add(new FuncionarioMoki { Identificador = "nfa78844", Nome = "Maria De Jesus Nascimento Dos Santos", Cargo = "Escrivão", DataCriacao = "23-12-2024" });
            ListaDeItens.Add(new FuncionarioMoki { Identificador = "nf7asd844", Nome = "Maria De Jesus Nascimento Dos Santos", Cargo = "Escrivão", DataCriacao = "23-12-2024" });
            ListaDeItens.Add(new FuncionarioMoki { Identificador = "naf7844", Nome = "Maria De Jesus Nascimento Dos Santos", Cargo = "Escrivão", DataCriacao = "23-12-2024" });
            ListaDeItens.Add(new FuncionarioMoki { Identificador = "naf7844", Nome = "Maria De Jesus Nascimento Dos Santos", Cargo = "Escrivão", DataCriacao = "23-12-2024" });
            ListaDeItens.Add(new FuncionarioMoki { Identificador = "nf78a44", Nome = "Maria De Jesus Nascimento Dos Santos", Cargo = "Escrivão", DataCriacao = "23-12-2024" });
            ListaDeItens.Add(new FuncionarioMoki { Identificador = "nf7a844", Nome = "Maria De Jesus Nascimento Dos Santos", Cargo = "Escrivão", DataCriacao = "23-12-2024" });
            ListaDeItens.Add(new FuncionarioMoki { Identificador = "nf78a44", Nome = "Maria De Jesus Nascimento Dos Santos", Cargo = "Escrivão", DataCriacao = "23-12-2024" });

        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            FuncionarioMoki item = (FuncionarioMoki)((Button)sender).DataContext;
            popupEdicao.IsOpen = true; // Abre o popup
        }

        private void Excluir_Click(object sender, RoutedEventArgs e)
        {
            FuncionarioMoki item = (FuncionarioMoki)((Button)sender).DataContext;
            ListaDeItens.Remove(item);
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            // Valide os dados (se necessário)
            // Lógica para salvar as alterações (ex: atualizar a lista)
            popupEdicao.IsOpen = false; // Fecha o popup
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            popupEdicao.IsOpen = false; // Fecha o popup
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e){

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

        private void CadastrarFuncionarioButton_Click()
        {
            
            // Lógica para CadastrarFuncionario e enviar para o back
    
        }



        private void btnCriarChaveAcesso_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("btnCriarChaveAcesso_Click: Método iniciado."); // LOG: Início do método

            try
            {
                Console.WriteLine("btnCriarChaveAcesso_Click: 1. Obtendo Identificador do Funcionário..."); // LOG: Iniciando passo 1

                // 1. Obter o Identificador do Funcionário do TextBox
                string identificadorFuncionario = txtIdentificadorFuncionarioChave.Text;
                Console.WriteLine($"btnCriarChaveAcesso_Click: Identificador do Funcionário obtido: '{identificadorFuncionario}'."); // LOG: Passo 1 completo

                Console.WriteLine("btnCriarChaveAcesso_Click: 2. Obtendo Chave de Acesso..."); // LOG: Iniciando passo 2
                // 2. Obter a Chave de Acesso do TextBox
                string chaveDigitada = txtChaveAcesso.Text;
                Console.WriteLine($"btnCriarChaveAcesso_Click: Chave de Acesso obtida: '{chaveDigitada}'."); // LOG: Passo 2 completo

                Console.WriteLine("btnCriarChaveAcesso_Click: 3. Encontrando Funcionário na Lista..."); // LOG: Iniciando passo 3
                // 3. Encontrar o Funcionário na Lista usando o Identificador
                FuncionarioMoki funcionarioParaAtualizar = null;
                foreach (var funcionario in ListaDeItens)
                {
                    if (funcionario.Identificador == identificadorFuncionario)
                    {
                        funcionarioParaAtualizar = funcionario;
                        Console.WriteLine($"btnCriarChaveAcesso_Click: Funcionário encontrado na lista: '{funcionario.Nome}' (ID: {funcionario.Identificador})."); // LOG: Funcionário encontrado
                        break; // Encontrou o funcionário, pode sair do loop
                    }
                }

                
            }
            catch (Exception ex)
            {
                // Bloco catch para capturar qualquer exceção que ocorra dentro do try
                Console.WriteLine("btnCriarChaveAcesso_Click: *** EXCEÇÃO CAPTURADA! ***"); // LOG: Exceção capturada
                Console.WriteLine($"btnCriarChaveAcesso_Click: Mensagem de Erro: {ex.Message}"); // LOG: Mensagem da exceção
                Console.WriteLine($"btnCriarChaveAcesso_Click: Stack Trace: {ex.StackTrace}"); // LOG: Stack Trace da exceção
                }
        }

        
    





    }
}

