using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models.Arquivos.StrategyExibicao
{
    public class ExibicaoSimplesStrategy : IExibicaoStrategy
    {
        public string Format(Reclamacao reclamacao)
        {
            return $"Título: {reclamacao.Titulo}, Situação: {reclamacao.Situacao}";
        }
    }
}
