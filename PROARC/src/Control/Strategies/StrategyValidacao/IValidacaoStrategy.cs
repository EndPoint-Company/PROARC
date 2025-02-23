using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Control.Strategies.StrategyValidacao
{
    internal interface IValidacaoStrategy
    {
        bool Validar(string valor);
    }
}
