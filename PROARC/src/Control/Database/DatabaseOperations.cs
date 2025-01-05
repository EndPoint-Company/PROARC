﻿using System.Collections.Generic;
using System.Security;
using Microsoft.Data.SqlClient;

namespace PROARC.src.Control.Database
{
    public static class DatabaseOperations
    {
        private static string connectionString = new SqlConnectionStringBuilder
        {
            Encrypt = DatabaseUtil.ReadJson<SQLBuilder>(@"Assets/credentials.json")?.encrypt ?? false,
            DataSource = DatabaseUtil.ReadJson<SQLBuilder>(@"Assets/credentials.json")?.dataSource ?? "undefined",
            UserID = DatabaseUtil.ReadJson<SQLBuilder>(@"Assets/credentials.json")?.user ?? "undefined",
            Password = DatabaseUtil.ReadJson<SQLBuilder>(@"Assets/credentials.json")?.password ?? "undefined",
        }.ConnectionString;

        public static void ValidateUserLogin(SecureString acessKey) // TODO
        {
            using var cn = new SqlConnection(connectionString);

            cn.Open();

            // REFAZER (olhar histórico para referência)

            cn.Close();
        }
        public static void QuerySqlCommandNoReturn(string sql)
        {
            using var cn = new SqlConnection(connectionString);

            cn.Open();

            using var command = new SqlCommand(sql, cn);
            using var reader = command.ExecuteReader();

            cn.Close();
        }

        public static List<string> QuerySqlCommand(string sql)

        {
            var results = new List<string>();

            using (var cn = new SqlConnection(connectionString))
            {
                cn.Open();

                using (var command = new SqlCommand(sql, cn))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            results.Add(reader.GetValue(i).ToString());
                        }
                    }
                }
            }

            return results;
        }

        private static bool CreateProgramDatabase()
        {
            using var cn = new SqlConnection(connectionString);

            cn.Open();

            string sql = "CREATE DATABASE ProArc";
            using var command = new SqlCommand(sql, cn);

            try
            {
                using var reader = command.ExecuteReader();
            }
            catch (SqlException)
            {
                cn.Close();

                return false;
            }

            cn.Close();

            return true;
        }

        public static bool CreateAllProgramTables()
        {
            CreateProgramDatabase();

            TableFactory.CreateUsuarioTable();
            TableFactory.CreateReclamadoTable();
            TableFactory.CreateReclamanteTable();
            TableFactory.CreateMotivoTable();
            TableFactory.CreateProcessoAdministrativoTable();
            TableFactory.CreateDiretorioTable();
            TableFactory.CreateArquivoTable();
            TableFactory.CreateDefaultPathTable();

            return true;
        }  
    }
}
