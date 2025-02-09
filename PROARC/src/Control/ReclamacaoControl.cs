using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static PROARC.src.Control.NetworkControl;
using PROARC.src.Models;
using PROARC.src.Models.Arquivos;
using PROARC.src.Models.Tipos;

namespace PROARC.src.Control
{
    public class ReclamacaoControl
    {

        public static async Task<Reclamacao> GetAsync(string titulo)
        {
            var request = new { action = "get_reclamacao_por_titulo", titulo };
            string response = await SendRequestAsync(request);
            Console.WriteLine(response);
            using JsonDocument doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            /*
            if (root.TryGetProperty("reclamacao", out JsonElement motivoElement) && motivoElement.GetArrayLength() == 1)
            {
                string nomeMotivo = motivoElement[0].GetString() ?? string.Empty;
                return new Motivo(nomeMotivo);
            }
            */

            return null;
        }

        public static async Task InsertAsync(List<Reclamado> reclamados)
        {
           var request = new { action = "insert_reclamacao", reclamados };
        }

        public static async Task DeleteAsync(string titulo)
        {
            var request = new { action = "delete_reclamacao_por_titulo", titulo };
            await SendRequestAsync(request);
        }

        public static async Task<int> CountAsync()
        {
            var request = new { action = "count_reclamacoes" };
            string response =  await SendRequestAsync(request);

            using JsonDocument doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            if (root.TryGetProperty("count", out JsonElement countElement))
            {
                return countElement.GetInt32();
            }

            return 0;
        }
    }
}
