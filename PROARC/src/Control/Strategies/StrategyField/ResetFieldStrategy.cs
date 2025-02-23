using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Microsoft.UI;

namespace PROARC.src.Control.Strategies.StrategyField
{
    public class ResetFieldStrategy : IFieldStrategy
    {
        public void Apply(FrameworkElement field, TextBlock labelBlock, TextBlock? titleBlock = null)
        {
            switch (field)
            {
                case TextBox textBox:
                    textBox.BorderBrush = new SolidColorBrush(Colors.Gray);
                    textBox.PlaceholderForeground = new SolidColorBrush(Colors.DarkGray);
                    labelBlock.Foreground = new SolidColorBrush(Colors.Black);
                    titleBlock?.SetValue(TextBlock.ForegroundProperty, new SolidColorBrush(Colors.Black));
                    break;

                case ComboBox comboBox:
                    comboBox.BorderBrush = new SolidColorBrush(Colors.Gray);
                    labelBlock.Foreground = new SolidColorBrush(Colors.Black);
                    break;

                case RadioButton radioButton:
                    if (radioButton.IsChecked == false)
                    {
                        radioButton.Foreground = new SolidColorBrush(Colors.Red);
                        labelBlock.Foreground = new SolidColorBrush(Colors.Red);
                        titleBlock?.SetValue(TextBlock.ForegroundProperty, new SolidColorBrush(Colors.Red));
                    }
                    break;
            }
        }
    }
}
