using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PROARC.src.Models.Tipos;

namespace PROARC.src.Models.Arquivos
{
    public class Diretorio
    {
        private string caminhoDoDiretorio;
        private ArquivoTipo tipo;
        private DateTime dataDeCriacao;
        private DateTime dataDeModificacao;
        private LinkedList<Arquivo> arquivos;

        public Diretorio(string caminhoDoDiretorio, ArquivoTipo tipo)
        {
            this.caminhoDoDiretorio = caminhoDoDiretorio;
            this.tipo = tipo;
            this.dataDeCriacao = DateTime.Now;
            this.dataDeModificacao = this.dataDeCriacao;
            arquivos = new LinkedList<Arquivo>();
        }

        public void AdicionarArquivo(Arquivo arquivo)
        {
            this.arquivos.AddLast(arquivo);
        }

        public void RemoverArquivo(Arquivo arquivo)
        {
            this.arquivos.Remove(arquivo);
        }

        private void AtualizarDataDeModificacao()
        {
            this.dataDeModificacao = DateTime.Now;
        }

        public Arquivo getLastArquivo()
        {
            return this.arquivos.Last();
        }

        public string CaminhoDoArquivo
        {
            get => this.caminhoDoDiretorio;

            set
            {
                this.caminhoDoDiretorio = value;
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
