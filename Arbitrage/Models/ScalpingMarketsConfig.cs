using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arbitrage.Models
{
    public class ScalpingMarketsConfig
    {
        /// <summary>
        /// Name of the Market
        /// </summary>
        [JsonProperty("marketName")]
        public string MarketName { get; set; }

        /// <summary>
        /// Minimum Size of bid to scalp
        /// </summary>
        [JsonProperty("minSize")]
        public decimal MinSize { get; set; }

        /// <summary>
        /// Fee of the trading in percentage
        /// </summary>
        [JsonProperty("marketFee")]
        public decimal Fee { get; set; }

        /// <summary>
        /// The maximum price to bid(realPrice - PercantageOfMaxPrice%)
        /// </summary>
        [JsonProperty("percentageOfMaxPrice")]
        public decimal PercentageOfMaxPrice { get; set; }

        /// <summary>
        /// The minimum percent of the big size that must be not selled after my bid is done
        /// </summary>
        [JsonProperty("minPercentOfAvailableBigSizeAfterBuying")]
        public decimal MinPercentOfAvailableBigSizeAfterBuying { get; set; }

        /// <summary>
        /// The percent to raise the price
        /// </summary>
        [JsonProperty("raisePercent")]
        public List<decimal> RaisingPercentsForAsks { get; set; }

        /// <summary>
        /// The percent to raise price on the BigSizeBid price to bid
        /// </summary>
        [JsonProperty("raisingPercentForBid")]
        public decimal RaisingPercentForBid { get; set; }
    }
}
