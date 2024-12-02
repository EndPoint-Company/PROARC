using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models
{
    class Usuario
    {
        private string nome;
        private int nivelPermissao;

        public required string Nome { get; set; }
    }
}

