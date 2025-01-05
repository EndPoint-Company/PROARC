using System;

namespace PROARC.src.Models.Tipos
{
    [Flags]
    public enum Status
    {
        EmTramitacaoAguardandoEnvioDaNotificacao = 1,
        EmTramitacaoAguardandoRespostaDaEmpresa = 2,
        EmTramitacaoAguardandoRealizacaoDaAudiencia = 4,
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
