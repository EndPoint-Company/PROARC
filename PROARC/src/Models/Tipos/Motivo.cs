using System;

namespace PROARC.src.Models.Tipos
{
    public class Motivo
    {
        private string nome;
        private DateTime? dataDeCriacao;

        public Motivo(string motivoNome, string? descricao = null)
        {
            this.nome = motivoNome;
            this.dataDeCriacao = DateTime.Now;
        }

        public override string ToString()
        {
            return nome;
        }

        public string Nome { get => this.nome; set { this.nome = value; } }
        public DateTime? DataDeCriacao { get => this.dataDeCriacao; }
    }
}
