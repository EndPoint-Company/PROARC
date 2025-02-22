using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models.FabricaEntidadesProcuradoras
{
    public interface IFabricaEntidadeProcuradora
    {
        IEntidadeProcuradora CriarEntidadeProcuradora(EnumEntidadeProcuradora tipo, string nome, string cpf, string? rg = null, string? telefone = null, string? email = null);
    }
}
