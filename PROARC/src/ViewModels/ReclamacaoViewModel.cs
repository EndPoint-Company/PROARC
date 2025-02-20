using PROARC.src.Converters;
using PROARC.src.Models.Arquivos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PROARC.src.ViewModels
{
    public class ReclamacaoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Reclamacao _reclamacao;

        public string Titulo { get; }
        public string Reclamante { get; }
        public string Cpf { get; }
        public string DataAbertura { get; }

        public List<string> StatusList { get; } = new()
        {
            "Aguardando fazer notificação",
            "Aguardando realização da audiência",
            "Aguardando resposta da empresa",
            "Aguardando envio da notificação",
            "Aguardando documentação",
            "Atendido",
            "Não Atendido"
        };

        private string status;
        public string Status
        {
            get => status;
            set
            {
                if (status != value)
                {
                    Debug.WriteLine($"[ViewModel] Status alterado de '{status}' para '{value}'");

                    status = value; // Apenas atualiza a variável local
                    OnPropertyChanged();
                }
            }
        }



        public event System.Action<ReclamacaoViewModel> StatusChanged;

        private void OnStatusChanged()
        {
            StatusChanged?.Invoke(this);
        }

        public string Criador { get; }
        public string DataCriacao { get; }
        public string CaminhoDir { get; }
        public string Motivo { get; }
        public string Procurador { get; }
        public string PrimeiroReclamadoNome { get; }
        public List<string> Reclamados { get; }

        public string DataAudiencia { get; }
        public string Conciliador { get; }
        public string Atendente { get; }
        public string ContatoEnelTelefone { get; }
        public string ContatoEnelEmail { get; }
        public string Observacao { get; }

        public ReclamacaoViewModel(Reclamacao reclamacao)
        {
            _reclamacao = reclamacao;
            Titulo = ReclamacaoConverter.ConverterTitulo(reclamacao);
            Reclamante = ReclamacaoConverter.ConverterReclamanteNome(reclamacao);
            Cpf = ReclamacaoConverter.ConverterCpf(reclamacao);
            DataAbertura = ReclamacaoConverter.ConverterDataAbertura(reclamacao);
            Status = ReclamacaoConverter.ConverterStatus(reclamacao);
            Criador = ReclamacaoConverter.ConverterCriador(reclamacao);
            DataCriacao = ReclamacaoConverter.ConverterDataCriacao(reclamacao);
            CaminhoDir = ReclamacaoConverter.ConverterCaminhoDir(reclamacao);
            Motivo = ReclamacaoConverter.ConverterMotivo(reclamacao);
            Procurador = ReclamacaoConverter.ConverterProcurador(reclamacao);
            PrimeiroReclamadoNome = ReclamacaoConverter.ConverterReclamado(reclamacao);
            Reclamados = ReclamacaoConverter.ConverterReclamados(reclamacao);

            DataAudiencia = ReclamacaoConverter.ConverterDataAudiencia(reclamacao);
            Conciliador = ReclamacaoConverter.ConverterConciliador(reclamacao);
            Atendente = ReclamacaoConverter.ConverterAtendente(reclamacao);
            ContatoEnelTelefone = ReclamacaoConverter.ConverterContatoEnelTelefone(reclamacao);
            ContatoEnelEmail = ReclamacaoConverter.ConverterContatoEnelEmail(reclamacao);
            Observacao = ReclamacaoConverter.ConverterObservacao(reclamacao);
        }

        public Reclamacao ObterModeloOriginal()
        {
            return _reclamacao;
        }
    }
}
