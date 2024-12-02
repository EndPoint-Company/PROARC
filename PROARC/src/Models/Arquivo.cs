using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models
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
        }

        public required string CaminhoDoArquivo { get; set; }
        public required ArquivoTipo Tipo { get; set; }
        public DateTime? DataDeCriacao { get; }
        public DateTime? DataDeModificacao { get; set; }
    }
}
