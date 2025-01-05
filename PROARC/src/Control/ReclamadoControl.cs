using PROARC.src.Control.Database;
using PROARC.src.Models;
using System;
using System.Collections.Generic;

namespace PROARC.src.Control
{
    public static class ReclamadoControl
    {
        public static Reclamado? GetReclamado(int id)
        {
            string sql = $"use ProArc; SELECT nome, cpf, cnpj, numero_rua, email, rua, bairro, cidade, uf FROM Reclamados WHERE reclamado_id = {id}";
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
                    return null;
                }

                return new Reclamado(nome, numeroRua, rua, bairro, email, cidade, uf, cnpj, cpf);
            }

            return null;
        }

        public static int? GetReclamadoId(string cpf, string nome)
        {
            string sql = $"USE ProArc; SELECT reclamado_id FROM Reclamados WHERE cpf = '{cpf}' AND nome = '{nome}'";
            List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

            if (reader.Count >= 1)
            {
                int idReclamado = int.Parse(reader[0]);
                return idReclamado;
            }

            return null;
        }

        public static LinkedList<Reclamado>? GetAllReclamados()
        {
            LinkedList<Reclamado> reclamados = new();
            string sql = "use ProArc; SELECT reclamado_id FROM Reclamados";

            List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

            if (reader.Count == 0)
            {
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
                }
            }

            return reclamados;
        }

        public static void AddReclamado(Reclamado reclamado)
        {
            if (reclamado == null)
            {
                throw new Exception("Insira um reclamado valido.");
            }

            string sql = $"use ProArc; INSERT INTO Reclamados(nome, cpf, cnpj, numero_rua, email, rua, bairro, cidade, uf) VALUES('{reclamado.Nome}', '{reclamado.Cpf}', '{reclamado.Cnpj}', {reclamado.NumeroDaRua}, '{reclamado.Email}', '{reclamado.Rua}', '{reclamado.Bairro}', '{reclamado.Cidade}', '{reclamado.Estado}')";

            DatabaseOperations.QuerySqlCommand(sql);
        }

        public static void RemoverReclamado(int id)
        {
            string sql = $"use ProArc; DELETE FROM Reclamados WHERE reclamado_id = '{id}' ";
            DatabaseOperations.QuerySqlCommand(sql);
        }

        public static void AtualizarReclamado(int id, Reclamado reclamado)
        {
            if (reclamado == null)
            {
                throw new Exception("Reclamado inválido.");
            }

            string sql = $"use ProArc; UPDATE Reclamados SET nome = '{reclamado.Nome}', cpf = '{reclamado.Cpf}', cnpj = '{reclamado.Cnpj}', numero_rua = {reclamado.NumeroDaRua}, email = '{reclamado.Email}', rua = '{reclamado.Rua}', bairro = '{reclamado.Bairro}', cidade = '{reclamado.Cidade}', uf = '{reclamado.Estado}' WHERE reclamado_id = {id}";
            DatabaseOperations.QuerySqlCommand(sql);
        }
    }
}
