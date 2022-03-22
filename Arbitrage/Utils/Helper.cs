using Arbitrage.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arbitrage.Utils
{
    public static class Helper
    {
        public static AskBid CalculatePriceToBid(List<AskBid> bigSizeBids, double bestBidPrice, double bestAskPrice)
        {
            var price = (bestAskPrice + bestAskPrice) / 2;
            var maxPrice = price * 0.995;
            foreach (var bigSizeBid in bigSizeBids)
            {
                if (bigSizeBid.Price < maxPrice)
                {
                    var bid = new AskBid
                    {
                        MarketName = bigSizeBid.MarketName,
                        OrderType = bigSizeBid.OrderType,
                        Price = bigSizeBid.Price * 1.003,
                    };
                    return bid;
                }
            }
            return new AskBid();
        }
    }
}
