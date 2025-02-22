using Microsoft.WindowsAppSDK.Runtime.Packages;
using PROARC.src.Models.FabricaEntidadesProcuradoras;
using PROARC.src.Models.Tipos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models.Arquivos.FabricaReclamacao
{
    public class FabricaReclamacao : IFabricaReclamacao
    {
        public Reclamacao CriarReclamacao(EnumReclamacao tipo,
        Motivo? motivo,
        Reclamante? reclamante,
        Procurador? procurador,
        LinkedList<Reclamado>? reclamados,
        string titulo,
        string situacao,
        string caminhoDir,
        DateOnly? dataAbertura,
        string criador,
        string? atendente = null,
        string? contatoEnelTelefone = null,
        string? contatoEnelEmail = null,
        string? observacao = null,
        DateTime? dataAudiencia = null,
        string? conciliador = null)
        {
            switch (tipo)
            {
                case EnumReclamacao.ReclamacaoEnel:
                    return new ReclamacaoEnel(motivo, reclamante, procurador, reclamados, titulo, situacao, caminhoDir, dataAbertura, criador,
                    atendente, contatoEnelTelefone, contatoEnelEmail, observacao);

                case EnumReclamacao.ReclamacaoGeral:
                   return new ReclamacaoGeral(motivo, reclamante, procurador, reclamados, titulo, situacao, caminhoDir, dataAbertura, criador,
                    dataAudiencia, conciliador);

                default:
                    throw new ArgumentException("Tipo de reclamacao inválido.");
            }
        }
    }
}
