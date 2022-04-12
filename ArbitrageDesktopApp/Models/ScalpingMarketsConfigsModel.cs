using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageDesktopApp.Models
{
    public class ScalpingMarketsConfigsModel : BindableBase
    {
        private string marketName;
        private decimal _minSize;
        private decimal _fee;
        private ObservableCollection<decimal> _raisePercent;

        /// <summary>
        /// Name of the Market
        /// </summary>
        [JsonProperty("marketName")]
        public string MarketName
        {
            get => marketName;
            set => SetProperty(ref marketName, value);
        }

        /// <summary>
        /// Minimum Size of bid to scalp
        /// </summary>
        [JsonProperty("minSize")]
        public decimal MinSize
        {
            get => _minSize;
            set => SetProperty(ref _minSize, value);
        }

        /// <summary>
        /// Fee of the trading in percentage
        /// </summary>
        [JsonProperty("marketFee")]
        public decimal Fee
        {
            get => _fee;
            set => SetProperty(ref _fee, value);
        }

        /// <summary>
        /// The percent to raise the price
        /// </summary>
        [JsonProperty("raisePercent")]
        public ObservableCollection<decimal> RaisePercent
        {
            get => _raisePercent;
            set => SetProperty(ref _raisePercent, value);
        }
    }
}
