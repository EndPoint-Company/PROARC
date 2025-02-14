using System;
using System.Diagnostics;
using Microsoft.UI.Xaml.Data;

namespace PROARC.src.Converters
{
    using System;
    using System.Diagnostics;
    using Windows.UI.Xaml.Data;

    public class TextTruncationConverter : IValueConverter
    {
        public int MaxLength { get; set; } = 30; // Defina um valor padrão para MaxLength caso não seja definido.

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                // Verifica se o valor é uma string e se não é nulo ou vazio
                if (value is string text && !string.IsNullOrEmpty(text))
                {
                    // Se o texto for maior que o limite, faz o truncamento
                    if (text.Length > MaxLength)
                    {
                        return text.Substring(0, MaxLength) + "...";
                    }
                    return text;
                }

                // Se o valor for nulo ou vazio, retorna "N/A"
                if (value == null)
                {
                    return "N/A";
                }

                // Se o valor não for uma string válida, loga e retorna o valor original
                Debug.WriteLine($"Texto inválido ou nulo para truncamento: {value}");
                return value;
            }
            catch (Exception ex)
            {
                // Loga a exceção detalhada para facilitar a depuração
                Debug.WriteLine($"Erro no conversor de truncamento: {ex.Message}\n{ex.StackTrace}");
                return value; // Retorna o valor original em caso de erro
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // Não precisamos de conversão reversa, então apenas retorna o valor original
            return value;
        }
    }



}
