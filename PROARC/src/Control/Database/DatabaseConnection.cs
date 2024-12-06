using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Windows.Security.Authentication.OnlineId;

namespace PROARC.src.Control.Database
{
    public static class DatabaseConnection
    {
        public static async Task<SqlConnection> Connect(string dataSource, string userId, string password, string database)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = dataSource,
                UserID = userId,
                Password = password,
                InitialCatalog = database,
            };

            var connectionString = builder.ConnectionString;

            await using var connection = new SqlConnection(connectionString);

            return connection;
        }
    }
}
