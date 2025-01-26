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
        private Status status;
        private Motivo? motivo;
        private Reclamado? reclamado;
        private Reclamante? reclamante;
        private DateTime? dataDaAudiencia;
        private DateTime? dataDeModificacao;
        private DateTime? dataDeCriacao;
        private Dictionary<ArquivoTipo, Diretorio>? diretorios;

        public ProcessoAdministrativo() { }

        public ProcessoAdministrativo(
            string caminhoDoProcesso, string titulo, short ano, Motivo? motivo = null,
            Reclamado? reclamado = null, Reclamante? reclamante = null, DateTime? dataDaAudiencia = null,
            Status status = Status.EmTramitacaoAguardandoEnvioDaNotificacao, DateTime? dataDeModificacao = null,
            DateTime? dataDeCriacao = null)
        {
            this.titulo = titulo;
            this.caminhoDoProcesso = caminhoDoProcesso;
            this.ano = ano;
            this.motivo = motivo;
            this.reclamante = reclamante;
            this.reclamado = reclamado;
            this.dataDaAudiencia = dataDaAudiencia;
            this.diretorios = new Dictionary<ArquivoTipo, Diretorio>();
            this.status = status;
            this.dataDeModificacao = dataDeModificacao;
            this.dataDeCriacao = dataDeCriacao;
        }

        private void AdicionarDiretorio(Arquivo arquivo, string caminhoDoDiretorio)
        {
            if (this.diretorios.ContainsKey(arquivo.Tipo)) return;

            this.diretorios.Add(arquivo.Tipo, new Diretorio(caminhoDoDiretorio, arquivo.Tipo));
        }

        public void AdicionarArquivo(Arquivo arquivo, string caminhoDoDiretorio)
        {
            AdicionarDiretorio(arquivo, caminhoDoDiretorio);

            this.diretorios[arquivo.Tipo].AdicionarArquivo(arquivo);
        }

        public void RemoverDiretorio(ArquivoTipo chave)
        {
            if (chave != ArquivoTipo.OutrosAnexos)
            {
                RemoverArquivo(this.diretorios[chave].getLastArquivo());
            }

            this.diretorios.Remove(chave);
        }

        public void RemoverArquivo(Arquivo arquivo)
        {
            this.diretorios[arquivo.Tipo].RemoverArquivo(arquivo);
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
                   $"Data da Audiência: {dataDaAudiencia?.ToString("dd/MM/yyyy") ?? "Não definida"}\n" +
                   $"Data de Modificação: {dataDeModificacao?.ToString("dd/MM/yyyy HH:mm:ss") ?? "Não definida"}\n" +
                   $"Data de Criação: {dataDeCriacao?.ToString("dd/MM/yyyy HH:mm:ss") ?? "Não definida"}\n" +
                   $"Diretórios:\n" +
                   $"{(diretorios != null ? string.Join("\n", diretorios.Select(d => $"- {d.Key}: {d.Value}")) : "Nenhum")}";
        }


        public string Titulo { get => this.titulo; set { this.titulo = value; } }
        public short Ano { get => this.ano; set { this.ano = value; } }
        public Motivo? Motivo { get => this.motivo; set { this.motivo = value; } }
        public Reclamado? Reclamado { get => this.reclamado; set { this.reclamado = value; } }
        public Reclamante? Reclamante { get => this.reclamante; set { this.reclamante = value; } }
        public string CaminhoDoProcesso { get => this.caminhoDoProcesso; set { this.caminhoDoProcesso = value; } }
        public DateTime? DataDaAudiencia { get => this.dataDaAudiencia; set { this.dataDaAudiencia = value; } }
        public Status Status { get => this.status; set { this.status = value; } }
        public DateTime? DataDeCriacao { get => this.dataDeCriacao; set { this.dataDeCriacao = value; } }
        public DateTime? DataDeModificacao { get => this.dataDeModificacao; set { this.dataDeModificacao = value; } }
        public ObservableCollection<Diretorio> GetObservableDiretorios()
        {
            return new ObservableCollection<Diretorio>(this.diretorios.Values);
        }
    }
}
