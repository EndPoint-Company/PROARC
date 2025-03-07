﻿using Microsoft.UI.Xaml.Data;
using System;

namespace PROARC.src.Converters
{
    public class NullToNAConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Se for nulo ou uma string vazia, retorna "N/A"
            if (value == null || (value is string str && string.IsNullOrEmpty(str)))
            {
                return "N/A";
            }

            // Converte o valor para string
            string stringValue = value.ToString();

            // Se for uma string com mais de 15 caracteres, corta a string e adiciona "..."
            if (stringValue.Length > 15)
            {
                return stringValue.Substring(0, 15) + "...";
            }

            return stringValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;  // Não precisamos de conversão reversa
        }
    }
}