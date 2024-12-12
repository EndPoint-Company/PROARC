using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PROARC.src.Models.Tipos;

namespace PROARC.src.Models.Arquivos
{
    public class Arquivo
    {
        private string caminhoDoArquivo;
        private ArquivoTipo tipo;
        private DateTime dataDeCriacao;
        private DateTime dataDeModificacao;
        private long tamanhoEmBytes;

        public Arquivo(string caminhoDoArquivo, ArquivoTipo tipo, long tamanhoEmBytes)
        {
            this.caminhoDoArquivo = caminhoDoArquivo;
            this.tipo = tipo;
            this.dataDeCriacao = DateTime.Now;
            this.dataDeModificacao = this.dataDeCriacao;
            this.tamanhoEmBytes = tamanhoEmBytes;
        }

        private void AtualizarDataDeModificacao()
        {
            this.dataDeModificacao = DateTime.Now;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var other = obj as Arquivo;
            if (other == null) return false;

            return other.caminhoDoArquivo == this.caminhoDoArquivo;
        }

        public string CaminhoDoArquivo 
        {
            get => this.caminhoDoArquivo;
                
            set
            {
                this.caminhoDoArquivo = value;
                AtualizarDataDeModificacao();
            }
        }

        public ArquivoTipo Tipo
        {
            get => this.tipo;

            set
            {
                this.tipo = value;
                AtualizarDataDeModificacao();
            }
        }

        public DateTime? DataDeCriacao { get => this.dataDeCriacao; }
        public DateTime? DataDeModificacao { get => this.dataDeModificacao; }
    }
}
