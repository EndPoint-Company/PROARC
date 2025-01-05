using PROARC.src.Models.Tipos;

namespace PROARC.src.Models.Arquivos
{
    public class Arquivo
    {
        private string caminhoDoArquivo;
        private ArquivoTipo tipo;
        private long tamanhoEmBytes;

        public Arquivo(string caminhoDoArquivo, ArquivoTipo tipo, long tamanhoEmBytes)
        {
            this.caminhoDoArquivo = caminhoDoArquivo;
            this.tipo = tipo;
            this.tamanhoEmBytes = tamanhoEmBytes;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            var other = obj as Arquivo;
            if (other == null) return false;

            return other.caminhoDoArquivo == this.caminhoDoArquivo;
        }

        public override int GetHashCode()
        {
            return this.caminhoDoArquivo.GetHashCode();
        }

        public string CaminhoDoArquivo
        {
            get => this.caminhoDoArquivo;

            set
            {
                this.caminhoDoArquivo = value;
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
    }
}
