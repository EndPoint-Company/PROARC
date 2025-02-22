using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models.Arquivos.StrategyExibicao
{
    public interface IExibicaoStrategy
    {
        string Format(Reclamacao reclamacao);
    }
}
