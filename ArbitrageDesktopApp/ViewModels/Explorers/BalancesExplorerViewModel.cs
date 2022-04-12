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
        private Timer _timerForUpdating;
        private Wallet _wallet;

        public Wallet Wallet
        {
            get => _wallet;
            set => SetProperty(ref _wallet, value);
        }


        public BalancesExplorerViewModel()
        {
            _timerForUpdating = new Timer();
            _timerForUpdating.Interval = 3500;
            _timerForUpdating.Elapsed += _timerForUpdating_Elapsed;
        }

        private void _timerForUpdating_Elapsed(object sender, ElapsedEventArgs e)
        {
            Wallet = ArbitrageWrapper.FTXViewModelBaseInstance.Wallet;
        }
    }
}
