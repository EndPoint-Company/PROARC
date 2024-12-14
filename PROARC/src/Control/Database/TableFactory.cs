using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace PROARC.src.Control.Database
{
    public class TableFactory
    {
        public static bool CreateReclamadoTable()
        {
            try
            {
                DatabaseOperations.QuerySqlCommandNoReturn("USE ProArc;CREATE TABLE Usuarios(\r\n\tusuario_id INT PRIMARY KEY NOT NULL IDENTITY(1,1),\r\n\tnome NVARCHAR(100) NOT NULL,\r\n\tnivel_permissao SMALLINT NOT NULL CONSTRAINT digito_unico CHECK (nivel_permissao BETWEEN 0 and 4),\r\n\tchave_acesso NVARCHAR(10) NOT NULL UNIQUE,\r\n);");
            }
            catch (SqlException)
            {
                return false;
            }


            return true;
        }

        public static bool CreateUsuarioTable()
        {
            try
            {
                DatabaseOperations.QuerySqlCommandNoReturn("USE ProArc;" +
                    "CREATE TABLE Usuarios(" +
                    "usuario_id INT PRIMARY KEY NOT NULL IDENTITY(1,1)," +
                    "nome NVARCHAR(100) NOT NULL," +
                    "nivel_permissao SMALLINT NOT NULL CONSTRAINT digito_unico CHECK (nivel_permissao BETWEEN 0 and 4)," +
                    "chave_acesso NVARCHAR(10) NOT NULL UNIQUE,);");
            }
            catch (SqlException)
            {
                return false;
            }


            return true;
        }

        public static bool CreateMotivoTable()
        {
            try
            {
                DatabaseOperations.QuerySqlCommandNoReturn("USE ProArc;CREATE TABLE Usuarios(\r\n\tusuario_id INT PRIMARY KEY NOT NULL IDENTITY(1,1),\r\n\tnome NVARCHAR(100) NOT NULL,\r\n\tnivel_permissao SMALLINT NOT NULL CONSTRAINT digito_unico CHECK (nivel_permissao BETWEEN 0 and 4),\r\n\tchave_acesso NVARCHAR(10) NOT NULL UNIQUE,\r\n);");
            }
            catch (SqlException)
            {
                return false;
            }


            return true;
        }

        public static bool CreateReclamanteTable()
        {
            try
            {
                DatabaseOperations.QuerySqlCommandNoReturn("USE ProArc;CREATE TABLE Usuarios(\r\n\tusuario_id INT PRIMARY KEY NOT NULL IDENTITY(1,1),\r\n\tnome NVARCHAR(100) NOT NULL,\r\n\tnivel_permissao SMALLINT NOT NULL CONSTRAINT digito_unico CHECK (nivel_permissao BETWEEN 0 and 4),\r\n\tchave_acesso NVARCHAR(10) NOT NULL UNIQUE,\r\n);");
            }
            catch (SqlException)
            {
                return false;
            }


            return true;
        }

        public static bool CreateProcessoAdministrativoTable()
        {
            try
            {
                DatabaseOperations.QuerySqlCommandNoReturn("USE ProArc;CREATE TABLE Usuarios(\r\n\tusuario_id INT PRIMARY KEY NOT NULL IDENTITY(1,1),\r\n\tnome NVARCHAR(100) NOT NULL,\r\n\tnivel_permissao SMALLINT NOT NULL CONSTRAINT digito_unico CHECK (nivel_permissao BETWEEN 0 and 4),\r\n\tchave_acesso NVARCHAR(10) NOT NULL UNIQUE,\r\n);");
            }
            catch (SqlException)
            {
                return false;
            }


            return true;
        }

        public static bool CreateDiretorioTable()
        {
            try
            {
                DatabaseOperations.QuerySqlCommandNoReturn("USE ProArc;CREATE TABLE Usuarios(\r\n\tusuario_id INT PRIMARY KEY NOT NULL IDENTITY(1,1),\r\n\tnome NVARCHAR(100) NOT NULL,\r\n\tnivel_permissao SMALLINT NOT NULL CONSTRAINT digito_unico CHECK (nivel_permissao BETWEEN 0 and 4),\r\n\tchave_acesso NVARCHAR(10) NOT NULL UNIQUE,\r\n);");
            }
            catch (SqlException)
            {
                return false;
            }


            return true;
        }

        public static bool CreateArquivoTable()
        {
            try
            {
                DatabaseOperations.QuerySqlCommandNoReturn("USE ProArc;CREATE TABLE Usuarios(\r\n\tusuario_id INT PRIMARY KEY NOT NULL IDENTITY(1,1),\r\n\tnome NVARCHAR(100) NOT NULL,\r\n\tnivel_permissao SMALLINT NOT NULL CONSTRAINT digito_unico CHECK (nivel_permissao BETWEEN 0 and 4),\r\n\tchave_acesso NVARCHAR(10) NOT NULL UNIQUE,\r\n);");
            }
            catch (SqlException)
            {
                return false;
            }


            return true;
        }
    }
}
