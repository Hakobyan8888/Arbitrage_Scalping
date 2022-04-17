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
        private ObservableCollection<decimal> _raisingPercentsForAsks;
        private decimal _percentageOfMaxPrice;
        private decimal _raisingPercentForBid;
        private decimal _minPercentOfAvailableBigSizeAfterBuying;
        private decimal _firstRaisingPercentForAsk;
        private decimal _secondRaisingPercentForAsk;
        private decimal _thirdRaisingPercentForAsk;

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
        public ObservableCollection<decimal> RaisingPercentsForAsks
        {
            get => _raisingPercentsForAsks;
            set => SetProperty(ref _raisingPercentsForAsks, value);
        }

        /// <summary>
        /// The maximum price to bid(realPrice - PercantageOfMaxPrice%)
        /// </summary>
        [JsonProperty("percentageOfMaxPrice")]
        public decimal PercentageOfMaxPrice
        {
            get => _percentageOfMaxPrice;
            set => SetProperty(ref _percentageOfMaxPrice, value);
        }

        /// <summary>
        /// The percent to raise price on the BigSizeBid price to bid
        /// </summary>
        [JsonProperty("raisingPercentForBid")]
        public decimal RaisingPercentForBid
        {
            get => _raisingPercentForBid;
            set => SetProperty(ref _raisingPercentForBid, value);
        }

        /// <summary>
        /// The minimum percent of the big size that must be not selled after my bid is done
        /// </summary>
        [JsonProperty("minPercentOfAvailableBigSizeAfterBuying")]
        public decimal MinPercentOfAvailableBigSizeAfterBuying
        {
            get => _minPercentOfAvailableBigSizeAfterBuying;
            set => SetProperty(ref _minPercentOfAvailableBigSizeAfterBuying, value);
        }

        [JsonIgnore]
        public decimal FirstRaisingPercentForAsk
        {
            get => _firstRaisingPercentForAsk;
            set
            {
                SetProperty(ref _firstRaisingPercentForAsk, value);
                RaisingPercentsForAsks[0] = FirstRaisingPercentForAsk;
            }
        }
        [JsonIgnore]
        public decimal SecondRaisingPercentForAsk
        {
            get => _secondRaisingPercentForAsk;
            set
            {
                SetProperty(ref _secondRaisingPercentForAsk, value);
                RaisingPercentsForAsks[1] = SecondRaisingPercentForAsk;
            }
        }
        [JsonIgnore]
        public decimal ThirdRaisingPercentForAsk
        {
            get => _thirdRaisingPercentForAsk;
            set
            {
                SetProperty(ref _thirdRaisingPercentForAsk, value);
                RaisingPercentsForAsks[2] = ThirdRaisingPercentForAsk;
            }
        }
    }
}
