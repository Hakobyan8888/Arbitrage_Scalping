using Arbitrage.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Arbitrage.Models.FTXModels
{
    /// <summary>
    /// FTX Exchange Order Book
    /// </summary>
    public class FTXOrderBook : OrderBookBase
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public FTXMarket FTXMarket { get; set; }

        /// <summary>
        /// Gets all Asks
        /// </summary>
        /// <returns></returns>
        public override List<AskBid> GetAllAsks()
        {
            var askList = new List<AskBid>();
            foreach (var ask in FTXMarket.Asks)
            {
                askList.Add(new AskBid
                {
                    Price = ask[0],
                    Size = ask[1],
                    OrderType = OrderType.Ask,
                    MarketName = MarketName,
                });
            }
            return askList;
        }

        /// <summary>
        /// Gets all Bids
        /// </summary>
        /// <returns></returns>
        public override List<AskBid> GetAllBids()
        {
            var bidList = new List<AskBid>();
            foreach (var bid in FTXMarket.Bids)
            {
                bidList.Add(new AskBid
                {
                    Price = bid[0],
                    Size = bid[1],
                    OrderType = OrderType.Bid,
                    MarketName = MarketName,
                });
            }
            return bidList;
        }
    }
}
