using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PROARC.src.Converters
{
    internal class Validacoes
    {
        // Expressões regulares para validação em tempo real
        private static readonly Regex RegexCPF = new Regex(@"^\d{0,3}(\.?\d{0,3})?(\.?\d{0,3})?(-?\d{0,2})?$");
        private static readonly Regex RegexCNPJ = new Regex(@"^\d{0,2}(\.?\d{0,3})?(\.?\d{0,3})?(/?\d{0,4})?(-?\d{0,2})?$");
        private static readonly Regex RegexCEP = new Regex(@"^\d{0,5}-?\d{0,3}$");
        private static readonly Regex RegexEmail = new Regex(@"^[^@\s]*@?[^@\s]*\.?[^@\s]*$");
        private static readonly Regex RegexTelefone = new Regex(@"^\(?\d{0,2}\)?[\s-]?\d{0,5}-?\d{0,4}$");

        // Validação em tempo real (formato)
        public static bool ValidarFormatoCPF(string cpf) => RegexCPF.IsMatch(cpf);
        public static bool ValidarFormatoCNPJ(string cnpj) => RegexCNPJ.IsMatch(cnpj);
        public static bool ValidarFormatoCEP(string cep) => RegexCEP.IsMatch(cep);
        public static bool ValidarFormatoEmail(string email) => RegexEmail.IsMatch(email);
        public static bool ValidarFormatoTelefone(string telefone) => RegexTelefone.IsMatch(telefone);

        // Validação completa (conteúdo)
        public static bool ValidarCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return false;

            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11)
                return false;

            if (cpf.Distinct().Count() == 1)
                return false;

            int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicadores1[i];

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            tempCpf += digito1;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicadores2[i];

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return cpf.EndsWith($"{digito1}{digito2}");
        }

        public static bool ValidarCNPJ(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj))
                return false;

            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            if (cnpj.Length != 14)
                return false;

            if (cnpj.Distinct().Count() == 1)
                return false;

            int[] multiplicadores1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadores2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicadores1[i];

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            tempCnpj += digito1;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicadores2[i];

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return cnpj.EndsWith($"{digito1}{digito2}");
        }

        public static bool ValidarCEP(string cep)
        {
            if (string.IsNullOrEmpty(cep))
                return false;

            cep = new string(cep.Where(char.IsDigit).ToArray());
            return cep.Length == 8;
        }

        public static bool ValidarEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public static bool ValidarTelefone(string telefone)
        {
            if (string.IsNullOrEmpty(telefone))
                return false;

            telefone = new string(telefone.Where(char.IsDigit).ToArray());
            return telefone.Length == 10 || telefone.Length == 11;
        }
    }
}