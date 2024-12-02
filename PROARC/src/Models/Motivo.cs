using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models
{
    public class Motivo
    {
        private string motivoNome;
        private DateTime? dataDeCriacao;

        public required string MotivoNome { get; set; }
        public DateTime? DataDeCriacao { get; set; }
    }
}