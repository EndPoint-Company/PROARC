using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace PROARC.src.Models
{
    class Reclamante
    {
        private string nome;
        private string? cpf;
        private string? rg;

        public required string Nome { get; set; }
        public required string Cpf { get; set; }
        public required string Rg { get; set; }
    }
}
