using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models
{
    public abstract class Reclamado
    {
        protected string nome;
        protected short numeroDaRua;
        protected string rua;
        protected string bairro;
        protected string cidade;
        protected string estado;

        public Reclamado
            (string nome, short numeroDaRua, string rua, string bairro, string cidade, string estado)
        {
            this.nome = nome;
            this.numeroDaRua = numeroDaRua;
            this.rua = rua;
            this.bairro = bairro;
            this.cidade = cidade;
            this.estado = estado;
        }

        public string Nome { get; set; }
        public short NumeroDaRua { get; set; }
        public string Rua { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
    }
}
