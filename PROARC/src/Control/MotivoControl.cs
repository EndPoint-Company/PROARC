using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PROARC.src.Models.Tipos;

namespace PROARC.src.Control
{
    public static class MotivoControl
    {
        // Gerencia os motivos na database, não tem muito segredo.
        public static Motivo? GetMotivo(int nome)
        {
            // Pega motivo da database pelo id unico

            return null;
        }

        public static void AddMotivo(Motivo motivo)
        {
            // Adiciona motivo na db
        }
    }
}
