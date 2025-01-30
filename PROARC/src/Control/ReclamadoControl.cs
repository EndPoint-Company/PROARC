
using PROARC.src.Models;
using PROARC.src.Models.Tipos;
using static PROARC.src.Control.NetworkControl;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace PROARC.src.Control
{
    public static class ReclamadoControl
    {
        public static async Task<Reclamado?> GetAsync(int id)
        {

            var request = new { action = "get_reclamado_by_id", id };
            string response = await SendRequestAsync(request);

            using JsonDocument doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            if (root.TryGetProperty("reclamado", out JsonElement reclamadoElement) && reclamadoElement.GetArrayLength() == 9)
            {
                string nome = reclamadoElement[0].GetString() ?? string.Empty;
                string? cpf = reclamadoElement[1].GetString() ?? string.Empty;
                string? cnpj = reclamadoElement[2].GetString() ?? string.Empty;
                short? numeroRua = reclamadoElement[3].GetInt16();
                string? email = reclamadoElement[4].GetString() ?? string.Empty;
                string? rua = reclamadoElement[5].GetString() ?? string.Empty;
                string? bairro = reclamadoElement[6].GetString() ?? string.Empty;
                string? cidade = reclamadoElement[7].GetString() ?? string.Empty;
                string? estado = reclamadoElement[8].GetString() ?? string.Empty;

                return new Reclamado(nome, numeroRua, rua, bairro, email, cidade, estado, cnpj, cpf);
            }

            return null;
        }

        public static async Task<List<Reclamado>> GetAllAsync()
        {
            var request = new { action = "get_all_reclamados" };
            string response = await SendRequestAsync(request);

            List<Reclamado> reclamados = new();

            using JsonDocument doc = JsonDocument.Parse(response);
            JsonElement root = doc.RootElement;
            
            if (root.TryGetProperty("reclamados", out JsonElement reclamadosArray) && reclamadosArray.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement item in reclamadosArray.EnumerateArray())
                {
                    
                    if (item.ValueKind == JsonValueKind.Array)
                    {
                        int id = item[0].GetInt32();
                        string nome = item[1].GetString() ?? string.Empty;
                        string? cpf = item[2].GetString();
                        string? cnpj = item[3].GetString();
                        short numeroRua = item[4].GetInt16();
                        string email = item[5].GetString() ?? string.Empty;
                        string rua = item[6].GetString() ?? string.Empty;
                        string bairro = item[7].GetString() ?? string.Empty;
                        string cidade = item[8].GetString() ?? string.Empty;
                        string estado = item[9].GetString() ?? string.Empty;

                        reclamados.Add(new Reclamado(nome, numeroRua, rua, bairro, email, cidade, estado, cnpj, cpf));
                    }
                }
            }

            return reclamados;
        }



        public static async Task InsertAsync(Reclamado reclamado)
        {
            var request = new { action = "add_reclamado", reclamado };
            Console.WriteLine(request);
            await SendRequestAsync(request);
        }

        public static async Task UpdateAsync(int id, Reclamado reclamado)
        {
            var request = new { action = "update_reclamado_by_id", id, reclamado };
            await SendRequestAsync(request);
        }

        public static async Task DeleteAsync(int id)
        {
            var request = new { action = "remove_reclamado_by_id", id };
            await SendRequestAsync(request);
        }

        public static async Task<int> CountAsync()
        {
            var request = new { action = "count_reclamados" };
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
