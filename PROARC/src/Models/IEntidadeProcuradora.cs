using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models
{
    public interface IEntidadeProcuradora
    {
        string Nome { get; set; }
        string Cpf { get; set; }
        string? Rg { get; set; }
        string? Email { get; set; }
        string? Telefone { get; set; }
    }
}
