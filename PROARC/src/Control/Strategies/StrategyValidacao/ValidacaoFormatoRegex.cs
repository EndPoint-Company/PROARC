using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PROARC.src.Control.Strategies.StrategyValidacao
{
    internal class ValidacaoFormatoRegex : IValidacaoStrategy
    {
        private readonly Regex _regex;

        public ValidacaoFormatoRegex(string pattern) => _regex = new Regex(pattern);

        public bool Validar(string valor) => _regex.IsMatch(valor);
    }
}
