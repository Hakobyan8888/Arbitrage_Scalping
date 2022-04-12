using Arbitrage.Models;
using Arbitrage.Utils;
using ArbitrageDesktopApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageDesktopApp.ViewModels.Explorers
{
    public class CurrencyPairsExplorerViewModel : ExplorerViewModelBase
    {
        private List<ScalpingMarketsConfigsModel> _scalpingMarketsConfigs;

        public List<ScalpingMarketsConfigsModel> ScalpingMarketsConfigs
        {
            get => _scalpingMarketsConfigs;
            set => SetProperty(ref _scalpingMarketsConfigs, value);
        }

        public CurrencyPairsExplorerViewModel()
        {
            GetScalpingConfigs();
        }

        public void GetScalpingConfigs()
        {
            var json = Helper.GetMarketsConfigs();
            ScalpingMarketsConfigs = JsonConvert.DeserializeObject<List<ScalpingMarketsConfigsModel>>(json);
        }

        public void SetScalpingConfigs()
        {
            ArbitrageWrappers.ArbitrageWrapper.FTXViewModelBaseInstance.Stop();
            var json = JsonConvert.SerializeObject(ScalpingMarketsConfigs);
            Helper.SetMarketsConfigs(json);
            ArbitrageWrappers.ArbitrageWrapper.FTXViewModelBaseInstance.Start();
        }
    }
}
