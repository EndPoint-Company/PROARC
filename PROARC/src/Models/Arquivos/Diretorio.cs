using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private LinkedList<Arquivo> arquivos;

        public Diretorio(string caminhoDoDiretorio, ArquivoTipo tipo)
        {
            this.caminhoDoDiretorio = caminhoDoDiretorio;
            this.tipo = tipo;
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
            }
        }

        public ArquivoTipo Tipo
        {
            get => this.tipo;

            set
            {
                this.tipo = value;
            }
        }

        public ObservableCollection<Arquivo> GetObservableArquivos()
        {
            return new ObservableCollection<Arquivo>(this.arquivos);
        }
    }
}
