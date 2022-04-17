using Arbitrage.Models;
using Arbitrage.Utils;
using ArbitrageDesktopApp.Models;
using Newtonsoft.Json;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageDesktopApp.ViewModels.Explorers
{
    public class CurrencyPairsExplorerViewModel : ExplorerViewModelBase
    {
        private ObservableCollection<ScalpingMarketsConfigsModel> _scalpingMarketsConfigs;

        public DelegateCommand ApplyPairsChangesCommand { get; set; }
        public DelegateCommand AddPairCommand { get; set; }
        public ObservableCollection<ScalpingMarketsConfigsModel> ScalpingMarketsConfigs
        {
            get => _scalpingMarketsConfigs;
            set => SetProperty(ref _scalpingMarketsConfigs, value);
        }

        public CurrencyPairsExplorerViewModel()
        {
            GetScalpingConfigs();
            ApplyPairsChangesCommand = new DelegateCommand(SetScalpingConfigs);
            AddPairCommand = new DelegateCommand(AddPair);
        }

        private void AddPair()
        {
            ScalpingMarketsConfigs.Add(new ScalpingMarketsConfigsModel
            {
                RaisingPercentsForAsks = new ObservableCollection<decimal>
                {
                    0,0,0
                },
            });
        }

        public void GetScalpingConfigs()
        {
            var json = Helper.GetMarketsConfigs();
            ScalpingMarketsConfigs = JsonConvert.DeserializeObject<ObservableCollection<ScalpingMarketsConfigsModel>>(json);
            foreach (var configs in ScalpingMarketsConfigs)
            {
                configs.FirstRaisingPercentForAsk = configs.RaisingPercentsForAsks[0];
                configs.SecondRaisingPercentForAsk = configs.RaisingPercentsForAsks[1];
                configs.ThirdRaisingPercentForAsk = configs.RaisingPercentsForAsks[2];
            }
        }

        public void SetScalpingConfigs()
        {
            ArbitrageWrappers.ArbitrageWrapper.FTXViewModelBaseInstance.Stop();
            var json = JsonConvert.SerializeObject(ScalpingMarketsConfigs);
            Helper.SetMarketsConfigs(json);
            ArbitrageWrappers.ArbitrageWrapper.FTXViewModelBaseInstance.Start();
        }

        public override void Update()
        {
        }
    }
}
