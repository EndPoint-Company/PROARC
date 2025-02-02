using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using PROARC.src.Models.Arquivos;
using PROARC.src.Models.Tipos;
using PROARC.src.Models;
using Newtonsoft.Json;
using static PROARC.src.Control.NetworkControl;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Diagnostics;
using System.Threading;

namespace PROARC.src.Control
{
    public class ProcessoAdministrativoControl
    {
        public class ProcessoAdministrativoDto
        {
            public int processo_id { get; set; }
            public MotivoDto? motivo { get; set; }
            public ReclamanteDto reclamante { get; set; }
            public List<ReclamadoDto>? reclamados { get; set; }
            public string titulo_processo { get; set; }
            public string status_processo { get; set; }
            public string path_processo { get; set; }
            public int ano { get; set; }
            public DateTime? data_audiencia { get; set; }
            public DateTime? created_at { get; set; }
        }

        public class MotivoDto
        {
            public int motivo_id { get; set; }
            public string nome { get; set; }
        }

        public class ReclamanteDto
        {
            public int reclamante_id { get; set; }
            public string nome { get; set; }
            public string rg { get; set; }
            public string cpf { get; set; }
        }

        public class ReclamadoDto
        {
            public int reclamado_id { get; set; }
            public string nome { get; set; }
            public string? cpf { get; set; }
            public string? cnpj { get; set; }
            public short? numero_rua { get; set; }
            public string? email { get; set; }
            public string? rua { get; set; }
            public string? bairro { get; set; }
            public string? cidade { get; set; }
            public string? uf { get; set; }
        }

        public static async Task<List<ProcessoAdministrativo>?> GetAllAsync()
        {
            var request = new { action = "new_get_all_processos" };
            string response = await NetworkControl.SendRequestAsync(request);

            try
            {
                var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, List<ProcessoAdministrativoDto>>>(response);
                if (jsonResponse != null && jsonResponse.TryGetValue("processos", out var processosDto))
                {
                    var processos = processosDto.Select(p => new ProcessoAdministrativo(
                        p.path_processo,
                        p.titulo_processo,
                        (short)p.ano,
                        p.status_processo,
                        p.motivo != null ? new Motivo(p.motivo.nome) : null,
                        p.reclamados?.FirstOrDefault() != null ? new Reclamado(
                            p.reclamados.First().nome,
                            p.reclamados.First().numero_rua,
                            p.reclamados.First().rua,
                            p.reclamados.First().bairro,
                            p.reclamados.First().email,
                            p.reclamados.First().cidade,
                            p.reclamados.First().uf,
                            p.reclamados.First().cnpj,
                            p.reclamados.First().cpf) : null,
                        new Reclamante(p.reclamante.nome, p.reclamante.cpf, p.reclamante.rg),
                        p.data_audiencia,
                        p.created_at)).ToList();

                    return processos;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao desserializar JSON: {ex.Message}");
            }

            return null;
        }

        public static async Task<ProcessoAdministrativo?> GetAsync(int id)
        {
            var request = new { action = "get_processo_by_id", id };

            string response = await SendRequestAsync(request);
            Console.WriteLine(response);
            ProcessoAdministrativo processo = new();

            var jo = JObject.Parse(response);
            Console.WriteLine(jo);

            foreach (var a in jo.Values())
            {
                var list = JsonConvert.DeserializeObject<List<List<object>>>((string)a);
                foreach (List<object> b in list)
                {
                    Motivo? motivo = await MotivoControl.GetAsync(Convert.ToInt32(b[1])).ConfigureAwait(false);
                    processo.Motivo = motivo;

                    Reclamante? reclamante = await ReclamanteControl.GetAsync(Convert.ToInt32(b[2])).ConfigureAwait(false);
                    processo.Reclamante = reclamante;

                    processo.Titulo = (string)b[3];

                    processo.Status = (string)b[4];

                    processo.CaminhoDoProcesso = (string)b[5];

                    processo.Ano = Convert.ToInt16(b[6]);

                    try
                    {
                        processo.DataDaAudiencia = DateTime.Parse((string)b[7], CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                        processo.DataDaAudiencia = null;
                    }
                }

            }

            return processo;
        }

        public static async Task<bool> InsertAsync(ProcessoAdministrativo processo)
        {
            var request = new { action = "add_processo", processo };

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

        public static async Task<bool> UpdateAsync
            (int id, int motivo_id, int reclamante_id,
            string titulo_processo, short ano, Status status_processo,
            string path_processo, string data_audiencia)
        {
            var request = new
            {
                action = "update_processo_by_id",
                id,
                motivo_id,
                reclamante_id,
                titulo_processo,
                ano,
                status_processo,
                path_processo,
                data_audiencia
            };

            try { await SendRequestAsync(request); }
            catch (Exception) { return false; }

            return true;
        }

        //mudar futuramente quando as criacoes de processo com mais de um reclamado estejam ajustadas
        public static async Task<Reclamado> GetReclamadoFromRelacao(int processo_id)
        {
            var request = new { action = "get_reclamado_from_relacao_by_processo_id", processo_id };
            string response = await SendRequestAsync(request);
            using JsonDocument doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            int numero = root.GetProperty("reclamados")[0][0].GetInt32();

            Reclamado reclamado = await ReclamadoControl.GetAsync(numero);

            return reclamado;

        }

        public static async Task<bool> DeleteAsync(int id)
        {
            var request = new { action = "remove_processo_by_id", id };
            try { await SendRequestAsync(request); }
            catch (Exception) { return false; }
            return true;
        }

        public static async Task<int> CountAsync()
        {
            var request = new { action = "count_processos" };
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
