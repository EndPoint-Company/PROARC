using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models
{
    public class Procurador
    {
        private string nome;
        private string cpf;
        private string? rg;
        private string? email;
        private string? telefone;

        public string Nome { get => this.nome; set { this.nome = value; } }
        public string Cpf { get => this.cpf; set { this.cpf = value; } }
        public string? Rg { get => this.rg; set { this.rg = value; } }
        public string? Email { get => this.email; set { this.email = value; } }
        public string? Telefone { get => this.telefone; set { this.telefone = value; } }

    }
}
