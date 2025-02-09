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
        private string? conciliador;

        public ReclamacaoGeral()
        {
        }

        public ReclamacaoGeral
            (Motivo? motivo, Reclamante? reclamante, Procurador? procurador, LinkedList<Reclamado>? reclamados, string titulo, string situacao, string caminhoDir, DateOnly? dataAbertura, string criador, DateTime? dataAudiencia, string? conciliador)
            : base(motivo, reclamante, procurador, reclamados, titulo, situacao, caminhoDir, dataAbertura, criador)
        {
            this.dataAudiencia = dataAudiencia;
            this.conciliador = conciliador;
        }

        public DateTime? DataAudiencia { get => this.dataAudiencia; set { this.dataAudiencia = value; } }
        public string? Conciliador { get => this.conciliador; set { this.conciliador = value; } }
    }
}
