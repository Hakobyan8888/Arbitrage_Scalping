using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arbitrage.Models.FTXModels
{
    public class FTXOrderResponse : OrderBase
    {
        /// <summary>
        /// Is Response Success
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        /// Order info
        /// </summary>
        [JsonProperty("result")]
        public FTXOrder Order { get; set; }

        public override FTXOrder GetOrder()
        {
            return Order;
        }
    }
}
