using System;
using System.Collections.Generic;
using static PROARC.src.Control.NetworkControl;
using PROARC.src.Models.Tipos;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using PROARC.src.Models;
using Newtonsoft.Json.Linq;

namespace PROARC.src.Control
{
    public static class MotivoControl
    {
        public static async Task<Motivo?> GetMotivoAsync(string nome)
        {
            var request = new { action = "get_motivo_by_nome", nome };
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

        public static async Task<Motivo?> GetMotivoAsync(int id)
        {
            var request = new { action = "get_motivo_by_id", id };
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

        public static async Task<int?> GetIdMotivoAsync(string nome)
        {
            var request = new { action = "get_id_motivo_by_nome", nome };
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

        public static async Task<List<Motivo>> GetAllMotivosAsync()
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

        public static async Task AddMotivoAsync(Motivo motivo)
        {
            var request = new { action = "add_motivo", motivo };
            await SendRequestAsync(request);
        }

        public static async Task RemoveMotivoAsync(string nome)
        {
            var request = new { action = "remove_motivo_by_nome", nome };
            await SendRequestAsync(request);
        }

        public static async Task UpdateMotivoAsync(string nome, string? novoNome = null, string? novaDescricao = null)
        {
            var request = new { action = "update_motivo_by_id", nome, novoNome, novaDescricao };
            await SendRequestAsync(request);
        }

        public static async Task<int> CountMotivos()
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
