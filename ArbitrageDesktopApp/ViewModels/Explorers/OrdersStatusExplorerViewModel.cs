using Arbitrage.Models;
using Arbitrage.Models.FTXModels;
using ArbitrageDesktopApp.ArbitrageWrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageDesktopApp.ViewModels.Explorers
{
    public class OrdersStatusExplorerViewModel : ExplorerViewModelBase
    {
        private ObservableCollection<FTXOrder> _openOrders;
        private ObservableCollection<FTXOrder> _closedOrders;

        public ObservableCollection<FTXOrder> OpenOrders
        {
            get => _openOrders;
            set => SetProperty(ref _openOrders, value);
        }
        public ObservableCollection<FTXOrder> ClosedOrders
        {
            get => _closedOrders;
            set => SetProperty(ref _closedOrders, value);
        }
        public OrdersStatusExplorerViewModel()
        {

        }
        public override void Update()
        {
            OpenOrders = new ObservableCollection<FTXOrder>();
            foreach (var order in ArbitrageWrapper.FTXViewModelBaseInstance.Orders)
            {
                OpenOrders.Add(order.PlacedOrder.GetOrder());
            }
            ClosedOrders = new ObservableCollection<FTXOrder>();

            foreach (var order in ArbitrageWrapper.FTXViewModelBaseInstance.CompletedOrders)
            {
                ClosedOrders.Add(order.PlacedOrder.GetOrder());
            }
        }
    }
}
