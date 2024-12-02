using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models
{
    class ProcessoAdministrativo
    {
        private int ano;
        private Motivo motivo;
        private Reclamado reclamado;
        private Reclamante reclamante;
        private Dictionary<String, Arquivo> arquivo; // String ex. -> 0001/2024 (numero / ano)

        public Motivo Motivo { get; set; }
        public Reclamado Reclamado { get; set; }
        public Reclamante Reclamante { get; set; }
        public Arquivo Arquivo { get; set; }
    }
}
