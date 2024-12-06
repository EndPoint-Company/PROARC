using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace PROARC.src.Control.Database
{
    public static class DatabaseOperations
    {
        public static (bool, Exception) ValidateUserLogin(SecureString acessKey)
        {
            var cn = DatabaseConnection.Connect("", "", "", "").Result;
            var cmd = new SqlCommand() { Connection = cn };

            cmd.CommandText = "SELECT Id,[dbo].[AcessKey_Check] (@AcessKey) as ValidItem FROM dbo.Users1";

            cmd.Parameters.Add("@AcessKey", SqlDbType.NChar).Value = acessKey.ToUnSecureString();

            try
            {
                cn.Open();

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    return (reader.GetValue(1) != DBNull.Value, null)!;
                }
                else
                {
                    return (false, null)!;
                }
            }
            catch (Exception exception)
            {
                return (false, exception);

            }
        }
    }
}
