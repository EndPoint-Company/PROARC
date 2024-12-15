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
                DatabaseOperations.QuerySqlCommandNoReturn("USE ProArc;" +
                    "CREATE TABLE Reclamados(" +
                    "reclamado_id INT PRIMARY KEY NOT NULL IDENTITY(1,1)," +
                    "nome NVARCHAR(100) NOT NULL," +
                    "cpf NVARCHAR(11) NULL," +
                    "cnpj NVARCHAR(14) NULL," +
                    "numero_rua SMALLINT NULL," +
                    "email NVARCHAR(100) NULL CONSTRAINT chk_email CHECK (email like '%_@__%.__%')," +
                    "rua NVARCHAR(100) NULL," +
                    "bairro NVARCHAR(100) NULL," +
                    "cidade NVARCHAR(100) NULL," +
                    "uf NVARCHAR(2) NULL," +
                    ");");
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
                    "chave_acesso NVARCHAR(10) NOT NULL UNIQUE," +
                    ");");
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
                DatabaseOperations.QuerySqlCommandNoReturn("USE ProArc;" +
                    "CREATE TABLE Motivos(" +
                    "motivo_id INT PRIMARY KEY NOT NULL IDENTITY(1,1)," +
                    "nome NVARCHAR(100) NOT NULL," +
                    "descricao NVARCHAR(600) NULL," +
                    "data_criacao DATETIME NOT NULL," +
                    ");");
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
                DatabaseOperations.QuerySqlCommandNoReturn("USE ProArc;" +
                    "CREATE TABLE Reclamantes(" +
                    "reclamante_id INT PRIMARY KEY NOT NULL IDENTITY(1,1)," +
                    "nome NVARCHAR(100) NOT NULL," +
                    "rg NVARCHAR(20) NOT NULL," +
                    "cpf NVARCHAR(11) NULL," +
                    ");");
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
                DatabaseOperations.QuerySqlCommandNoReturn("USE ProArc;" +
                    "CREATE TABLE ProcessosAdministrativos(" +
                    "processo_id INT PRIMARY KEY NOT NULL IDENTITY(1,1)," +
                    "motivo_id INT," +
                    "reclamante_id INT," +
                    "reclamado_id INT," +
                    "numero_processo NVARCHAR(10) NOT NULL UNIQUE," +
                    "caminho_processo NVARCHAR(200) NOT NULL," +
                    "ano SMALLINT NOT NULL," +
                    "data_audiencia DATE NULL," +
                    "" +
                    "FOREIGN KEY (reclamante_id) REFERENCES Reclamantes(reclamante_id)" +
                    "ON DELETE CASCADE" +
                    "    ON UPDATE CASCADE," +
                    "FOREIGN KEY (motivo_id) REFERENCES Motivos(motivo_id)" +
                    "ON DELETE CASCADE" +
                    "    ON UPDATE CASCADE," +
                    "FOREIGN KEY (reclamado_id) REFERENCES Reclamados(reclamado_id)" +
                    "ON DELETE CASCADE" +
                    "    ON UPDATE CASCADE," +
                    ");");
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
                DatabaseOperations.QuerySqlCommandNoReturn("USE ProArc;" +
                    "CREATE TABLE Diretorios(" +
                    "processo_id INT," +
                    "tipo NVARCHAR(17) NOT NULL," +
                    "data_criacao DATETIME NOT NULL," +
                    "data_modificacao DATETIME NOT NULL," +
                    "" +
                    "FOREIGN KEY (processo_id) REFERENCES ProcessosAdministrativos(processo_id)" +
                    "ON DELETE CASCADE" +
                    "    ON UPDATE CASCADE," +
                    ");");
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
                DatabaseOperations.QuerySqlCommandNoReturn("USE ProArc;" +
                    "CREATE TABLE Arquivos(" +
                    "processo_id INT," +
                    "nome NVARCHAR(100) NOT NULL," +
                    "tipo NVARCHAR(17) NOT NULL," +
                    "data_criacao DATETIME NOT NULL," +
                    "data_modificacao DATETIME NOT NULL," +
                    "" +
                    "FOREIGN KEY (processo_id) REFERENCES ProcessosAdministrativos(processo_id)" +
                    "ON DELETE CASCADE" +
                    "    ON UPDATE CASCADE," +
                    ");");
            }
            catch (SqlException)
            {
                return false;
            }

            return true;
        }

        public static bool CreateDefaultPathTable()
        {
            try
            {
                DatabaseOperations.QuerySqlCommandNoReturn("USE ProArc;" +
                    "CREATE TABLE DefaultPath(" +
                    "remote NVARCHAR(200) NULL," +
                    "local NVARCHAR(200) NULL," +
                    ");");
            }
            catch (SqlException)
            {
                return false;
            }

            return true;
        }
    }
}
