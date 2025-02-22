using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models.Arquivos.StrategyExibicao
{
    public class ExibicaoDetalhadaStrategy : IExibicaoStrategy
    {
        public string Format(Reclamacao reclamacao)
        {
            return $"Título: {reclamacao.Titulo}\n" +
                   $"Reclamante - {reclamacao.Reclamante}\n" +
                   $"Situação: {reclamacao.Situacao}\n" +
                   $"Criador: {reclamacao.Criador}\n" +
                   $"Data de Abertura: {reclamacao.DataAbertura?.ToString() ?? "N/A"}\n";
        }
    }
}
