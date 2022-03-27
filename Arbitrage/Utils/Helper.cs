using Arbitrage.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arbitrage.Utils
{
    public static class Helper
    {
        /// <summary>
        /// Calculate the real price for crypto
        /// </summary>
        /// <param name="bestBidPrice">the Maximum bid price</param>
        /// <param name="bestAskPrice">the Minimum ask price</param>
        /// <returns></returns>
        public static double RealPrice(double bestBidPrice, double bestAskPrice)
        {
            var price = (bestBidPrice + bestAskPrice) / 2;
            return price;
        }

        /// <summary>
        /// Calculate the right price to place a bid
        /// </summary>
        /// <param name="bigSizeBids">The bids with big size</param>
        /// <returns>Big size bid and the calculated bid price</returns>
        public static Tuple<AskBid, AskBid> CalculatePriceToBid(List<AskBid> bigSizeBids, double realPrice)
        {
            var maxPrice = realPrice * 0.995;
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
                    return Tuple.Create(bigSizeBid, bid);
                }
            }
            return Tuple.Create(new AskBid(), new AskBid());
        }

        /// <summary>
        /// Calculates the Size to bid
        /// </summary>
        /// <param name="myBalance">balance $</param>
        /// <param name="bid">The price calculated bid(after this method call CalculatePriceToBid)</param>
        /// <returns></returns>
        public static AskBid AmountToBid(double myBalance, AskBid bid)
        {
            var balanceToBid = myBalance * 0.1;
            bid.Size = balanceToBid / bid.Price;
            return bid;
        }

        public static List<AskBid> CalculateAsks(AskBid bid, List<double> percentages)
        {
            List<AskBid> asks = new List<AskBid>();
            bool firstTime = true;
            var size = bid.Size / percentages.Count + 1;
            foreach (var percent in percentages)
            {
                if (firstTime)
                {
                    size *= 2;
                    firstTime = false;
                }
                else
                {
                    size = bid.Size / percentages.Count + 1;
                }
                asks.Add(new AskBid
                {
                    MarketName = bid.MarketName,
                    OrderType = Enums.OrderType.Ask,
                    Price = bid.Price * ((100 + percent) / 100),
                    Size = size,
                });
            }
            return asks;
        }
    }
}
