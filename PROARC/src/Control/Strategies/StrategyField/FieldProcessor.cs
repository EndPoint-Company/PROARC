using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace PROARC.src.Control.Strategies.StrategyField
{
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
