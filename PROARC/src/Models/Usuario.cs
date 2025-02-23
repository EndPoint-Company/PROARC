using System;

namespace PROARC.src.Models
{
    public class Usuario(string? nome = null, string? cargo = null)
    {
        public override string ToString()
        {
            return $"Nome: {Nome}, Cargo: {Cargo}";
        }

        public string? Nome { get => nome; set { nome = value; } }
        public string? Cargo { get => cargo; set { cargo = value; } }
    }
}
