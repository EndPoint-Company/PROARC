using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models.Tipos
{
    public enum StatusReclamacao
    {
        AguardandoFazerNotificacao,
        AguardandoRealizacaoAudiencia,
        AguardandoRespostaEmpresa,
        AguardandoEnvioNotificacao,
        AguardandoDocumentacao,
        Atendido,
        NaoAtendido
    }
}
