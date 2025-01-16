using System;
using System.Collections.Generic;
using static PROARC.src.Control.NetworkControl;
using PROARC.src.Models.Tipos;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Control
{
    public static class MotivoControl
    {
        public static async Task<Motivo?> GetMotivoAsync(string nome)
        {
            var request = new { action = "get_motivo", nome };
            string response = await SendRequestAsync(request);

            using JsonDocument doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            if (root.TryGetProperty("motivo", out JsonElement motivoElement) && motivoElement.GetArrayLength() == 2)
            {
                string nomeMotivo = motivoElement[0].GetString() ?? string.Empty;
                string descricaoMotivo = motivoElement[1].GetString() ?? string.Empty;

                return new Motivo(nomeMotivo, descricaoMotivo);
            }

            return null;
        }

        public static async Task<Motivo?> GetMotivoAsync(int id)
        {
            var request = new { action = "get_motivo_id", id };
            string response = await SendRequestAsync(request);

            using JsonDocument doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            if (root.TryGetProperty("motivo", out JsonElement motivoElement) && motivoElement.GetArrayLength() == 2)
            {
                string nomeMotivo = motivoElement[0].GetString() ?? string.Empty;
                string descricaoMotivo = motivoElement[1].GetString() ?? string.Empty;

                return new Motivo(nomeMotivo, descricaoMotivo);
            }

            return null;
        }

        public static async Task<int?> GetIdMotivoAsync(string nome)
        {
            var request = new { action = "get_id_motivo", nome };
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

            List<Motivo> motivos = new();

            using JsonDocument doc = JsonDocument.Parse(response);
            JsonElement root = doc.RootElement;


            string rootString = root.ToString();
            List<char> reader = rootString.ToList();

            string nome = string.Empty;
            string descricao = string.Empty;
            bool isNome = true;

            StringBuilder currentString = new StringBuilder();

            foreach (char ch in reader)
            {
                if (ch == ',' || ch == '}')
                {
                    if (isNome)
                    {
                        nome = currentString.ToString();
                        isNome = false;
                    }
                    else
                    {
                        descricao = currentString.ToString();
                        motivos.Add(new Motivo(nome, descricao));
                        isNome = true;
                    }

                    currentString.Clear();
                }
                else if (ch != '{' && ch != '[' && ch != ']' && ch != '"')
                {
                    currentString.Append(ch);
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
            var request = new { action = "remove_motivo", nome };
            await SendRequestAsync(request);
        }

        public static async Task UpdateMotivoAsync(string nome, string? novoNome = null, string? novaDescricao = null)
        {
            var request = new { action = "update_motivo", nome, novoNome, novaDescricao };
            await SendRequestAsync(request);
        }
    }
}
