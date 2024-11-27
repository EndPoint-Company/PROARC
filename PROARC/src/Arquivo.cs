using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src
{
    public class Arquivo(string caminhoDoArquivo, string tipo)
    {
        private string caminhoDoArquivo;
        private string tipo;
        private DateTime? dataDeCriacao;
        private DateTime? dataDeModificacao;
        private long tamanhoEmBytes;

        public required string CaminhoDoArquivo { get; set; }
        public required string Tipo { get; set; }
        public DateTime? DataDeCriacao { get; set; }
        public DateTime? DataDeModificacao { get; set; }
    }
}
