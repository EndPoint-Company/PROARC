﻿using PROARC.src.Control.Database;
using PROARC.src.Models;
using PROARC.src.Models.Tipos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Control
{
    public static class ReclamanteControl
    {

        public static int? GetReclamanteId(string rg)
        {
            string sql = $"use ProArc; SELECT reclamante_id FROM Reclamantes WHERE rg = '{rg}'";

            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

               if (reader.Count >= 1)
                {
                    int idReclamante = int.Parse(reader[0]);

                    return idReclamante;

                }
                else
                {
                    Console.WriteLine($"Reclamante com nome {rg} não encontrado ou dados insuficientes.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar reclamante {rg}: {ex.Message}");
            }

            return null;
        }

        public static Reclamante? GetReclamante(int id)
        {
            string sql = $"use ProArc; SELECT nome, rg, cpf FROM Reclamantes WHERE reclamante_id = {id}";

            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

                if (reader.Count >= 3)
                {
                    string nome = reader[0];
                    string rg = reader[1];
                    string cpf = reader[2];

                    return new Reclamante(nome, cpf, rg);   
                }
                else
                {
                    Console.WriteLine($"Reclamante com ID {id} não encontrado ou dados insuficientes.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar reclamante com ID {id}: {ex.Message}");
            }
            return null;
        }

        public static LinkedList<Reclamante>? GetAllReclamante()
        {
            LinkedList<Reclamante> reclamantes = new LinkedList<Reclamante>();
            string sql = "use ProArc; SELECT reclamante_id FROM Reclamantes";

            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

                if (reader.Count == 0)
                {
                    Console.WriteLine("Nenhum reclamado encontrado.");
                    return null;
                }

                string nome = string.Empty;
                string rg = string.Empty;
                string cpf = string.Empty;

                bool isNome = true;

                foreach (string idStr in reader)
                {
                    if (int.TryParse(idStr, out int id))
                    {
                        Reclamante? reclamante = GetReclamante(id);
                        if (reclamante != null)
                        {
                            reclamantes.AddLast(reclamante);
                        }
                        else
                        {
                            Console.WriteLine($"Erro ao obter os dados do reclamante com ID {id}.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Erro ao converter o ID {idStr} para inteiro.");
                    }
                }
                return reclamantes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar usuários: {ex.Message}");
                return null;
            }
        }

        public static void AddReclamante(Reclamante reclamante)
        {
            if (reclamante == null)
            {
                throw new Exception("Insira um reclamante válido.");
            }

            string sql = $"use ProArc; INSERT INTO Reclamantes (nome, rg, cpf) VALUES ('{reclamante.Nome}', '{reclamante.Rg}', '{reclamante.Cpf}')";

            List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

            //foreach (string str in reader)
            //{
            //    Console.WriteLine("Reclamante adicionado com sucesso");
            //}
        }

        public static void AtualizarReclamante(string rg, string novoNome, string novoRg, string novoCpf)
        {
            string sql = $"use ProArc; UPDATE Reclamantes SET nome = '{novoNome}', rg = '{novoRg}', cpf = '{novoCpf}' WHERE rg  = '{rg}'";
            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);
                foreach (string str in reader)
                {
                    Console.WriteLine("Reclamante atualizado com sucesso");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar reclamante {novoNome}: {ex.Message}");
            }
        }

        public static void RemoverReclamante(string rg)
        {
            string sql = $"use ProArc; DELETE FROM Reclamantes WHERE rg = '{rg}'";
            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);
                foreach (string str in reader)
                {
                    Console.WriteLine("Reclamante removido com sucesso");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao deletar reclamante {rg}: {ex.Message}");
            }
        }
    }
}
