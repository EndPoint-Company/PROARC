using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROARC.src.Control.Database;
using PROARC.src.Models;
using PROARC.src.Models.Tipos;

namespace PROARC.src.Control
{
    public static class MotivoControl
    {
        // Gerencia os motivos na database, não tem muito segredo.
        public static Motivo? GetMotivo(string nome)
        {
            string sql = $"SELECT nome, descricao FROM motivos WHERE nome = {nome}";

            try
            {
                using var reader = DatabaseOperations.QuerySqlCommand(sql);

                if (reader.Read())
                {
                    string nomes = reader.GetString(0);
                    string descricao = reader.GetString(1);

                    return new Motivo(nomes, descricao);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar motivo {nome}: {ex.Message}");
            }

            return null;
        }
        public static Motivo? GetMotivo(int id)
        {
            string sql = $"SELECT nome, descricao FROM motivos WHERE id = {id}";

            try
            {
                using var reader = DatabaseOperations.QuerySqlCommand(sql);

                if (reader.Read())
                {
                    string nomes = reader.GetString(0);
                    string descricao = reader.GetString(1);

                    return new Motivo(nomes, descricao);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar motivo {id}: {ex.Message}");
            }

            return null;
        }

        public static void AddMotivo(Motivo motivo)
        {

            if (motivo == null)
            {
                throw new Exception("Insira um motivo valido.");
            }

            string sql = $"INSERT INTO motivos (nome, descricao) VALUES ('{motivo.MotivoNome}', '{motivo.Descricao}')";
            try
            {
                using var reader = DatabaseOperations.QuerySqlCommand(sql);
                if (reader.Read())
                {
                    Console.WriteLine("Motivo adicionado com sucesso.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar motivo {ex.Message}");
            }
        }

        public static void RemoverMotivo(string nome)
        {
            Motivo? toBeRemoved = GetMotivo(nome);

            if (toBeRemoved != null)
            {
                throw new Exception("Motivo não encontrado no banco de dados.");
            }

            string sql = $"DELETE FROM motivos WHERE nome = {nome}";
            using var reader = DatabaseOperations.QuerySqlCommand(sql);
            try
            {
                if (reader.Read())
                {
                    Console.WriteLine("Motivo removido com sucesso.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao remover Motivo {ex.Message}");
            }
        }

        public static void AtualizarMotivo(int id, string novoNome, string novaDescricao)
        {
            if (string.IsNullOrWhiteSpace(novoNome) || string.IsNullOrWhiteSpace(novaDescricao))
            {
                throw new Exception("Nome e descrição não podem ser nulos ou vazios.");
            }

            Motivo? toBeUpdated = GetMotivo(id);
            if (toBeUpdated != null)
            {
                throw new Exception("Motivo não encontrado no banco de dados.");
            }

            string sql = $"UPDATE motivos SET nome = {novoNome}, descricao = {novaDescricao} WHERE id = {id}";

            try
            {
                using var reader = DatabaseOperations.QuerySqlCommand(sql);
                if (reader.Read())
                {
                    Console.WriteLine("Motivo atualizado com sucesso.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar motivo {ex.Message}");
            }
        }
    }

}
