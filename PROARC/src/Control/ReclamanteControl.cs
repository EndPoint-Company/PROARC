using PROARC.src.Control.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Control
{
    public static class ReclamanteControl
    {
        /*
        public static void AddReclamante(string nome, string rg, string cpf)
        {
            string sql = $"use ProArc; INSERT INTO Reclamantes (nome, rg, cpf) VALUES ('{nome}', '{rg}', '{cpf}')";

            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

                foreach (string str in reader)
                {
                    Console.WriteLine("Reclamante adicionado com sucesso");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cadastrar reclamante {nome}: {ex.Message}");
            }
        }

        public static void AtualizarReclamante(string nome, string rg, string cpf)
        {
            string sql = $"use ProArc; UPDATE Reclamantes SET email = '{email}', telefone = '{telefone}' WHERE cpf = '{cpf}'";
            try
            {
                Database.DatabaseOperations.NonQuerySqlCommand(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar reclamante {nome}: {ex.Message}");
            }
        }
        public static void DeletarReclamante(string cpf)
        {
            string sql = $"use ProArc; DELETE FROM Reclamantes WHERE cpf = '{cpf}'";
            try
            {
                Database.DatabaseOperations.NonQuerySqlCommand(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao deletar reclamante {cpf}: {ex.Message}");
            }
        }*/
    }
}
