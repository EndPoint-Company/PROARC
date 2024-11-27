using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src
{
public class Reclamado()
{
    private enum tipoDeReclamado
    {
        PessoaFisica,
        PessoaJuridica
    }

        private  string nome;
        private  SHA512? cpf;
        private  SHA512? cnpj;

        public required string Nome { get; set; }
        public required SHA512 Cpf { get; set; }
        public required SHA512 Cnpj { get; set; }

    }
}
