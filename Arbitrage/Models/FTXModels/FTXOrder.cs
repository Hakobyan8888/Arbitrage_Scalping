using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arbitrage.Models.FTXModels
{
    public class FTXOrder
    {

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("filledSize")]
        public int FilledSize { get; set; }

        [JsonProperty("future")]
        public string Future { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("market")]
        public string Market { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("remainingSize")]
        public int RemainingSize { get; set; }

        [JsonProperty("side")]
        public string Side { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("reduceOnly")]
        public bool ReduceOnly { get; set; }

        [JsonProperty("ioc")]
        public bool Ioc { get; set; }

        [JsonProperty("postOnly")]
        public bool PostOnly { get; set; }

        [JsonProperty("clientId")]
        public object ClientId { get; set; }
    }
}
