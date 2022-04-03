using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arbitrage.Models
{
    public class Wallet
    {

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public List<Balance> Balances { get; set; }
    }

    public class Balance
    {
        /// <summary>
        /// The Crypto name
        /// </summary>
        public string Coin { get; set; }

        /// <summary>
        /// free amount
        /// </summary>
        public decimal Free { get; set; }

        /// <summary>
        /// amount borrowed using spot margin
        /// </summary>
        public decimal SpotBorrow { get; set; }

        /// <summary>
        /// total amount
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// approximate total amount in USD
        /// </summary>
        public decimal UsdValue { get; set; }

        /// <summary>
        /// amount available without borrowing
        /// </summary>
        public decimal AvailableWithoutBorrow { get; set; }
    }
}
