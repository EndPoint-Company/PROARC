using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Control.Strategies.StrategyValidacao
{
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
}
