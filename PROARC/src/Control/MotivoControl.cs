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
            List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

            if (reader.Count >= 2)
            {
                string nomeMotivo = reader[0];
                string descricao = reader[1];
                return new Motivo(nomeMotivo, descricao);
            }

            return null;
        }

        public static int? GetMotivoId(string nome)
        {
            string sql = $"use ProArc; SELECT motivo_id FROM Motivos WHERE nome = '{nome}'";
            List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

            if (reader.Count >= 1)
            {
                return int.Parse(reader[0]);
            }

            return null;
        }

        public static Motivo? GetMotivo(int id)
        {
            string sql = $"use ProArc; SELECT nome, descricao FROM Motivos WHERE motivo_id = {id}";
            List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

            if (reader.Count >= 2)
            {
                string nomeMotivo = reader[0];
                string descricao = reader[1];
                return new Motivo(nomeMotivo, descricao);
            }

            return null;
        }

        public static LinkedList<Motivo>? GetAllMotivos()
        {
            LinkedList<Motivo> motivos = new();
            string sql = "USE ProArc; SELECT nome, descricao FROM Motivos";

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
                    motivos.AddLast(new Motivo(nome, descricao));
                    isNome = true;
                }
            }

            return motivos;
        }

        public static LinkedList<string>? GetAllMotivosToString()
        {
            LinkedList<string> motivos = new();
            string sql = "USE ProArc; SELECT nome FROM Motivos";
            List<string> reader = DatabaseOperations.QuerySqlCommand(sql);

            foreach (string linha in reader)
            {
                motivos.AddLast(linha);
            }

            return motivos;
        }

        public static void AddMotivo(Motivo motivo)
        {
            if (motivo == null)
            {
                throw new Exception("Insira um motivo válido.");
            }

            DateTime dataCriacao = DateTime.Now;
            string checkSql = $"use ProArc; SELECT COUNT(*) FROM Motivos WHERE nome = '{motivo.Nome}'";
            List<string> checkReader = DatabaseOperations.QuerySqlCommand(checkSql);

            if (checkReader.Count > 0 && checkReader[0] == "1")
            {
                return;
            }

            string insertSql = $"use ProArc; INSERT INTO Motivos (nome, descricao, data_criacao) VALUES ('{motivo.Nome}', '{motivo.Descricao}', '{dataCriacao}')";
            DatabaseOperations.QuerySqlCommand(insertSql);
        }

        public static void RemoverMotivo(string nome)
        {
            Motivo? toBeRemoved = GetMotivo(nome);

            if (toBeRemoved == null)
            {
                throw new Exception("Motivo não encontrado no banco de dados.");
            }

            string sql = $"use ProArc; DELETE FROM Motivos WHERE nome = '{nome}'";
            DatabaseOperations.QuerySqlCommand(sql);
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
            DatabaseOperations.QuerySqlCommand(sql);
        }
    }
}
