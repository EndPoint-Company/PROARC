using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Converters
{
    internal class ProcessosListaViewModel
    {
        public List<string> ListaSituacoes { get; } = new List<string>
    {
        "Em Tramitação",
        "Concluído",
        "Cancelado",
        "Arquivado"
    };

        private string _situacao;
        public string Situacao
        {
            get => _situacao;
            set
            {
                _situacao = value;
                OnPropertyChanged(nameof(Situacao));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
