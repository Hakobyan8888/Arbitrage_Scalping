using ArbitrageDesktopApp.ViewModels.Explorers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ArbitrageDesktopApp.DataTemplateSelectors
{
    public class ExplorerSelector : DataTemplateSelector
    {
        public DataTemplate BalancesExplorerDataTemplate { get; set; }
        public DataTemplate CurrencyExplorerDataTemplate { get; set; }
        public DataTemplate OrderStatusExplorerDataTemplate { get; set; }
        public DataTemplate HomeExplorerDataTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is CurrencyPairsExplorerViewModel)
            {
                return CurrencyExplorerDataTemplate;
            }
            else if (item is OrdersStatusExplorerViewModel)
            {
                return OrderStatusExplorerDataTemplate;
            }
            else if (item is BalancesExplorerViewModel)
            {
                return BalancesExplorerDataTemplate;
            }
            else if (item is HomeExplorerViewModel)
            {
                return HomeExplorerDataTemplate;
            }
            return base.SelectTemplate(item, container);
        }

    }
}
