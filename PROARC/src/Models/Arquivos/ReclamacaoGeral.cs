using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROARC.src.Models.Tipos;

namespace PROARC.src.Models.Arquivos
{
    public class ReclamacaoGeral : Reclamacao
    {
        private DateTime? dataAudiencia;
        private string? conciliadora;

       public ReclamacaoGeral
            (Motivo? motivo, Reclamante? reclamante, Procurador? procurador, List<Reclamado>? reclamados, string titulo, string situacao, string caminhoDir, DateOnly? dataAbertura, string criador, DateTime? dataAudiencia, string? conciliadora)
            : base(motivo, reclamante, procurador, reclamados, titulo, situacao, caminhoDir, dataAbertura, criador)
        {
            this.dataAudiencia = dataAudiencia;
            this.conciliadora = conciliadora;
        }
        public DateTime? DataAudiencia { get => this.dataAudiencia; set { this.dataAudiencia = value; } }
        public string? Conciliadora { get => this.conciliadora; set { this.conciliadora = value; } }
    }
}
