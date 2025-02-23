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
    public class HighlightFieldStrategy : IFieldStrategy
    {
        public void Apply(FrameworkElement field, TextBlock labelBlock, TextBlock? titleBlock = null)
        {
            switch (field)
            {
                case TextBox textBox when string.IsNullOrWhiteSpace(textBox.Text):
                    textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                    textBox.PlaceholderForeground = new SolidColorBrush(Colors.Red);
                    labelBlock.Foreground = new SolidColorBrush(Colors.Red);
                    titleBlock?.SetValue(TextBlock.ForegroundProperty, new SolidColorBrush(Colors.Red));
                    break;

                case ComboBox comboBox when comboBox.SelectedItem == null:
                    comboBox.BorderBrush = new SolidColorBrush(Colors.Red);
                    labelBlock.Foreground = new SolidColorBrush(Colors.Red);
                    break;

                case RadioButton radioButton when radioButton.IsChecked == false:
                    radioButton.Foreground = new SolidColorBrush(Colors.Red);
                    labelBlock.Foreground = new SolidColorBrush(Colors.Red);
                    break;
            }
        }
    }
}
