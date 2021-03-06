using Arbitrage.Enums;

namespace Arbitrage.Models
{
    /// <summary>
    /// The Price and Count for selling the crypto
    /// </summary>
    public struct AskBid
    {
        /// <summary>
        /// Price for one token
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Size of the Tokens to sell
        /// </summary>
        public decimal Size { get; set; }

        /// <summary>
        /// The type of order is placed(ask/bid)
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// The Name of the market
        /// </summary>
        public string MarketName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is AskBid askBid)
            {
                return askBid.MarketName == MarketName && askBid.OrderType == OrderType && askBid.Price == askBid.Price && askBid.Size == Size;
            }
            return base.Equals(obj);
        }
    }
}
