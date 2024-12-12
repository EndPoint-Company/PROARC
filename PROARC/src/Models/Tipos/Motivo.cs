using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models.Tipos
{
    public class Motivo
    {
        private string motivoNome;
        private DateTime? dataDeCriacao;
        private string descricao;

        public Motivo(string motivoNome, string descricao)
        {
            this.motivoNome = motivoNome;
            this.descricao = descricao;
            this.dataDeCriacao = DateTime.Now;
        }

        public string MotivoNome { get => this.motivoNome; set { this.motivoNome = value; } }
        public DateTime? DataDeCriacao { get => this.dataDeCriacao; }

        public string Descricao { get => this.descricao; set { this.descricao = value; } }
    }
}