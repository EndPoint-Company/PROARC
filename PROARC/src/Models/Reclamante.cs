namespace PROARC.src.Models
{
    public class Reclamante
    {
        private string nome;
        private string? cpf;
        private string? rg;

        public Reclamante(string nome, string? cpf = null, string? rg = null)
        {
            this.nome = nome;
            this.cpf = cpf;
            this.rg = rg;
        }

        public override string ToString()
        {
            return $"Nome: {Nome}, rg: {Rg}, cpf: {Cpf}";
        }

        public string Nome { get => this.nome; set { this.nome = value; } }
        public string? Cpf { get => this.cpf; set { this.cpf = value; } }
        public string? Rg { get => this.rg; set { this.rg = value; } }
    }
}
