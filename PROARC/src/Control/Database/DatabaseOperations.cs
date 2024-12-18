﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Windows.Storage.Streams;

namespace PROARC.src.Control.Database
{
    public static class DatabaseOperations
    {
        [UnconditionalSuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "<Pending>")]
        private static string connectionString = new SqlConnectionStringBuilder
        {
            // Otimizar isso aqui depois
            Encrypt = DatabaseUtil.ReadJson<SQLBuilder>(@"Assets/credentials.json").encrypt,
            DataSource = DatabaseUtil.ReadJson<SQLBuilder>(@"Assets/credentials.json").dataSource ?? "undefined",
            UserID = DatabaseUtil.ReadJson<SQLBuilder>(@"Assets/credentials.json").user ?? "undefined",
            Password = DatabaseUtil.ReadJson<SQLBuilder>(@"Assets/credentials.json").password ?? "undefined",
            InitialCatalog = DatabaseUtil.ReadJson<SQLBuilder>(@"Assets/credentials.json").initialCatalog ?? "undefined",
        }.ConnectionString;

        public static void ValidateUserLogin(SecureString acessKey) // TODO
        {
            using var cn = new SqlConnection(connectionString);

            cn.Open();

            // REFAZER (olhar histórico para referência)

            cn.Close();
        }

        public static SqlDataReader QuerySqlCommand(string sql)
        {
            using var cn = new SqlConnection(connectionString);

            cn.Open();

            using var command = new SqlCommand(sql, cn);
            using var reader = command.ExecuteReader();

            cn.Close();

            return reader;
        }

        public static bool CreateProgramDatabase()
        {
            using var cn = new SqlConnection(connectionString);

            cn.Open();

            string sql = "CREATE DATABASE ProArcDB";
            using var command = new SqlCommand(sql, cn);

            try
            {
                using var reader = command.ExecuteReader();
            }
            catch (SqlException ex)
            {
                return false;
            }           

            return true;
        }
    }
}
