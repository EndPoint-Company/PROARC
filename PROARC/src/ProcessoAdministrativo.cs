using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src
{
    class ProcessoAdministrativo
    {
        private Motivo motivo;
        private Reclamado reclamado;
        private Reclamante reclamante;
        private Arquivo arquivo;

        public required Motivo Motivo { get; set; }
        public required Reclamado Reclamado { get; set; }
        public required Reclamante Reclamante { get; set; }
        public required Arquivo Arquivo { get; set; }
    }
}
