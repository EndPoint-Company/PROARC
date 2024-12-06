using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models
{
    public class Usuario
    {
        private string nome;
        private int nivelDePermissao;

        public Usuario(string nome, int nivelDePermissao)
        {
            this.nome = nome;
            this.nivelDePermissao = nivelDePermissao;
        }

        public string Nome { get; set; }
        public int NivelDePermissao { get; }
    }
}

