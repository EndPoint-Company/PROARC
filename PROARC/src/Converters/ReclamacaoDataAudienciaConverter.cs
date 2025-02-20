using System;
using Windows.UI.Xaml.Data;
using PROARC.src.Models.Arquivos;
using System.Diagnostics;
using Microsoft.UI.Xaml.Data;

namespace PROARC.src.Converters
{
    public class ReclamacaoDataAudienciaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                Debug.WriteLine("❌ [Conversor] O valor recebido é NULL!");
                return "N/A";
            }

            Debug.WriteLine($"🔍 [Conversor] Tipo recebido: {value.GetType().Name}");

            if (value is ReclamacaoGeral reclamacaoGeral)
            {
                Debug.WriteLine($"✅ [Conversor] DataAudiencia: {reclamacaoGeral.DataAudiencia}");
                return reclamacaoGeral.DataAudiencia?.ToString("dd/MM/yyyy") ?? "N/A";
            }

            Debug.WriteLine("⚠ [Conversor] Não é um objeto do tipo ReclamacaoGeral!");
            return "N/A";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
