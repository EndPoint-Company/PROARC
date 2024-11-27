using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src
{
    class Usuario(string nome, int nivelPermissao, SHA512 pwd)
    {
        private string nome;
        private int nivelPermissao;
        private SHA512 pwd;


        public required string Nome { get; set; }
        public required int NivelPermissao { get; set; }
        public required SHA512 Pwd { get; set; }
    }
}

