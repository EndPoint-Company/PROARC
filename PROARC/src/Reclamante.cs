using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace PROARC.src
{
    class Reclamante()
    {
        private string nome;
        private SHA512 cpf;
        private SHA512 rg;

        public required string Nome { get; set; }
        public required SHA512 Cpf { get; set; }
        public required SHA512 Rg { get; set; }
    }
}
