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

namespace PROARC.src.Control
{
    public class ProcessoAdministrativoControl : IDatabaseCRUD<ProcessoAdministrativo>
    {
        public static async Task<List<ProcessoAdministrativo>?> GetAll()
        {
            var request = new { action = "get_all_processos" };

            string response = await SendRequestAsync(request);
            var jo = JObject.Parse(response);
            Console.WriteLine(jo);

            List<ProcessoAdministrativo> processos = new List<ProcessoAdministrativo>();

            foreach (var a in jo.Values())
            {
                var list = JsonConvert.DeserializeObject<List<List<object>>>((string)a);
                foreach (List<object> b in list)
                {
                    ProcessoAdministrativo processo = new ProcessoAdministrativo();
                    Console.WriteLine("!!!!!!!!");

                    Motivo? motivo = await MotivoControl.GetMotivoAsync(Convert.ToInt32(b[1])).ConfigureAwait(false);
                    Console.WriteLine("!!!!!!!!");
                    processo.Motivo = motivo;
                    Console.WriteLine("!!!!!!!!");

                    Reclamante? reclamante = await ReclamanteControl.GetReclamanteByIdAsync(Convert.ToInt32(b[2])).ConfigureAwait(false);
                    processo.Reclamante = reclamante;
                    Console.WriteLine("!!!!!!!!");

                    processo.Titulo = (string)b[3];

                    processo.Status = Status.EmTramitacaoAguardandoDocumentacao;

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

                    Console.WriteLine(processo.ToString());

                    processos.Add(processo);
                }

            }

            return processos;
        }

        public static async Task<ProcessoAdministrativo?> Get(int id)
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
                    Console.WriteLine("!!!!!!!!");

                    Motivo? motivo = await MotivoControl.GetMotivoAsync(Convert.ToInt32(b[1])).ConfigureAwait(false);
                    Console.WriteLine("!!!!!!!!");
                    processo.Motivo = motivo;
                    Console.WriteLine("!!!!!!!!");

                    Reclamante? reclamante = await ReclamanteControl.GetReclamanteByIdAsync(Convert.ToInt32(b[2])).ConfigureAwait(false);
                    processo.Reclamante = reclamante;
                    Console.WriteLine("!!!!!!!!");

                    processo.Titulo = (string)b[3];

                    processo.Status = Status.EmTramitacaoAguardandoDocumentacao;

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

                    Console.WriteLine(processo.ToString());
                }

            }

            return processo;
        }

        public static async Task<bool> Insert(ProcessoAdministrativo processo)
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

        public static async Task<bool> Update
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

        public static async Task<bool> Delete(int id)
        {
            var request = new { action = "remove_processo_by_id", id };
            try { await SendRequestAsync(request); }
            catch (Exception) { return false; }
            return true;
        }
}
