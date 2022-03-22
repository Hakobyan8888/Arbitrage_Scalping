using Arbitrage.Models;
using Arbitrage.Models.FTXModels;
using FtxApi;
using FtxApi.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrage.Services
{
    public class FTXService : ServiceBase
    {
        private Client _client;
        private FtxRestApi _ftxRestApi;

        public FTXService(Client client) : base()
        {
            _client = client;
            _ftxRestApi = new FtxRestApi(_client);
        }

        public override async Task<OrderBookBase> GetOrderBookAsync(string marketName, int depth = 20)
        {
            var orderBook = await _ftxRestApi.GetMarketOrderBookAsync(marketName, depth);
            var orderBookSerialized = JsonConvert.DeserializeObject<FTXOrderBook>(orderBook);
            orderBookSerialized.MarketName = marketName;
            return orderBookSerialized;
        }

        public override async Task<OrderBase> PlaceOrderAsync(string instrument, SideType side, decimal price,
            OrderType orderType, decimal amount, bool reduceOnly = false)
        {
            var orderJson = await _ftxRestApi.PlaceOrderAsync(instrument, side, price, orderType, amount, reduceOnly);
            var order = JsonConvert.DeserializeObject<FTXOrderResponse>(orderJson);
            return order;
        }

        public override async Task<OrderBase> GetOrderStatusAsync(string id)
        {
            var orderJson = await _ftxRestApi.GetOrderStatusAsync(id);
            var order = JsonConvert.DeserializeObject<FTXOrderResponse>(orderJson);
            return order;
        }

        public override async Task<OrderBase> ModifyOrderPriceAsync(string orderId, decimal triggerPrice)
        {
            var orderJson = await _ftxRestApi.ModifyOrderPriceAsync(orderId, triggerPrice);
            var order = JsonConvert.DeserializeObject<FTXOrderResponse>(orderJson);
            return order;
        }

        public override async Task<OrderBase> ModifyOrderSizeAsync(string orderId, decimal size)
        {
            var orderJson = await _ftxRestApi.ModifyOrderSizeAsync(orderId, size);
            var order = JsonConvert.DeserializeObject<FTXOrderResponse>(orderJson);
            return order;
        }

        public override async Task<OrderBase> ModifyOrderAsync(string orderId, decimal size, decimal triggerPrice)
        {
            var orderJson = await _ftxRestApi.ModifyOrderAsync(orderId, size, triggerPrice);
            var order = JsonConvert.DeserializeObject<FTXOrderResponse>(orderJson);
            return order;
        }

        public override async Task<bool> CancelOrderAsync(string orderId)
        {
            var order = await _ftxRestApi.CancelOrderAsync(orderId);
            return order.success;
        }
    }
}
