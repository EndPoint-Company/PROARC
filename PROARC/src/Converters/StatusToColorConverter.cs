using System;
using System.Globalization;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using PROARC.src.Models;
using Microsoft.UI;
using PROARC.src.Models.Tipos;

namespace PROARC.src.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is StatusReclamacao status)
            {
                return status switch
                {
                    StatusReclamacao.AguardandoFazerNotificacao => new SolidColorBrush(Colors.Orange),
                    StatusReclamacao.AguardandoRealizacaoAudiencia => new SolidColorBrush(Colors.Blue),
                    StatusReclamacao.AguardandoRespostaEmpresa => new SolidColorBrush(Colors.Purple),
                    StatusReclamacao.AguardandoEnvioNotificacao => new SolidColorBrush(Colors.Green),
                    StatusReclamacao.AguardandoDocumentacao => new SolidColorBrush(Colors.Yellow),
                    StatusReclamacao.Atendido => new SolidColorBrush(Colors.LightGreen),
                    StatusReclamacao.NaoAtendido => new SolidColorBrush(Colors.Red),
                    _ => new SolidColorBrush(Colors.Gray)
                };
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
