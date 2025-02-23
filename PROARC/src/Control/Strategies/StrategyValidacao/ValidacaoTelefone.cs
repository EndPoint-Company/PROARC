using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Control.Strategies.StrategyValidacao
{
    internal class ValidacaoTelefone : IValidacaoStrategy
    {
        public bool Validar(string telefone)
        {
            if (string.IsNullOrEmpty(telefone)) return false;
            telefone = new string(telefone.Where(char.IsDigit).ToArray());
            return telefone.Length is 10 or 11;
        }
    }
}
