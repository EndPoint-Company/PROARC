using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PROARC.src.Converters
{
    internal interface IValidacaoStrategy
    {
        bool Validar(string valor);
    }

    internal class ValidacaoFormatoRegex : IValidacaoStrategy
    {
        private readonly Regex _regex;

        public ValidacaoFormatoRegex(string pattern) => _regex = new Regex(pattern);

        public bool Validar(string valor) => _regex.IsMatch(valor);
    }

    internal class ValidacaoCPF : IValidacaoStrategy
    {
        public bool Validar(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return false;

            cpf = new string(cpf.Where(char.IsDigit).ToArray());
            if (cpf.Length != 11 || cpf.Distinct().Count() == 1) return false;

            int[] m1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] m2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf[..9];
            int soma = tempCpf.Select((t, i) => int.Parse(t.ToString()) * m1[i]).Sum();
            int resto = soma % 11;
            int dig1 = resto < 2 ? 0 : 11 - resto;

            tempCpf += dig1;
            soma = tempCpf.Select((t, i) => int.Parse(t.ToString()) * m2[i]).Sum();
            resto = soma % 11;
            int dig2 = resto < 2 ? 0 : 11 - resto;

            return cpf.EndsWith($"{dig1}{dig2}");
        }
    }

    internal class ValidacaoCNPJ : IValidacaoStrategy
    {
        public bool Validar(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj)) return false;

            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());
            if (cnpj.Length != 14 || cnpj.Distinct().Count() == 1) return false;

            int[] m1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] m2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj[..12];
            int soma = tempCnpj.Select((t, i) => int.Parse(t.ToString()) * m1[i]).Sum();
            int resto = soma % 11;
            int dig1 = resto < 2 ? 0 : 11 - resto;

            tempCnpj += dig1;
            soma = tempCnpj.Select((t, i) => int.Parse(t.ToString()) * m2[i]).Sum();
            resto = soma % 11;
            int dig2 = resto < 2 ? 0 : 11 - resto;

            return cnpj.EndsWith($"{dig1}{dig2}");
        }
    }

    internal class ValidacaoCEP : IValidacaoStrategy
    {
        public bool Validar(string cep)
        {
            if (string.IsNullOrEmpty(cep)) return false;
            cep = new string(cep.Where(char.IsDigit).ToArray());
            return cep.Length == 8;
        }
    }

    internal class ValidacaoEmail : IValidacaoStrategy
    {
        public bool Validar(string email) =>
            !string.IsNullOrEmpty(email) &&
            Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    internal class ValidacaoTelefone : IValidacaoStrategy
    {
        public bool Validar(string telefone)
        {
            if (string.IsNullOrEmpty(telefone)) return false;
            telefone = new string(telefone.Where(char.IsDigit).ToArray());
            return telefone.Length is 10 or 11;
        }
    }

    internal class Validador
    {
        private readonly IValidacaoStrategy _validacao;

        public Validador(IValidacaoStrategy validacao) => _validacao = validacao;

        public bool Validar(string valor) => _validacao.Validar(valor);
    }
}
