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

namespace PROARC.src.Control
{
    public class ProcessoAdministrativoControl : IDatabaseCRUD<ProcessoAdministrativo>
    {
        public static async Task<List<ProcessoAdministrativo>?> GetAll()
        {
            var request = new { action = "get_all_processos" };

            string response = await SendRequestAsync(request);
            Console.WriteLine("opaaa");
            var jo = JObject.Parse(response);
            Console.WriteLine(jo);

            List<ProcessoAdministrativo> processos = new List<ProcessoAdministrativo>();

            foreach (var a in jo.Values())
            {
                var list = JsonConvert.DeserializeObject<List<List<object>>>((string)a);
                foreach (List<object> b in list)
                {
                    ProcessoAdministrativo processo = new ProcessoAdministrativo();

                    Motivo motivo = await MotivoControl.GetMotivoAsync((int)b[1]);
                    processo.Motivo = motivo;

                    Reclamante reclamante = await ReclamanteControl.GetReclamanteAsync((int)b[2]);
                    processo.Reclamante = reclamante;

                    processo.Titulo = (string)b[3];

                    processo.Status = Status.EmTramitacaoAguardandoDocumentacao;

                    processo.CaminhoDoProcesso = (string)b[5];

                    processo.Ano = (short)b[6];

                    processo.DataDaAudiencia = (DateTime)b[7];

                    processos.Add(processo);
                }

            }
            return null;
        }

        public static async Task<object?> Get(int id)
        {
            return null;
        }

        public static async Task<bool> Insert(object obj)
        {
            return false;
        }

        public static async Task<bool> Update(object obj)
        {
            return false;
        }

        public static async Task<bool> Delete(int id)
        {
            return false;
        }
    }
}
