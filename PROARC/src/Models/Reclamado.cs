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

        public string Nome { get => this.nome; set { this.nome = value; } }
        public short NumeroDaRua { get => this.numeroDaRua; set { this.numeroDaRua = value; } }
        public string Rua { get => this.rua; set { this.rua = value; } }
        public string Bairro { get => this.bairro; set { this.bairro = value; } }
        public string Cidade { get => this.cidade; set { this.cidade = value; } }
        public string Estado { get => this.estado; set { this.estado = value; } }
    }
}
