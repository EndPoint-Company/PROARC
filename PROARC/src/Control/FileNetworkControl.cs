using System.Collections.Generic;
using System.IO;
using PROARC.src.Control.Database;
using PROARC.src.Models.Tipos;

namespace PROARC.src.Control
{
    public static class FileNetworkControl
    {
        // Classe pra gerenciar a transferência de arquivos entre Windows (cliente) e Linux (server)
        // Possível utilização de Samba no server side.

        // Localmente
        // Formato dos caminhos (ex.) -> C:/Users/Documentos/Proarc/
        private static string? local_DefaultPath = Local_GetDefaultFolder();

        public static void Local_SetDefaultFolder(string path)
        {
            string sql = $"USE ProArc; INSERT INTO DefaultPath (remote, local) VALUES (NULL, '{path}');";
            DatabaseOperations.QuerySqlCommandNoReturn(sql);

            local_DefaultPath = path;
        }

        public static string? Local_GetDefaultFolder()
        {
            // Revisar
            string sql = $"USE ProArc; SELECT local FROM DefaultPath;";
            List<string> local = DatabaseOperations.QuerySqlCommand(sql);

            return local[0];
        }

        public static void Local_CriarProcessoAdministrativo(string numeroProcesso)
        {
            Directory.CreateDirectory($"{local_DefaultPath}/{numeroProcesso}");
        }

        public static void Local_CriarDiretorio(ArquivoTipo tipo, string numeroProcesso)
        {
            Directory.CreateDirectory($"{local_DefaultPath}/{numeroProcesso}/{tipo}");
        }

        public static void Local_AdicionarAquivoToDiretorio(ArquivoTipo tipo, string numeroProcesso, string arquivoPath)
        {
            File.Copy(arquivoPath, $"{local_DefaultPath}/{numeroProcesso}/{tipo}/" + System.IO.Path.GetFileName(arquivoPath));
        }

        public static void Local_CriarAllDiretorios(string numero_processo)
        {
            Local_CriarDiretorio(ArquivoTipo.TermoDeReclamação, numero_processo);
            Local_CriarDiretorio(ArquivoTipo.Notificacao, numero_processo);
            Local_CriarDiretorio(ArquivoTipo.Procuracao, numero_processo);
            Local_CriarDiretorio(ArquivoTipo.AtaDeAudiencia, numero_processo);
            Local_CriarDiretorio(ArquivoTipo.OutrosAnexos, numero_processo);
        }
    }
}
