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

            string sql = $"SELECT nome, nivel_permissao FROM usuarios WHERE id = {id}";

            try
            {
                using var reader = DatabaseOperations.QuerySqlCommand(sql);

                if (reader.Read())
                {
                    string nome = reader.GetString(0);
                    int nivelDePermissao = reader.GetInt32(1);

                    return new Usuario(nome, nivelDePermissao);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar usuário com ID {id}: {ex.Message}");
            }

            return null;
        }


        /*public static Usuario? GetUsuario(SecureString acessKey)
        {
            // Pega usuario da database pela chave de acesso unica

            return null;
        }
        */

        public static LinkedList<Usuario>? GetAllUsuario()
        {
            LinkedList<Usuario> usuarios = new LinkedList<Usuario>();
            string sql = "SELECT id, nome, nivel_permissao FROM usuarios";

            try
            {
                using var reader = DatabaseOperations.QuerySqlCommand(sql);

                while (reader.Read())
                {
                    string nome = reader.GetString(0);
                    int nivelDePermissao = reader.GetInt32(1);

                    Usuario user = new Usuario(nome, nivelDePermissao);
                    usuarios.AddLast(user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar usuários {ex.Message}");
            }

            return null;
        }

        public static void RemoveUsuario(int id)
        {
            Usuario? toBeRemoved = GetUsuario(id);

            if (toBeRemoved != null)
            {
                throw new Exception("Usuario não encontrado no banco de dados.");
            }

            string sql = $"DELETE FROM usuarios WHERE id = {id}";
            using var reader = DatabaseOperations.QuerySqlCommand(sql);
            try
            {
                if (reader.Read())
                {
                    Console.WriteLine("Usuário removido com sucesso.");
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

            string sql = $"INSERT INTO usuarios (nome, nivel_permissao) VALUES ('{usuario.Nome}', {usuario.NivelDePermissao})";
            try
            {
                using var reader = DatabaseOperations.QuerySqlCommand(sql);
                if (reader.Read())
                {
                    Console.WriteLine("Usuário adicionado com sucesso.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar usuário {ex.Message}");
            }
        }

        public static void AtualizarUsuario(int id, string nome)
        {
            Usuario? toBeUpdated = GetUsuario(id);
            if (toBeUpdated != null)
            {
                throw new Exception("Usuario não encontrado no banco de dados.");
            }

            string sql = $"UPDATE usuarios SET nome = '{nome}' WHERE id = {id}";
            try
            {
                using var reader = DatabaseOperations.QuerySqlCommand(sql);
                if (reader.Read())
                {
                    Console.WriteLine("Usuário atualizado com sucesso.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar usuário {ex.Message}");
            }
        }

        public static void AtualizarUsuario(int id, int nivelDePermissao)
        {
            Usuario? toBeUpdated = GetUsuario(id);
            if (toBeUpdated != null)
            {
                throw new Exception("Usuario não encontrado no banco de dados.");
            }

            string sql = $"UPDATE usuarios SET nivel_permissao = '{nivelDePermissao}' WHERE id = {id}";
            try
            {
                using var reader = DatabaseOperations.QuerySqlCommand(sql);
                if (reader.Read())
                {
                    Console.WriteLine("Usuário atualizado com sucesso.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar usuário {ex.Message}");
            }
        }
        public static void AtualizarUsuario(int id, string nome, int nivelDePermissao)
        {
            Usuario? toBeUpdated = GetUsuario(id);
            if (toBeUpdated != null)
            {
                throw new Exception("Usuario não encontrado no banco de dados.");
            }

            string sql = $"UPDATE usuarios Set nome = '{nome}', nivel_permissao = '{nivelDePermissao}' WHERE id = {id}";
            try
            {
                using var reader = DatabaseOperations.QuerySqlCommand(sql);
                if (reader.Read())
                {
                    Console.WriteLine("Usuário atualizado com sucesso.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar usuário {ex.Message}");
            }
        }
    }
}
