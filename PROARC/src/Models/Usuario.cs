using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models
{
    public class Usuario
    {
        private string nome;
        private int nivelDePermissao; // 0 1 2 3 4

        public Usuario(string nome, int nivelDePermissao)
        {
            this.nome = nome;

            if (nivelDePermissao < 0 || nivelDePermissao > 4)
            {
                throw new Exception("Não temos esse nível aí não, amegon");
            }

            this.nivelDePermissao = nivelDePermissao;
        }
        public void Promover()
        {
            if (this.nivelDePermissao > 3)
            {
                throw new Exception("Tá voando alto demais");
            }

            this.nivelDePermissao++;
        }

        public void Rebaixar()
        {
            if (NivelDePermissao <= 0)
            {
                throw new Exception("Quer ficar num nível negativo, é, meu fi?");
            }

            this.nivelDePermissao--;
        }

        public override string ToString()
        {
            return $"Nome: {Nome}, Nível de Permissão: {NivelDePermissao}";
        }

        public string Nome { get; set; }
        public int NivelDePermissao { get; }
    }
}

