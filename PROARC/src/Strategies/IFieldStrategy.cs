using System;
using System.Collections.Generic;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace PROARC.src.Strategies
{
    public interface IFieldStrategy
    {
        void Apply(FrameworkElement field, TextBlock labelBlock, TextBlock? titleBlock = null);
    }

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

    public class FieldProcessor
    {
        private readonly IFieldStrategy _strategy;

        public FieldProcessor(IFieldStrategy strategy)
        {
            _strategy = strategy;
        }

        public void ProcessField(FrameworkElement field, TextBlock labelBlock, TextBlock? titleBlock = null)
        {
            _strategy.Apply(field, labelBlock, titleBlock);
        }

        public void ProcessMultipleFields(IEnumerable<(FrameworkElement Field, TextBlock Label, TextBlock? Title)> fields)
        {
            foreach (var (field, label, title) in fields)
            {
                _strategy.Apply(field, label, title);
            }
        }
    }
}
