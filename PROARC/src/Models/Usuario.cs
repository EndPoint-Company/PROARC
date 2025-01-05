using System;

namespace PROARC.src.Models
{
    public class Usuario
    {
        private string nome;
        private short nivelDePermissao; // 0 1 2 3 4

        public Usuario(string nome, short nivelDePermissao)
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

        public string Nome { get => this.nome; set { this.nome = value; } }
        public short NivelDePermissao { get => this.nivelDePermissao; }
    }
}
