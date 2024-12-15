using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using PROARC.src.Control.Database;
using PROARC.src.Models;

namespace PROARC.src.Control
{
    public static class UsuarioControl
    {

        public static Usuario? GetUsuario(int id)
        {
            string sql = $"use ProArc; SELECT nome, nivel_permissao FROM Usuarios WHERE usuario_id = {id}";
          
            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

                if (reader.Count >= 2)
                {
                    string nome = reader[0];
                  
                    if (short.TryParse(reader[1], out short nivelPermissao))
                      
                    {
                        return new Usuario(nome, nivelPermissao);
                    }
                    else
                    {
                        Console.WriteLine($"Erro ao converter nível de permissão para inteiro: {reader[1]}");
                    }
                }
                else
                {
                    Console.WriteLine($"Usuário com ID {id} não encontrado ou dados insuficientes.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar usuário com ID {id}: {ex.Message}");
            }
            return null;
        }



        public static LinkedList<Usuario>? GetAllUsuario()
        {
            LinkedList<Usuario> usuarios = new LinkedList<Usuario>();
            string sql = "use ProArc; SELECT nome, nivel_permissao FROM Usuarios";

            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

                string nome = string.Empty;

                short nivelPermissao = 0;

                bool isNome = true;

                foreach (string linha in reader)
                {
                    if (isNome)
                    {
                        nome = linha;
                        isNome = false;
                    }
                    else
                    {

                        if (short.TryParse(linha, out nivelPermissao))
                        {

                            Usuario usuario = new Usuario(nome, nivelPermissao);
                            usuarios.AddLast(usuario);
                            isNome = true;
                        }
                        else
                        {
                            Console.WriteLine($"Erro ao converter nível de permissão para inteiro: {linha}");
                        }
                    }
                }
                return usuarios;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar usuários: {ex.Message}");
                return null;
            }
        }

        public static void RemoveUsuario(int id)
        {
            Usuario? toBeRemoved = GetUsuario(id);

            if (toBeRemoved == null)
            {
                throw new Exception("Usuario não encontrado no banco de dados.");
            }


            string sql = $"use ProArc; DELETE FROM usuarios WHERE usuario_id = {id}";

            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

                foreach (string str in reader)
                {
                    Console.WriteLine("Usuario removido com sucesso");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao remover usuário {ex.Message}");
            }
        }
        public static void AddUsuario(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new Exception("Insira um usuario valido.");
            }


            Random random = new Random();
            int chaveAcesso = random.Next(100000, 999999);

            string sql = $"use ProArc; INSERT INTO Usuarios (nome, nivel_permissao, chave_acesso) VALUES ('{usuario.Nome}', {usuario.NivelDePermissao}, '{chaveAcesso}')";
            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);
                foreach (string str in reader)
                {
                    Console.WriteLine("Usuario adicionado com sucesso");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar usuário {ex.Message}");
            }
        }

        public static void AtualizarUsuario(int id, string novoNome)
        {
            Usuario? toBeRemoved = GetUsuario(id);

            if (toBeRemoved == null)
            {
                throw new Exception("Usuario não encontrado no banco de dados.");
            }


            string sql = $"use ProArc; UPDATE Usuarios SET nome = '{novoNome}' WHERE usuario_id = {id}";
            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);
                foreach (string str in reader)
                {
                    Console.WriteLine("Dados do usuario atualizado com sucesso");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar usuário {ex.Message}");
            }
        }


        public static void AtualizarUsuario(int id, int novoNivelDePermissao)
        {
            Usuario? toBeUpdated = GetUsuario(id);
            if (toBeUpdated == null)
            {
                throw new Exception("Usuario não encontrado no banco de dados.");
            }


            string sql = $"use ProArc; UPDATE Usuarios SET nivel_permissao = '{novoNivelDePermissao}' WHERE usuario_id = {id}";

            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);
                foreach (string str in reader)
                {
                    Console.WriteLine("Dados do usuario atualizado com sucesso");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar usuário {ex.Message}");
            }
        }


        public static void AtualizarUsuario(int id, string novoNome, int novoNivelDePermissao)
        {
            Usuario? toBeUpdated = GetUsuario(id);
            if (toBeUpdated == null)
            {
                throw new Exception("Usuario não encontrado no banco de dados.");
            }


            string sql = $"use ProArc; UPDATE Usuarios Set nome = '{novoNome}', nivel_permissao = '{novoNivelDePermissao}' WHERE usuario_id = {id}";

            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);
                foreach (string str in reader)
                {
                    Console.WriteLine("Dados do usuario atualizado com sucesso");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar usuário {ex.Message}");
            }

            /*
                    public static Usuario? GetUsuario(SecureString acessKey)
                    {
                        // Pega usuario da database pela chave de acesso unica

                        return null;
                    }

                    }*/
        }
    }
}