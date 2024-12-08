﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

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

        public string Nome { get; set; }
        public string? Cpf { get; set; }
        public string? Rg { get; set; }
    }
}
