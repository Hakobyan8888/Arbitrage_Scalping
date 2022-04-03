using System;
using System.Collections.Generic;
using System.Text;

namespace Arbitrage.Models
{
    public class ScalpingModel
    {
        /// <summary>
        /// The real price of the crypto
        /// </summary>
        public decimal RealPrice { get; set; }

        /// <summary>
        /// The big size bid that is considered for the
        /// </summary>
        public AskBid BigSizeBid { get; set; }

        /// <summary>
        /// Order that is placed
        /// </summary>
        public OrderBase PlacedOrder { get; set; }
    }
}
