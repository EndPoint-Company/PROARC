using PROARC.src.Models.Tipos;
using PROARC.src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Models.Arquivos
{
    public class Reclamacao
    {
        private Motivo? motivo;
        private Reclamante? reclamante;
        private Procurador? procurador;
        private List<Reclamado>? reclamados;
        private string titulo;
        private string situacao;
        private string caminhoDir;
        private DateOnly? dataAbertura;
        private string criador;


        public Reclamacao
            (Motivo? motivo, Reclamante? reclamante, Procurador? procurador, List<Reclamado>? reclamados, string titulo, string situacao, string caminhoDir, DateOnly? dataAbertura, string criador)
        {
            this.motivo = motivo;
            this.reclamante = reclamante;
            this.procurador = procurador;
            this.reclamados = reclamados;
            this.titulo = titulo;
            this.situacao = situacao;
            this.caminhoDir = caminhoDir;
            this.dataAbertura = dataAbertura;
            this.criador = criador;
        }
        public Motivo? Motivo { get => this.motivo; set { this.motivo = value; } }
        public Reclamante? Reclamante { get => this.reclamante; set { this.reclamante = value; } }
        public Procurador? Procurador { get => this.procurador; set { this.procurador = value; } }
        public List<Reclamado>? Reclamados { get => this.reclamados; set { this.reclamados = value; } }
        public string Titulo { get => this.titulo; set { this.titulo = value; } }
        public string Situacao { get => this.situacao; set { this.situacao = value; } }
        public string CaminhoDir { get => this.caminhoDir; set { this.caminhoDir = value; } }
        public DateOnly? DataAbertura { get => this.dataAbertura; set { this.dataAbertura = value; } }
        public string Criador { get => this.criador; set { this.criador = value; } }

    }

}
