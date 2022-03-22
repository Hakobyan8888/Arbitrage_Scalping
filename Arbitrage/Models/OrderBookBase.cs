using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrage.Models
{
    /// <summary>
    /// Base Order Book
    /// </summary>
    public abstract class OrderBookBase
    {
        /// <summary>
        /// The name of the market
        /// </summary>
        public string MarketName { get; set; }

        /// <summary>
        /// Gets all Asks
        /// </summary>
        /// <returns></returns>
        public abstract List<AskBid> GetAllAsks();

        /// <summary>
        /// Gets all Bids
        /// </summary>
        /// <returns></returns>
        public abstract List<AskBid> GetAllBids();
    }
}
