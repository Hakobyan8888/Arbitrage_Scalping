using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Arbitrage.Models;
using ArbitrageDesktopApp.ArbitrageWrappers;

namespace ArbitrageDesktopApp.ViewModels.Explorers
{
    public class BalancesExplorerViewModel : ExplorerViewModelBase
    {
        private Wallet _wallet;

        public Wallet Wallet
        {
            get => _wallet;
            set => SetProperty(ref _wallet, value);
        }


        public BalancesExplorerViewModel()
        {
        }

        public override void Update()
        {
            Wallet = ArbitrageWrapper.FTXViewModelBaseInstance.Wallet;
        }
    }
}
