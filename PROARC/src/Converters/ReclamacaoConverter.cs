using System;
using System.Collections.Generic;
using System.Linq;
using PROARC.src.Models.Arquivos;

namespace PROARC.src.Converters
{
    public static class ReclamacaoConverter
    {
        public static string ConverterTitulo(Reclamacao? reclamacao)
        {
            return string.IsNullOrEmpty(reclamacao?.Titulo) ? "N/A" : reclamacao.Titulo;
        }

        public static string ConverterReclamanteNome(Reclamacao? reclamacao)
        {
            return string.IsNullOrEmpty(reclamacao?.Reclamante?.Nome) ? "N/A" : reclamacao.Reclamante.Nome;
        }

        public static string ConverterCpf(Reclamacao? reclamacao)
        {
            return string.IsNullOrEmpty(reclamacao?.Reclamante?.CpfFormatado) ? "N/A" : reclamacao.Reclamante.CpfFormatado;
        }

        public static string ConverterDataAbertura(Reclamacao? reclamacao)
        {
            return reclamacao?.DataAbertura != null
                ? reclamacao.DataAbertura.Value.ToString("dd/MM/yyyy")
                : "N/A";
        }

        public static string ConverterStatus(Reclamacao? reclamacao)
        {
            return string.IsNullOrEmpty(reclamacao?.Situacao) ? "N/A" : reclamacao.Situacao;
        }

        public static string ConverterCriador(Reclamacao? reclamacao)
        {
            return string.IsNullOrEmpty(reclamacao?.Criador) ? "N/A" : reclamacao.Criador;
        }

        public static string ConverterDataCriacao(Reclamacao? reclamacao)
        {
            return reclamacao?.DataCriacao?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
        }

        public static string ConverterCaminhoDir(Reclamacao? reclamacao)
        {
            return string.IsNullOrEmpty(reclamacao?.CaminhoDir) ? "N/A" : reclamacao.CaminhoDir;
        }

        public static string ConverterMotivo(Reclamacao? reclamacao)
        {
            return string.IsNullOrEmpty(reclamacao?.Motivo?.Nome) ? "N/A" : reclamacao.Motivo.Nome;
        }

        public static string ConverterProcurador(Reclamacao? reclamacao)
        {
            return string.IsNullOrEmpty(reclamacao?.Procurador?.Nome) ? "N/A" : reclamacao.Procurador.Nome;
        }

        public static string ConverterReclamado(Reclamacao? reclamacao)
        {
            if (reclamacao?.Reclamados == null || reclamacao.Reclamados.Count == 0)
                return "N/A";

            return reclamacao.Reclamados.First?.Value?.Nome ?? "N/A";
        }

        public static List<string> ConverterReclamados(Reclamacao? reclamacao)
        {
            if (reclamacao?.Reclamados == null || reclamacao.Reclamados.Count == 0)
                return new List<string> { "N/A" };

            var listaNomes = new List<string>();
            foreach (var reclamado in reclamacao.Reclamados)
            {
                listaNomes.Add(reclamado.Nome ?? "N/A");
            }
            return listaNomes;
        }

        public static string ConverterDataAudiencia(Reclamacao? reclamacao)
        {
            if (reclamacao is ReclamacaoGeral reclamacaoGeral)
            {
                return reclamacaoGeral.DataAudiencia?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";
            }
            return "N/A";
        }

        public static string ConverterConciliador(Reclamacao? reclamacao)
        {
            if (reclamacao is ReclamacaoGeral reclamacaoGeral)
            {
                return string.IsNullOrEmpty(reclamacaoGeral.Conciliador) ? "N/A" : reclamacaoGeral.Conciliador;
            }
            return "N/A";
        }

        public static string ConverterAtendente(Reclamacao? reclamacao)
        {
            if (reclamacao is ReclamacaoEnel reclamacaoEnel)
            {
                return string.IsNullOrEmpty(reclamacaoEnel.Atendente) ? "N/A" : reclamacaoEnel.Atendente;
            }
            return "N/A";
        }

        public static string ConverterContatoEnelTelefone(Reclamacao? reclamacao)
        {
            if (reclamacao is ReclamacaoEnel reclamacaoEnel)
            {
                return string.IsNullOrEmpty(reclamacaoEnel.ContatoEnelTelefone) ? "N/A" : reclamacaoEnel.ContatoEnelTelefone;
            }
            return "N/A";
        }

        public static string ConverterContatoEnelEmail(Reclamacao? reclamacao)
        {
            if (reclamacao is ReclamacaoEnel reclamacaoEnel)
            {
                return string.IsNullOrEmpty(reclamacaoEnel.ContatoEnelEmail) ? "N/A" : reclamacaoEnel.ContatoEnelEmail;
            }
            return "N/A";
        }

        public static string ConverterObservacao(Reclamacao? reclamacao)
        {
            if (reclamacao is ReclamacaoEnel reclamacaoEnel)
            {
                return string.IsNullOrEmpty(reclamacaoEnel.Observacao) ? "N/A" : reclamacaoEnel.Observacao;
            }
            return "N/A";
        }

    }
}
