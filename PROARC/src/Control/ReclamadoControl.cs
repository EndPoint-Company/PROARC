using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.WindowsAppSDK.Runtime.Packages;
using PROARC.src.Control.Database;
using PROARC.src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Control
{
    public static class ReclamadoControl
    {

        public static Reclamado? GetReclamado(int id)
        {
            string sql = $"use ProArc; SELECT nome, cpf, cnpj, numero_rua, email, rua, bairro, cidade, uf FROM Reclamados WHERE reclamado_id = {id}";
            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);


                if (reader.Count >= 9)
                {
                    string nome = reader[0];
                    string cpf = reader[1];
                    string cnpj = reader[2];
                    string email = reader[4];
                    string rua = reader[5];
                    string bairro = reader[6];
                    string cidade = reader[7];
                    string uf = reader[8];


                    if (!short.TryParse(reader[3], out short numeroRua))
                    {
                        Console.WriteLine($"Erro ao converter número da rua: {reader[3]}");
                        return null;
                    }

                    return new Reclamado(nome, numeroRua, rua, bairro, email, cidade, uf, cnpj, cpf);
                }
                else
                {
                    Console.WriteLine($"Reclamado com ID {id} não encontrado ou dados insuficientes.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar reclamado com ID {id}: {ex.Message}");
            }
            return null;
        }

        public static int? GetReclamadoId(string cpf, string nome)
        {
            string sql = $"USE ProArc; SELECT reclamado_id FROM Reclamados WHERE cpf = '{cpf}' AND nome = '{nome}'";

            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

                if (reader.Count >= 1)
                {
                    int idReclamado = int.Parse(reader[0]);

                    return idReclamado;

                }
                else
                {
                    Console.WriteLine($"Reclamado com nome {cpf} não encontrado ou dados insuficientes.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar reclamado {cpf}: {ex.Message}");
            }

            return null;
        }

        public static LinkedList<Reclamado>? GetAllReclamados()
        {
            LinkedList<Reclamado> reclamados = new();
            string sql = "use ProArc; SELECT reclamado_id FROM Reclamados";

            try
            {

                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

                if (reader.Count == 0)
                {
                    Console.WriteLine("Nenhum reclamado encontrado.");
                    return null;
                }


                foreach (string idStr in reader)
                {
                    if (int.TryParse(idStr, out int id))
                    {
                        Reclamado? reclamado = GetReclamado(id);
                        if (reclamado != null)
                        {
                            reclamados.AddLast(reclamado);
                        }
                        else
                        {
                            Console.WriteLine($"Erro ao obter os dados do reclamado com ID {id}.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Erro ao converter o ID {idStr} para inteiro.");
                    }
                }

                return reclamados;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar reclamados: {ex.Message}");
                return null;
            }
        }

        public static void AddReclamado(Reclamado reclamado)
        {
            if (reclamado == null)
            {
                throw new Exception("Insira um reclamado valido.");
            }

            string sql = $"use ProArc; INSERT INTO Reclamados(nome, cpf, cnpj, numero_rua, email, rua, bairro, cidade, uf) VALUES('{reclamado.Nome}', '{reclamado.Cpf}', '{reclamado.Cnpj}', {reclamado.NumeroDaRua}, '{reclamado.Email}', '{reclamado.Rua}', '{reclamado.Bairro}', '{reclamado.Cidade}', '{reclamado.Estado}')";

            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);
                foreach (string str in reader)
                {
                    Console.WriteLine("Reclamado adicionado com sucesso");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar usuário {ex.Message}");
            }
        }

        public static void RemoverReclamado(int id)
        {
            string sql = $"use ProArc; DELETE FROM Reclamados WHERE reclamado_id = '{id}' ";

            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

                foreach (string str in reader)
                {
                    Console.WriteLine("Reclamado removido com sucesso");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao remover usuário {ex.Message}");
            }
        }

        public static void AtualizarReclamado(int id, Reclamado reclamado)
        {
            if (reclamado == null)
            {
                throw new Exception("Reclamado inválido.");
            }

            string sql = $"use ProArc; UPDATE Reclamados SET nome = '{reclamado.Nome}', cpf = '{reclamado.Cpf}', cnpj = '{reclamado.Cnpj}', numero_rua = {reclamado.NumeroDaRua}, email = '{reclamado.Email}', rua = '{reclamado.Rua}', bairro = '{reclamado.Bairro}', cidade = '{reclamado.Cidade}', uf = '{reclamado.Estado}' WHERE reclamado_id = {id}";
            try
            {
                List<string> reader = DatabaseOperations.QuerySqlCommand(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar o reclamado: {ex.Message}");
            }
        }
    }
}
