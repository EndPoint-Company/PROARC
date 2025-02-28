using System;
using System.Collections.Generic;
using static PROARC.src.Control.NetworkControl;
using PROARC.src.Models.Tipos;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PROARC.src.Control
{
    public class MotivoControl
    {
        public static async Task<Motivo?> GetAsync(string nome)
        {
            var request = new { action = "get_motivo_por_nome", nome };
            string response = await SendRequestAsync(request);

            using JsonDocument doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            if (root.TryGetProperty("motivo", out JsonElement motivoElement) && motivoElement.GetArrayLength() == 1)
            {
                string nomeMotivo = motivoElement[0].GetString() ?? string.Empty;      
                return new Motivo(nomeMotivo);
            }

            return null;
        }

        public static async Task<Motivo?> GetAsync(int id)
        {
            var request = new { action = "get_motivo_por_id", id };
            string response = await SendRequestAsync(request);

            using JsonDocument doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            if (root.TryGetProperty("motivo", out JsonElement motivoElement) && motivoElement.GetArrayLength() == 1)
            {
                string nomeMotivo = motivoElement[0].GetString() ?? string.Empty;

                return new Motivo(nomeMotivo);
            }

            return null;
        }

        public static async Task<int?> GetIdAsync(string nome)
        {
            var request = new { action = "get_motivo_id_por_nome", nome };
            string response = await SendRequestAsync(request);

            using JsonDocument doc = JsonDocument.Parse(response);
            var root = doc.RootElement;
            string rootstring = root.ToString();

            List<char> reader = rootstring.ToList();
            String numero = string.Empty;

            foreach (char ch in reader)
            {
                if (char.IsDigit(ch))
                {
                    numero += ch;
                }
            }

            return int.Parse(numero);
        }

        public static async Task<List<Motivo>> GetAllAsync()
        {
            var request = new { action = "get_all_motivos" };

            string response = await SendRequestAsync(request);
            JObject jsonResponse = JObject.Parse(response);

            List<Motivo> motivos = new List<Motivo>();

            foreach (JToken motivosList in jsonResponse.Values())
            {
                foreach (var motivoNome in motivosList.Values())
                {
                    Motivo motivo = new((string)motivoNome);
                    motivos.Add(motivo);
                }
            }

            return motivos;
        }

        public static async Task InsertAsync(Motivo motivo)
        {
            var request = new { action = "insert_motivo", motivo };
            await SendRequestAsync(request);
        }

        public static async Task DeleteAsync(string nome)
        {
            var request = new { action = "delete_motivo_by_nome", nome };
            await SendRequestAsync(request);
        }

        public static async Task UpdateAsync(string nome, string? novoNome = null)
        {
            var request = new { action = "update_motivo_por_id", nome, novoNome};
            await SendRequestAsync(request);
        }

        public static async Task<int> CountAsync()
        {
            var request = new { action = "count_motivos" };
            string response = await SendRequestAsync(request);

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
