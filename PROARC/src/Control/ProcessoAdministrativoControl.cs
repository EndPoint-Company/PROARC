using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using PROARC.src.Models.Arquivos;
using PROARC.src.Models.Tipos;
using static PROARC.src.Control.NetworkControl;

namespace PROARC.src.Control
{
    public class ProcessoAdministrativoControl : IDatabaseCRUD<ProcessoAdministrativo>
    {
        public static async Task<List<ProcessoAdministrativo>?> GetAll()
        {
            var request = new { action = "get_all_processos" };

            string response = await SendRequestAsync(request);

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
