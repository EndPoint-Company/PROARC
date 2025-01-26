using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PROARC.src.Models.Tipos;

namespace PROARC.src.Models.Arquivos
{
    public class ProcessoAdministrativo
    {
        private string? titulo;
        private string? caminhoDoProcesso;
        private short ano;
        private string status;
        private Motivo? motivo;
        private Reclamado? reclamado;
        private Reclamante? reclamante;
        private DateTime? dataDaAudiencia;
        private DateTime? dataDeCriacao;

        public ProcessoAdministrativo() { }

        public ProcessoAdministrativo(
            string caminhoDoProcesso, string titulo, short ano, Motivo? motivo = null,
            Reclamado? reclamado = null, Reclamante? reclamante = null, DateTime? dataDaAudiencia = null,
            string status = "Em tramitação", DateTime? dataDeModificacao = null,
            DateTime? dataDeCriacao = null)
        {
            this.titulo = titulo;
            this.caminhoDoProcesso = caminhoDoProcesso;
            this.ano = ano;
            this.motivo = motivo;
            this.reclamante = reclamante;
            this.reclamado = reclamado;
            this.dataDaAudiencia = dataDaAudiencia;
            this.status = status;
            this.dataDeCriacao = dataDeCriacao;
        }

        public override string ToString()
        {
            return $"Processo Administrativo:\n" +
                   $"Título: {titulo ?? "Não definido"}\n" +
                   $"Caminho do Processo: {caminhoDoProcesso ?? "Não definido"}\n" +
                   $"Ano: {ano}\n" +
                   $"Status: {status}\n" +
                   $"Motivo: {motivo?.ToString() ?? "Não definido"}\n" +
                   $"Reclamado: {reclamado?.ToString() ?? "Não definido"}\n" +
                   $"Reclamante: {reclamante?.ToString() ?? "Não definido"}\n" +
                   $"Data da Audiência: {dataDaAudiencia?.ToString("dd/MM/yyyy") ?? "Não definida"}\n";
                 
        }

        public string Titulo { get => this.titulo; set { this.titulo = value; } }
        public short Ano { get => this.ano; set { this.ano = value; } }
        public Motivo? Motivo { get => this.motivo; set { this.motivo = value; } }
        public Reclamado? Reclamado { get => this.reclamado; set { this.reclamado = value; } }
        public Reclamante? Reclamante { get => this.reclamante; set { this.reclamante = value; } }
        public string CaminhoDoProcesso { get => this.caminhoDoProcesso; set { this.caminhoDoProcesso = value; } }
        public DateTime? DataDaAudiencia { get => this.dataDaAudiencia; set { this.dataDaAudiencia = value; } }
        public string Status { get => this.status; set { this.status = value; } }
        public DateTime? DataDeCriacao { get => this.dataDeCriacao; set { this.dataDeCriacao = value; } }
    }
}
