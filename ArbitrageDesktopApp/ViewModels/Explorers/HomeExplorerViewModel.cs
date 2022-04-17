using ArbitrageDesktopApp.ArbitrageWrappers;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageDesktopApp.ViewModels.Explorers
{
    public class HomeExplorerViewModel : ExplorerViewModelBase
    {
        public DelegateCommand StartArbitrageCommand { get; set; }
        public DelegateCommand StopArbitrageCommand { get; set; }
        public HomeExplorerViewModel()
        {
            StartArbitrageCommand = new DelegateCommand(StartArbitrage);
            StopArbitrageCommand = new DelegateCommand(StopArbitrage);
        }

        private void StartArbitrage()
        {
            ArbitrageWrapper.FTXViewModelBaseInstance.Start();
        }

        private void StopArbitrage()
        {
            ArbitrageWrapper.FTXViewModelBaseInstance.Stop();
        }

        public override void Update()
        {
        }
    }
}
