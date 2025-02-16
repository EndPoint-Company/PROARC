
using PROARC.src.Models;
using PROARC.src.Models.Tipos;
using static PROARC.src.Control.NetworkControl;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using PROARC.src.Models.Arquivos;

namespace PROARC.src.Control
{
    public static class ReclamadoControl
    {
        public static async Task<Reclamado?> GetAsync(int id)
        {

            var request = new { action = "get_reclamado_por_id", id };
            string response = await SendRequestAsync(request);
            Console.WriteLine(response);

            using JsonDocument doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            if (root.TryGetProperty("reclamado", out JsonElement reclamadoElement) && reclamadoElement.GetArrayLength() == 11)
            {
                string nome = reclamadoElement[0].GetString() ?? string.Empty;
                string? cpf = reclamadoElement[1].GetString() ?? string.Empty;
                string? cnpj = reclamadoElement[2].GetString() ?? string.Empty;
                short? numero = reclamadoElement[3].GetInt16();
                string? logradouro = reclamadoElement[4].GetString() ?? string.Empty;
                string? bairro = reclamadoElement[5].GetString() ?? string.Empty;
                string? cidade = reclamadoElement[6].GetString() ?? string.Empty;
                string? uf = reclamadoElement[7].GetString() ?? string.Empty;
                string? cep = reclamadoElement[8].GetString() ?? string.Empty;
                string? telefone = reclamadoElement[9].GetString() ?? string.Empty;
                string? email = reclamadoElement[10].GetString() ?? string.Empty;

                return new Reclamado(nome, cpf, cnpj, numero, logradouro, bairro, cidade, uf, cep, telefone, email);
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
                        short numero = item[4].GetInt16();
                        string logradouro = item[5].GetString() ?? string.Empty;
                        string bairro = item[6].GetString() ?? string.Empty;
                        string cidade = item[7].GetString() ?? string.Empty;
                        string uf = item[8].GetString() ?? string.Empty;
                        string cep = item[9].GetString() ?? string.Empty;
                        string telefone = item[10].GetString() ?? string.Empty;
                        string email = item[11].GetString() ?? string.Empty;

                        reclamados.Add(new Reclamado(nome, cpf, cnpj, numero, logradouro, bairro, cidade, uf, cep, telefone, email));
                    }
                }
            }

            return reclamados;
        }



        public static async Task<bool> InsertAsync(Reclamado reclamado)
        {
            var request = new { action = "insert_reclamado", reclamado };

            try
            {
                string response = await SendRequestAsync(request);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task UpdateAsync(int id, Reclamado reclamado)
        {
            var request = new { action = "update_reclamado_por_id", id, reclamado };
            await SendRequestAsync(request);
        }

        public static async Task DeleteAsync(int id)
        {
            var request = new { action = "delete_reclamado_por_id", id };
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
