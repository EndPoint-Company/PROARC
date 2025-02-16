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
        private string? cpf;
        private string? cnpj;
        private short? numero;
        private string? logradouro;
        private string? bairro;
        private string? cidade;
        private string? uf;
        private string? cep;
        private string? telefone;
        private string? email;
        private object numeroAddr;
        private object logradouroAddr;
        private object bairroAddr;
        private object cidadeAddr;
        private object ufAddr;
        private DateTime dateTime;

        public Reclamado(string nome, string cpf = null, string cnpj = null,
                         short? numero = null, string logradouro = null, string bairro = null, string cidade = null,
                         string uf = null, string cep = null, string telefone = null, string email = null)
        {
            this.nome = nome;            
            this.cpf = cpf;
            this.cnpj = cnpj;
            this.numero = numero;
            this.logradouro = logradouro;
            this.bairro = bairro;
            this.cidade = cidade;
            this.uf = uf;
            this.telefone = telefone;
            this.email = email;
            this.cep = cep;
        }
        

        public override string ToString()
        {
            return $"Nome: {this.nome}\n" + 
                   $"Logradouro: {this.logradouro}\n" +
                   $"Número: {this.numero}\n" +
                   $"Bairro: {this.bairro}\n" +
                   $"Cidade: {this.cidade}\n" +
                   $"Estado: {this.uf}\n" +
                   $"CEP: {this.cep}\n" +
                   $"CNPJ: {this.cnpj}\n" +
                   $"CPF: {this.cpf}\n" +
                   $"Telefone: {this.telefone}\n" +
                   $"Email: {this.email}";
        }

        public string Nome { get => nome; set => nome = value; }
     
        public string? Logradouro { get => logradouro; set => logradouro = value; }
        public short? Numero { get => numero; set => numero = value; }
        public string? Bairro { get => bairro; set => bairro = value; }
        public string? Cidade { get => cidade; set => cidade = value; }
        public string? Uf { get => uf; set => uf = value; }
        public string? Cnpj { get => cnpj; set => cnpj = value; }
        public string? Cpf { get => cpf; set => cpf = value; }
        public string? Telefone { get => telefone; set => telefone = value; }
        public string? Email { get => email; set => email = value; }
        public string? Cep
        {
            get => cep; set => cep = value;
        }
    }
}
