using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models
{
    public class Reclamado
    {
        private string nome;
        private short? numeroDaRua;
        private string? rua;
        private string? email;
        private string? bairro;
        private string? cidade;
        private string? estado;
        private string? cnpj;
        private string? cpf;


        public Reclamado
            (string nome, short? numeroDaRua = null, string? rua = null, string? bairro = null, string? email = null, string? cidade = null, 
            string? estado = null, string? cnpj = null, string? cpf = null)
        {
            this.nome = nome;
            this.numeroDaRua = numeroDaRua;
            this.rua = rua;
            this.bairro = bairro;
            this.cidade = cidade;
            this.estado = estado;
            this.cnpj = cnpj;
            this.cpf = cpf;
            this.email = email;
        }

        public override string ToString()
        {
            return $"Nome: {this.nome}\n" +
           $"Número da Rua: {this.numeroDaRua}\n" +
           $"Rua: {this.rua}\n" +
           $"Bairro: {this.bairro}\n" +
           $"Cidade: {this.cidade}\n" +
           $"Estado: {this.estado}\n" +
           $"RG: {this.cnpj}\n" +
           $"CPF: {this.cpf}\n" +
           $"Email: {this.email}";
        }

        public string Nome { get => this.nome; set { this.nome = value; } }
        public short? NumeroDaRua { get => this.numeroDaRua; set { this.numeroDaRua = value; } }
        public string? Rua { get => this.rua; set { this.rua = value; } }
        public string? Bairro { get => this.bairro; set { this.bairro = value; } }
        public string? Cidade { get => this.cidade; set { this.cidade = value; } }
        public string? Estado { get => this.estado; set { this.estado = value; } }       
        public string? Cnpj { get => this.cnpj; set { this.cnpj = value; } }
        public string? Cpf { get => this.cpf; set { this.cpf = value; } }
        public string? Email { get => this.email; set { this.email = value; } }
    }
}
