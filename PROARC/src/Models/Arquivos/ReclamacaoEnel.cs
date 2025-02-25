using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PROARC.src.Models.Tipos;

namespace PROARC.src.Models.Arquivos
{
    public class ReclamacaoEnel : Reclamacao
    {
        private string? atendente;
        private string? contatoEnelTelefone;
        private string? contatoEnelEmail;
        private string? observacao;

        public ReclamacaoEnel()
        {
        }

        public ReclamacaoEnel
            (Motivo? motivo, Reclamante? reclamante, Procurador? procurador, LinkedList<Reclamado>? reclamados, string titulo, string situacao, string caminhoDir, DateOnly? dataAbertura, string criador, string? atendente, string? contatoEnelTelefone, string? contatoEnelEmail, string? observacao)
            : base(motivo, reclamante, procurador, reclamados, titulo, situacao, caminhoDir, dataAbertura, criador)
        {
            this.atendente = atendente;
            this.contatoEnelTelefone = contatoEnelTelefone;
            this.contatoEnelEmail = contatoEnelEmail;
            this.observacao = observacao;
        }
        public override string ToString()
        {
            return $"Título: {Titulo}\n" +
                   $"Reclamante - {Reclamante}\n" +
                   $"Situação: {Situacao}\n" +
                   $"Criador: {Criador}\n" +
                   $"Data de Abertura: {DataAbertura?.ToString() ?? "N/A"}\n" +
                   $"Atendente: {Atendente ?? "N/A"}\n" +
                   $"Contato Enel - Telefone: {ContatoEnelTelefone ?? "N/A"}\n" +
                   $"Contato Enel - Email: {ContatoEnelEmail ?? "N/A"}\n" +
                   $"Observação: {Observacao ?? "N/A"}";
        }

        public string? Atendente { get => this.atendente; set { this.atendente = value; } }
        public string? ContatoEnelTelefone { get => this.contatoEnelTelefone; set { this.contatoEnelTelefone = value; } }
        public string? ContatoEnelEmail { get => this.contatoEnelEmail; set { this.contatoEnelEmail = value; } }
        public string? Observacao { get => this.observacao; set { this.observacao = value; } }

        public static explicit operator ReclamacaoEnel(Task<Reclamacao> v)
        {
            throw new NotImplementedException();
        }
    }
}
