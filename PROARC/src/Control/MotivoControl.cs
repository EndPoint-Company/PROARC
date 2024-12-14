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
        public static Motivo? GetMotivo(string nome)
        {
            string sql = $"SELECT nome, descricao FROM motivos WHERE nome = '{nome}'";

            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

                if (reader.Count >= 2)
                {
                    string nomeMotivo = reader[0];
                    string descricao = reader[1];
                   
                    return new Motivo(nomeMotivo, descricao);
                                      
                }
                else
                {
                    Console.WriteLine($"Motivo com nome {nome} não encontrado ou dados insuficientes.");
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
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

                if (reader.Count >= 2)
                {
                    string nomeMotivo = reader[0];
                    string descricao = reader[1];

                    return new Motivo(nomeMotivo, descricao);

                }
                else
                {
                    Console.WriteLine($"Motivo com nome {id} não encontrado ou dados insuficientes.");
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
                throw new Exception("Insira um motivo válido.");
            }

            string dataCriacao = DateTime.Now.ToString("yyyy-MM-dd");

            
            string checkSql = $"SELECT COUNT(*) FROM motivos WHERE nome = '{motivo.MotivoNome}'";

            try
            {
                List<string> checkReader = DatabaseOperations.QuerySqlCommand(checkSql);

               
                if (checkReader.Count > 0 && checkReader[0] == "1") 
                {
                    Console.WriteLine($"Motivo com o nome '{motivo.MotivoNome}' já existe no banco de dados.");
                    return;
                }
               
                string insertSql = $"INSERT INTO motivos (nome, descricao, data_criacao) VALUES ('{motivo.MotivoNome}', '{motivo.Descricao}', '{dataCriacao}')";
                List<string> insertReader = DatabaseOperations.QuerySqlCommand(insertSql);

                Console.WriteLine("Motivo adicionado com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar motivo: {ex.Message}");
            }
        }

        public static void RemoverMotivo(string nome)
        {
            Motivo? toBeRemoved = GetMotivo(nome);

            if (toBeRemoved == null)
            {
                throw new Exception("Motivo não encontrado no banco de dados.");
            }

            string sql = $"DELETE FROM motivos WHERE nome = '{nome}'";
            List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

            try
            {
                foreach (string str in reader)
                {
                    Console.WriteLine("Motivo removido com sucesso");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao remover Motivo {ex.Message}");
            }
        }

        
        public static void AtualizarMotivo(string nome, string? novoNome = null, string? novaDescricao = null)
        {
            if (string.IsNullOrWhiteSpace(novoNome) && string.IsNullOrWhiteSpace(novaDescricao))
            {
                throw new Exception("Nome e descrição não podem ser nulos ou vazios.");
            }

            Motivo? toBeUpdated = GetMotivo(nome);
            if (toBeUpdated == null)
            {
                throw new Exception("Motivo não encontrado no banco de dados.");
            }

            string sql = $"UPDATE motivos SET nome = '{novoNome}', descricao = '{novaDescricao}' WHERE nome = '{nome}'";

            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);
                foreach (string str in reader)
                {
                    Console.WriteLine("Motivo atualizado com sucesso");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar motivo {ex.Message}");
            }
        }
        
    }

}
