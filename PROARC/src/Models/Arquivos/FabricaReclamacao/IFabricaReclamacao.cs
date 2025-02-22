using PROARC.src.Models.Tipos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models.Arquivos.FabricaReclamacao
{
    public interface IFabricaReclamacao
    {
        object CriarReclamacao(EnumReclamacao tipo, Motivo? motivo, Reclamante? reclamante, Procurador? procurador, LinkedList<Reclamado>? reclamados, string titulo, string situacao, string caminhoDir, DateOnly? dataAbertura, string criador, DateTime? dataCriacao = null);
    }
}
