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
        public string MarketName { get; set; }

        /// <summary>
        /// Minimum Size of bid to scalp
        /// </summary>
        public double MinSize { get; set; }

        /// <summary>
        /// Fee of the trading in percentage
        /// </summary>
        public double Fee { get; set; }

        /// <summary>
        /// The percent to raise the price
        /// </summary>
        public List<double> RaisePercent { get; set; }
    }
}
