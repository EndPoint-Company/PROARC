using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models
{
    public class ProcessoAdministrativo
    {
        private string numeroProcesso;
        private int ano;
        private Motivo? motivo;
        private Reclamado? reclamado;
        private Reclamante? reclamante;
        private Dictionary<ArquivoTipo, Diretorio> diretorios;

        public ProcessoAdministrativo(string numeroProcesso, int ano, Motivo? motivo = null, Reclamado? reclamado = null, Reclamante? reclamante = null)
        {
            this.numeroProcesso = numeroProcesso;
            this.ano = ano;
            this.motivo = motivo;
            this.reclamante = reclamante;
            this.reclamado = reclamado;
            this.diretorios = new Dictionary<ArquivoTipo, Diretorio>();
        }

        private void AdicionarDiretorio(Arquivo arquivo, string caminhoDoDiretorio)
        {
            if (diretorios.ContainsKey(arquivo.Tipo)) return;

            diretorios.Add(arquivo.Tipo, new Diretorio(caminhoDoDiretorio, arquivo.Tipo));
        }

        public void AdicionarArquivo(Arquivo arquivo, string caminhoDoDiretorio)
        {
            AdicionarDiretorio(arquivo, caminhoDoDiretorio);

            // TODO
        }

        private void RemoverDiretorio() // TODO
        {
        
        }

        public void RemoverArquivo() // TODO
        {

        }

        public required string NumeroProcesso { get; set;  }
        public required int Ano { get; set; }
        public Motivo? Motivo { get; set; }
        public Reclamado? Reclamado { get; set; }
        public Reclamante? Reclamante { get; set; }
    }
}
