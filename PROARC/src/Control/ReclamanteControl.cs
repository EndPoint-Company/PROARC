
using PROARC.src.Models;
using System;
using System.Collections.Generic;
using static PROARC.src.Control.NetworkControl;
using System.Text.Json;
using System.Threading.Tasks;

namespace PROARC.src.Control
{
    public static class ReclamanteControl
    {
        public static async Task<Reclamante?> GetReclamanteByIdAsync(int id)
        {
            var request = new { action = "get_reclamante_by_id", id };
            string response = await SendRequestAsync(request);

            using JsonDocument doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            if (root.TryGetProperty("reclamante", out JsonElement reclamanteElement) && reclamanteElement.ValueKind == JsonValueKind.Array)
            {
                string nome = reclamanteElement[1].GetString() ?? string.Empty;
                string? rg = reclamanteElement[2].GetString();
                string? cpf = reclamanteElement[3].GetString();

                return new Reclamante(nome, cpf, rg);
            }

            return null;
        }

        public static async Task<Reclamante?> GetReclamanteByCpfAsync(string cpf)
        {
            var request = new { action = "get_reclamante_by_cpf", cpf };
            string response = await SendRequestAsync(request);

            using JsonDocument doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            if (root.TryGetProperty("reclamante", out JsonElement reclamanteElement) && reclamanteElement.ValueKind == JsonValueKind.Array)
            {
                string nome = reclamanteElement[1].GetString() ?? string.Empty;
                string? rg = reclamanteElement[2].GetString();
                string? cpfFromResponse = reclamanteElement[3].GetString();

                return new Reclamante(nome, cpfFromResponse, rg);
            }

            return null;
        }

        public static async Task<Reclamante?> GetReclamanteByRgAsync(string rg)
        {
            var request = new { action = "get_reclamante_by_rg", rg };
            string response = await SendRequestAsync(request);

            using JsonDocument doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            if (root.TryGetProperty("reclamante", out JsonElement reclamanteElement) && reclamanteElement.ValueKind == JsonValueKind.Array)
            {
                string nome = reclamanteElement[1].GetString() ?? string.Empty;
                string? rgFromResponse = reclamanteElement[2].GetString();
                string? cpf = reclamanteElement[3].GetString();

                return new Reclamante(nome, cpf, rgFromResponse);
            }

            return null;
        }

        public static async Task<List<Reclamante>> GetAllReclamantesAsync()
        {
            var request = new { action = "get_all_reclamantes" };
            string response = await SendRequestAsync(request);

            List<Reclamante> reclamantes = new();

            using JsonDocument doc = JsonDocument.Parse(response);
            JsonElement root = doc.RootElement;

            if (root.TryGetProperty("reclamantes", out JsonElement reclamantesArray) && reclamantesArray.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement item in reclamantesArray.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.Array)
                    {
                        string nome = item[1].GetString() ?? string.Empty;
                        string? rg = item[2].GetString();
                        string? cpf = item[3].GetString();

                        reclamantes.Add(new Reclamante(nome, cpf, rg));
                    }
                }
            }

            return reclamantes;
        }

        public static async Task AddReclamanteAsync(Reclamante reclamante)
        {
            var request = new { action = "add_reclamante", reclamante };
            await SendRequestAsync(request);
        }

        public static async Task UpdateReclamanteByIdAsync(int id, Reclamante reclamante)
        {
            var request = new { action = "update_reclamante_by_id", id, reclamante };
            await SendRequestAsync(request);
        }

        public static async Task RemoveReclamanteByIdAsync(int id)
        {
            var request = new { action = "remove_reclamante_by_id", id };
            await SendRequestAsync(request);
        }

        public static async Task<int> CountReclamantesAsync()
        {
            var request = new { action = "count_reclamantes" };
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

