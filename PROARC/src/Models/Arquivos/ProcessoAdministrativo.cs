using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PROARC.src.Models.Tipos;

namespace PROARC.src.Models.Arquivos
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
            if (this.diretorios.ContainsKey(arquivo.Tipo)) return;

            this.diretorios.Add(arquivo.Tipo, new Diretorio(caminhoDoDiretorio, arquivo.Tipo));
        }

        public void AdicionarArquivo(Arquivo arquivo, string caminhoDoDiretorio)
        {
            AdicionarDiretorio(arquivo, caminhoDoDiretorio);

            this.diretorios[arquivo.Tipo].AdicionarArquivo(arquivo);
        }

        public void RemoverDiretorio(ArquivoTipo chave)
        {
            if (chave != ArquivoTipo.OutrosAnexos)
            {
                RemoverArquivo(this.diretorios[chave].getLastArquivo());
            }

            this.diretorios.Remove(chave);
        }

        public void RemoverArquivo(Arquivo arquivo)
        {
            this.diretorios[arquivo.Tipo].RemoverArquivo(arquivo);
        }

        public string NumeroProcesso { get => this.numeroProcesso; set { this.numeroProcesso = value; }  }
        public int Ano { get => this.ano; set { this.ano = value; } }
        public Motivo? Motivo { get => this.motivo; set { this.motivo = value; } }
        public Reclamado? Reclamado { get => this.reclamado; set { this.reclamado = value; } }
        public Reclamante? Reclamante { get => this.reclamante; set { this.reclamante = value; } }
    }
}
