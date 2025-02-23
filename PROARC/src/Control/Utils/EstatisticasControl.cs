using static PROARC.src.Control.NetworkControl;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace PROARC.src.Control.Utils
{
    public class EstatisticasControl
    {
        public static async Task<List<string>> GetAsync(int quantidade)
        {
            var request = new { action = "estatistica_mais_reclamados", quantidade };
            string response = await SendRequestAsync(request);

            using JsonDocument doc = JsonDocument.Parse(response);
            JsonElement root = doc.RootElement;
            JsonElement reclamados = root.GetProperty("reclamados");

            List<string> listaFormatada = new List<string>();

            foreach (JsonElement item in reclamados.EnumerateArray())
            {
                string empresa = item[0].GetString();
                int quantidadeReclamacoes = item[1].GetInt32();
                listaFormatada.Add($"{empresa}: {quantidadeReclamacoes}");
            }

            return listaFormatada;

        }

        public static async Task<List<string>> GetMotivosNumAsync()
        {
            var request = new { action = "estatistica_motivos_mais_usados"};
            string response = await SendRequestAsync(request);
            Console.WriteLine(response);

            using JsonDocument doc = JsonDocument.Parse(response);
            JsonElement root = doc.RootElement;
            JsonElement estatisticas = root.GetProperty("estatisticas");

            List<string> listaFormatada = new List<string>();

            foreach (JsonElement item in estatisticas.EnumerateArray())
            {
                string motivo = item[0].GetString();
                int quantidade = item[1].GetInt32();
                listaFormatada.Add($"{motivo}: {quantidade}");
            }

            return listaFormatada;

        }

        public static async Task<List<string>> GetReclamadosPorMesAsync()
        {
            var request = new { action = "estatistica_reclamacoes_por_mes_ano_atual" };
            string response = await SendRequestAsync(request);
            using JsonDocument doc = JsonDocument.Parse(response);
            JsonElement root = doc.RootElement;
            JsonElement estatisticas = root.GetProperty("estatisticas");

            List<string> listaFormatada = new List<string>();

            foreach (JsonProperty item in estatisticas.EnumerateObject())
            {
                string mes = item.Name; // Número do mês como string
                int quantidade = item.Value.GetInt32(); // Quantidade de reclamações
                listaFormatada.Add($"Mês {mes}: {quantidade}");
            }

            return listaFormatada;

        }
    }
}
