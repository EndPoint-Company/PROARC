namespace PROARC.src.Models
{
    public class Reclamante
    {
        private string nome;
        private string cpf;
        private string? rg;
        private string? email;
        private string? telefone;

        // Propriedade formatada
        public string CpfFormatado
        {
            get
            {
                if (!string.IsNullOrEmpty(Cpf) && Cpf.Length == 11)
                {
                    return $"{Cpf.Substring(0, 3)}.{Cpf.Substring(3, 3)}.{Cpf.Substring(6, 3)}-{Cpf.Substring(9, 2)}";
                }
                return Cpf ?? "N/A"; // Retorna como está, se o formato não for válido
            }
        }

        public Reclamante(string nome, string cpf, string? rg = null, string telefone = null, string email = null)
        {
            this.nome = nome;
            this.cpf = cpf;
            this.rg = rg;
            this.telefone = telefone;
            this.email = email;
        }

        public override string ToString()
        {
            return $"Nome: {Nome}, rg: {Rg}, cpf: {Cpf}, telefone: {Telefone}, email: {Email}";
        }

        public string Nome { get => this.nome; set { this.nome = value; } }
        public string Cpf { get => this.cpf; set { this.cpf = value; } }
        public string? Rg { get => this.rg; set { this.rg = value; } }

        public string? Email { get => this.email; set { this.email = value; } }
        public string? Telefone { get => this.telefone; set { this.telefone = value; } }
    }
}
