using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Control.Strategies.StrategyValidacao
{
    internal class Validador
    {
        private readonly IValidacaoStrategy _validacao;

        public Validador(IValidacaoStrategy validacao) => _validacao = validacao;

        public bool Validar(string valor) => _validacao.Validar(valor);
    }
}
