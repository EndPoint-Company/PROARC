using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Control.Strategies.StrategyValidacao
{
    internal class ValidacaoCEP : IValidacaoStrategy
    {
        public bool Validar(string cep)
        {
            if (string.IsNullOrEmpty(cep)) return false;
            cep = new string(cep.Where(char.IsDigit).ToArray());
            return cep.Length == 8;
        }
    }
}
