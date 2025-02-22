using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models.FabricaEntidadesProcuradoras
{
    public class FabricaEntidadeProcuradora : IFabricaEntidadeProcuradora
    {
        public IEntidadeProcuradora CriarEntidadeProcuradora(EnumEntidadeProcuradora tipo, string nome, string cpf, string? rg = null, string? telefone = null, string? email = null)
        {
            switch (tipo)
            {
                case EnumEntidadeProcuradora.Reclamante:
                    return new Reclamante(nome, cpf, rg, telefone, email);

                case EnumEntidadeProcuradora.Procurador:
                    return new Procurador(nome, cpf, rg, telefone, email);

                default:
                    throw new ArgumentException("Tipo de pessoa inválido.");
            }
        }

        /*
            // Criando instâncias usando a fábrica
            IFabricaEntidadeProcuradora fabricaEntidadeProcuradora = new FabricaEntidadeProcuradora();
            Reclamante reclamante = fabricaEntidadeProcuradora.CriarEntidadeProcuradora("Reclamante", "João Silva", "12345678901", "1234567", "11999999999", "joao@email.com");
            Procurador procurador = fabricaEntidadeProcuradora.CriarEntidadeProcuradora("Procurador", "Ana Souza", "98765432100", "7654321", "1188887777", "ana@email.com", DateTime.Now);
            Console.WriteLine(reclamante);
            Console.WriteLine(procurador);
        */
    }
}
