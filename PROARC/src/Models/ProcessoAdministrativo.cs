using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models
{
    class ProcessoAdministrativo
    {
        private string numeroProcesso;
        private int ano;
        private Motivo? motivo;
        private Reclamado? reclamado;
        private Reclamante? reclamante;
        private Dictionary<ArquivoTipo, Arquivo> arquivos;

        public ProcessoAdministrativo(string numeroProcesso, int ano, Motivo? motivo = null, Reclamado? reclamado = null, Reclamante? reclamante = null)
        {
            this.numeroProcesso = numeroProcesso;
            this.ano = ano;
            this.motivo = motivo;
            this.reclamante = reclamante;
            this.reclamado = reclamado;
            this.arquivos = new Dictionary<ArquivoTipo, Arquivo>();
        }

        public void adicionarArquivo(Arquivo arquivo)
        {
            this.arquivos.Add(arquivo.Tipo, arquivo);
        }

        public required string NumeroProcesso { get; set;  }
        public required int Ano { get; set; }
        public Motivo? Motivo { get; set; }
        public Reclamado? Reclamado { get; set; }
        public Reclamante? Reclamante { get; set; }
    }
}
