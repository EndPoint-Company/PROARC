using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models
{
    public class ReclamadoPJ : Reclamado
    {
        private string cnpj;

        public ReclamadoPJ
            (string nome, short numeroDaRua, string rua, string bairro, string cidade, string estado, string cnpj) : base(nome, numeroDaRua, rua, bairro, cidade, estado)
        {
            this.cnpj = cnpj;
        }

        public string Cnpj { get => this.cnpj; set { this.cnpj = value; } }
    }
}
