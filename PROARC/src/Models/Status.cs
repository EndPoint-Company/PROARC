using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models
{
    [Flags]
    public enum Status
    {
        EmTramitacaoAguardandoRespostaDaEmpresa = 1,
        EmTramitacaoAguardandoRealizacaoDaAudiencia = 2,
        EmTramitacaoAguardandoEnvioDaNotificacao = 4,
        EmTramitacaoAguardandoDocumentacao = 8,
        ArquivadoNaoAtendido = 16,
        ArquivadoAtendido = 32,

        EmTramitacao = EmTramitacaoAguardandoRespostaDaEmpresa
            | EmTramitacaoAguardandoRealizacaoDaAudiencia
            | EmTramitacaoAguardandoEnvioDaNotificacao
            | EmTramitacaoAguardandoDocumentacao,

        Arquivado = ArquivadoNaoAtendido | ArquivadoAtendido
    }
}
