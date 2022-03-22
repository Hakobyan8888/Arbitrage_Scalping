using Arbitrage.Models;
using FtxApi;
using FtxApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrage.Services
{
    public abstract class ServiceBase
    {
        public ServiceBase()
        {
        }

        /// <summary>
        /// Get the Order Book
        /// </summary>
        /// <param name="marketName">The name of the market</param>
        /// <param name="depth">The depth of the Order Book</param>
        /// <returns>OrderBook of market</returns>
        public abstract Task<OrderBookBase> GetOrderBookAsync(string marketName, int depth = 20);

        /// <summary>
        /// Place Order on exchange
        /// </summary>
        /// <param name="instrument">Pair of Cryptos</param>
        /// <param name="side">Ask or Bid</param>
        /// <param name="price">Price on which to sell or buy crypto</param>
        /// <param name="orderType">Limit or Market</param>
        /// <param name="amount">Amount of crypto to buy or sell</param>
        /// <param name="reduceOnly">Order condition that only allows you to reduce or close a current position you hold</param>
        /// <returns>new placed Order model</returns>
        public abstract Task<OrderBase> PlaceOrderAsync(string instrument, SideType side, decimal price,
            OrderType orderType, decimal amount, bool reduceOnly = false);

        /// <summary>
        /// Get the status of made order
        /// </summary>
        /// <param name="id">Id of order</param>
        /// <returns>OrderModel</returns>
        public abstract Task<OrderBase> GetOrderStatusAsync(string id);
        
        /// <summary>
        /// Modify made order price
        /// </summary>
        /// <param name="orderId">Order id to modify</param>
        /// <param name="triggerPrice">Price to change</param>
        /// <returns>new modified Order model</returns>
        public abstract Task<OrderBase> ModifyOrderPriceAsync(string orderId, decimal triggerPrice);

        /// <summary>
        /// Modify made order size(amount)
        /// </summary>
        /// <param name="orderId">Order id to modify</param>
        /// <param name="size">Size to change</param>
        /// <returns>new modified Order model</returns>
        public abstract Task<OrderBase> ModifyOrderSizeAsync(string orderId, decimal size);

        /// <summary>
        /// Modify made order price and size
        /// </summary>
        /// <param name="orderId">>Order id to modify</param>
        /// <param name="size">Size to change</param>
        /// <param name="triggerPrice">Price to change</param>
        /// <returns>new modified Order model</returns>
        public abstract Task<OrderBase> ModifyOrderAsync(string orderId, decimal size, decimal triggerPrice);

        /// <summary>
        /// Cancel made Order
        /// </summary>
        /// <param name="orderId">Order id to cancel</param>
        /// <returns></returns>
        public abstract Task<bool> CancelOrderAsync(string orderId);
    }
}
