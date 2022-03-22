using Newtonsoft.Json;
using System.Collections.Generic;

namespace Arbitrage.Models.FTXModels
{
    public class FTXMarket
    {
        /// <summary>
        /// Ask Orders in FTX Exchange
        /// </summary>
        [JsonProperty("asks")]
        public List<List<double>> Asks { get; set; }

        /// <summary>
        /// Bid Orders in FTX Exchange
        /// </summary>
        [JsonProperty("bids")]
        public List<List<double>> Bids { get; set; }
    }
}
