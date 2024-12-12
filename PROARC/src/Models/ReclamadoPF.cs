using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models
{
    public class ReclamadoPF : Reclamado
    {
        private string cpf;

        public ReclamadoPF
            (string nome, short numeroDaRua, string rua, string bairro, string cidade, string estado, string cpf) : base(nome, numeroDaRua, rua, bairro, cidade, estado)
        {
            this.cpf = cpf;
        }

        public string Cpf { get => this.cpf; set { this.cpf = value; } }
    }
}
