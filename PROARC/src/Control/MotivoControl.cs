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

            string sql = $"use ProArc; SELECT nome, descricao FROM Motivos WHERE nome = '{nome}'";

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

            string sql = $"use ProArc; SELECT nome, descricao FROM Motivos WHERE motivo_id = {id}";

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


        public static LinkedList<Motivo>? GetAllMotivos()
        {
            LinkedList<Motivo> motivos = new LinkedList<Motivo>();
            string sql = "USE ProArc; SELECT nome, descricao FROM Motivos";

            try
            {
                
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

                string nome = string.Empty;
                string descricao = string.Empty;

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
                        descricao = linha;
                      
                        Motivo motivo = new Motivo(nome, descricao);
                      
                        motivos.AddLast(motivo);
                       
                        isNome = true;
                    }
                }
                return motivos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar motivos: {ex.Message}");
                return null;
            }
        }

        public static void AddMotivo(Motivo motivo)
        {
            if (motivo == null)
            {
                throw new Exception("Insira um motivo válido.");
            }


            DateTime dataCriacao = DateTime.Now;     
            string checkSql = $"use ProArc; SELECT COUNT(*) FROM Motivos WHERE nome = '{motivo.MotivoNome}'";

            try
            {
                List<string> checkReader = DatabaseOperations.QuerySqlCommand(checkSql);

               
                if (checkReader.Count > 0 && checkReader[0] == "1") 
                {
                    Console.WriteLine($"Motivo com o nome '{motivo.MotivoNome}' já existe no banco de dados.");
                    return;
                }
               

                string insertSql = $"use ProArc; INSERT INTO Motivos (nome, descricao, data_criacao) VALUES ('{motivo.MotivoNome}', '{motivo.Descricao}', '{dataCriacao}')";
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


            string sql = $"use ProArc; DELETE FROM Motivos WHERE nome = '{nome}'";
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


            string sql = $"use ProArc; UPDATE Motivos SET nome = '{novoNome}', descricao = '{novaDescricao}' WHERE nome = '{nome}'";

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
