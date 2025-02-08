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
        public static async Task<Reclamante?> GetAsync(int id)
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
                string? telefone = reclamanteElement[4].GetString();
                string? email = reclamanteElement[5].GetString();

                return new Reclamante(nome, cpf, rg, telefone, email);
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
                string? cpf2 = reclamanteElement[3].GetString();
                string? telefone = reclamanteElement[4].GetString();
                string? email = reclamanteElement[5].GetString();

                return new Reclamante(nome, cpf2, rg, telefone, email);
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
                string? rg2 = reclamanteElement[2].GetString();
                string? cpf = reclamanteElement[3].GetString();
                string? telefone = reclamanteElement[4].GetString();
                string? email = reclamanteElement[5].GetString();

                return new Reclamante(nome, cpf, rg2, telefone, email);
            }

            return null;
        }

        public static async Task<List<Reclamante>> GetAllAsync()
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
                        string? telefone = item[2].GetString();
                        string? email = item[2].GetString();

                        reclamantes.Add(new Reclamante(nome, cpf, rg, telefone, email));
                    }
                }
            }

            return reclamantes;
        }

        public static async Task InsertAsync(Reclamante reclamante)
        {
            var request = new { action = "add_reclamante", reclamante };
            await SendRequestAsync(request);
        }

        public static async Task UpdateAsync(int id, Reclamante reclamante)
        {
            var request = new { action = "update_reclamante_by_id", id, reclamante };
            await SendRequestAsync(request);
        }

        public static async Task DeleteAsync(int id)
        {
            var request = new { action = "remove_reclamante_by_id", id };
            await SendRequestAsync(request);
        }

        public static async Task<int> CountAsync()
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

