using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROARC.src.Models.Arquivos;
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
            // TODO: colocar path na database
            // Tabela -> local_path varchar
            //           remote_path varchar

            local_DefaultPath = path;
        }

        public static string? Local_GetDefaultFolder()
        {
            // Pegar caminho na database
            return null;
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
            File.Copy(arquivoPath, $"{local_DefaultPath}/{numeroProcesso}/{tipo}/" + Path.GetFileName(arquivoPath));
        }
    }
}
