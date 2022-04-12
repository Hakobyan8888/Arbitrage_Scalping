using Arbitrage.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
        public static decimal RealPrice(decimal bestBidPrice, decimal bestAskPrice)
        {
            var price = (bestBidPrice + bestAskPrice) / 2;
            return price;
        }

        /// <summary>
        /// Calculate the right price to place a bid
        /// </summary>
        /// <param name="bigSizeBids">The bids with big size</param>
        /// <returns>Big size bid and the calculated bid price</returns>
        public static Tuple<AskBid, AskBid> CalculatePriceToBid(List<AskBid> bigSizeBids, decimal realPrice)
        {
            var maxPrice = realPrice * 0.995m;
            Logs.Log.StringBuilder.AppendLine($"the max price to bid is {maxPrice}");
            foreach (var bigSizeBid in bigSizeBids)
            {
                if (bigSizeBid.Price < maxPrice)
                {
                    var bid = new AskBid
                    {
                        MarketName = bigSizeBid.MarketName,
                        OrderType = bigSizeBid.OrderType,
                        Price = bigSizeBid.Price * 1.003m,
                    };
                    Logs.Log.StringBuilder.AppendLine($"Calculated bid price is {bid.Price} Market name is {bid.MarketName} order type is {bid.OrderType}");
                    return Tuple.Create(bigSizeBid, bid);
                }
            }
            Logs.Log.StringBuilder.AppendLine($"the BigSizeBids are greater than max price");
            return null;
        }

        /// <summary>
        /// Calculates the Size to bid
        /// </summary>
        /// <param name="myBalance">balance $</param>
        /// <param name="bid">The price calculated bid(after this method call CalculatePriceToBid)</param>
        /// <returns></returns>
        public static AskBid AmountToBid(decimal myBalance, AskBid bid)
        {
            var balanceToBid = myBalance * 0.1m;
            bid.Size = balanceToBid / bid.Price;
            Logs.Log.StringBuilder.AppendLine($"The size to bid is {bid.Size}");
            return bid;
        }

        public static List<AskBid> CalculateAsks(AskBid bid, List<decimal> percentages, decimal balance)
        {
            Logs.Log.StringBuilder.AppendLine($"Calculate Asks");
            List<AskBid> asks = new List<AskBid>();
            bool firstTime = true;
            var size = bid.Size / (percentages.Count + 1);
            foreach (var percent in percentages)
            {
                if (firstTime)
                {
                    size *= 2;
                    firstTime = false;
                }
                else
                {
                    size = bid.Size / (percentages.Count + 1);
                }
                asks.Add(new AskBid
                {
                    MarketName = bid.MarketName,
                    OrderType = Enums.OrderType.Ask,
                    Price = bid.Price * ((100 + percent) / 100),
                    Size = size,
                });
            }
            int i = 1;
            foreach (var ask in asks)
            {
                Logs.Log.StringBuilder.AppendLine($"{i}. ask: Market Name:{ask.MarketName}, price:{ask.Price}, size:{ask.Size}");
                i++;
            }
            return asks;
        }

        public static string GetMarketsConfigs()
        {
            using StreamReader r = new StreamReader("Assets/Jsons/ScalpingMarketsConfigs.json");
            string json = r.ReadToEnd();
            return json;
        }

        public static void SetMarketsConfigs(string json)
        {
            using StreamWriter r = new StreamWriter("Assets/Jsons/ScalpingMarketsConfigs.json");
            r.Write(json);
        }
    }
}
