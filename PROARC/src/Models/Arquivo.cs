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
        private DateTime? dataDeCriacao;
        private DateTime? dataDeModificacao;
        private long tamanhoEmBytes;

        public Arquivo(string caminhoDoArquivo, ArquivoTipo tipo, DateTime? dataDeCriacao, DateTime? dataDeModificacao, long tamanhoEmBytes)
        {
            this.caminhoDoArquivo = caminhoDoArquivo;
            this.tipo = tipo;
            this.dataDeCriacao = dataDeCriacao;
            this.dataDeModificacao = dataDeModificacao;
        }

        public string CaminhoDoArquivo { get; set; }
        public string Tipo { get; set; }
        public DateTime? DataDeCriacao { get; set; }
        public DateTime? DataDeModificacao { get; set; }
    }
}
