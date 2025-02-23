using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Control.Strategies.StrategyValidacao
{
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
}
