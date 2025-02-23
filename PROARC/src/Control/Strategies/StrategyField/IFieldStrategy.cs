using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace PROARC.src.Control.Strategies.StrategyField
{
    public interface IFieldStrategy
    {
        void Apply(FrameworkElement field, TextBlock labelBlock, TextBlock? titleBlock = null);
    }
}
