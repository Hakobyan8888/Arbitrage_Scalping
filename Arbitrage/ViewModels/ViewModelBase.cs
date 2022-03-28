using Arbitrage.Models;
using Arbitrage.Services;
using Arbitrage.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Arbitrage.ViewModels
{
    public abstract class ViewModelBase
    {
        /// <summary>
        /// List of Orders that are opened
        /// Tuple<Big Size by which was opened, Order>
        /// </summary>
        protected List<ScalpingModel> Orders { get; set; }
        protected ServiceBase Service { get; set; }

        protected List<ScalpingMarketsConfig> ScalpingMarketsConfigs { get; set; }

        private Timer _timer;
        public virtual void Start()
        {
            _timer = new Timer();
            _timer.Interval = 8500;
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Start();
            using StreamReader r = new StreamReader("Assets/Jsons/ScalpingMarketsConfigs.json");
            string json = r.ReadToEnd();
            ScalpingMarketsConfigs = JsonConvert.DeserializeObject<List<ScalpingMarketsConfig>>(json);
        }
        public virtual void Stop()
        {
            _timer.Elapsed -= Timer_Elapsed;
            _timer?.Stop();
            _timer = null;
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var configs in ScalpingMarketsConfigs)
            {
                var orderBook = await Service.GetOrderBookAsync(configs.MarketName, Constants.ORDER_BOOK_DEPTH);
                var validBids = GetValidBidsByGivenSize(orderBook, configs.MinSize);
                var orders = Orders.Where(x => x.PlacedOrder.GetOrder().Market == configs.MarketName);
                if (validBids.Count > 0)
                {
                    if (orders != null || orders.Count() == 0)
                    {
                        foreach (var order in orders)
                        {
                            var orderStatus = await Service.GetOrderStatusAsync(order.PlacedOrder.GetOrder().Id.ToString());
                            if (orderStatus.GetOrder().Status == "new" || orderStatus.GetOrder().Status == "open")
                            {
                                await CheckOpenAndNewOrders(validBids, orderStatus, order, orderBook);
                            }
                            else if (orderStatus.GetOrder().Status == "closed")
                            {
                                await CheckClosedOrders(validBids, orderStatus, order, orderBook, configs);
                            }
                        }
                    }
                    else
                    {
                        await OpenNewBidOrder(validBids, orderBook);
                    }
                }
                else
                {
                    if (orders != null)
                    {
                        foreach (var order in orders)
                        {
                            var orderStatus = await Service.GetOrderStatusAsync(order.PlacedOrder.GetOrder().Id.ToString());
                            if (orderStatus.GetOrder().Status == "new" || orderStatus.GetOrder().Status == "open")
                            {
                                //cancel orders and sell bought cryptos
                                await CancelOrderAndSellCryptos(orderStatus, order, orderBook);
                            }
                            else if (orderStatus.GetOrder().Status == "closed")
                            {
                                validBids = GetValidBidsByGivenSize(orderBook, configs.MinSize * 0.3);
                                await CheckClosedOrders(validBids, orderStatus, order, orderBook, configs);
                            }
                        }
                    }
                }
            }

            //Check if the valid orders are bots


            //var bestBidPrice = btcOrderBook.GetAllBids().Max(x => x.Price);
            //var bestAskPrice = btcOrderBook.GetAllAsks().Min(x => x.Price);

            //Calculate the Price to bid
            //var priceToBid = Helper.CalculatePriceToBid(btcValidOrder, bestBidPrice, bestAskPrice);
            //var balance = await Service.GetBalance();
            //var amountToBid = Helper.AmountToBid(balance, priceToBid);
            //var order = await Service.PlaceOrderAsync(FTXMarkets.FUTURE_BTC_USD, FtxApi.Enums.SideType.buy, Convert.ToDecimal(priceToBid.Price), FtxApi.Enums.OrderType.limit, Convert.ToDecimal(amountToBid));

            //Orders.Add(order);

            //#region Telegram Messages
            //foreach (var item in btcValidOrder)
            //{
            //    await Program._telegramBotClient.SendTextMessageAsync(Program.ChatId, $"Market Name - {item.MarketName}\n Order Type - {item.OrderType} \n Size - {item.Size} \n Price - {item.Price}");
            //    Console.WriteLine($"{item.MarketName} {item.OrderType} : {item.Size} --- {item.Price}");
            //}
            //foreach (var item in solValidOrder)
            //{
            //    await Program._telegramBotClient.SendTextMessageAsync(Program.ChatId, $"Market Name - {item.MarketName}\n Order Type - {item.OrderType} \n Size - {item.Size} \n Price - {item.Price}");
            //    Console.WriteLine($"{item.MarketName} {item.OrderType} : {item.Size} --- {item.Price}");
            //}
            //foreach (var item in avaxValidOrder)
            //{
            //    await Program._telegramBotClient.SendTextMessageAsync(Program.ChatId, $"Market Name - {item.MarketName}\n Order Type - {item.OrderType} \n Size - {item.Size} \n Price - {item.Price}");
            //    Console.WriteLine($"{item.MarketName} {item.OrderType} : {item.Size} --- {item.Price}");
            //}
            //foreach (var item in atomValidOrder)
            //{
            //    await Program._telegramBotClient.SendTextMessageAsync(Program.ChatId, $"Market Name - {item.MarketName}\n Order Type - {item.OrderType} \n Size - {item.Size} \n Price - {item.Price}");
            //    Console.WriteLine($"{item.MarketName} {item.OrderType} : {item.Size} --- {item.Price}");
            //}
            //#endregion
        }


        /// <summary>
        /// return all Bids that match the condition of size
        /// </summary>
        /// <param name="orderBook">the order book which is going to be checked</param>
        /// <param name="minSize">The min amount to look</param>
        /// <returns></returns>
        private List<AskBid> GetValidBidsByGivenSize(OrderBookBase orderBook, double minSize)
        {
            List<AskBid> orderResults = new List<AskBid>();
            var bids = orderBook.GetAllBids();

            foreach (var bid in bids)
            {
                if (bid.Size >= minSize)
                {
                    orderResults.Add(bid);
                }
            }

            return orderResults;
        }

        /// <summary>
        /// return all orders that match the condition of size
        /// </summary>
        /// <param name="orderBook">the order book which is going to be checked</param>
        /// <returns></returns>
        private List<AskBid> GetValidOrdersByGivenSize(OrderBookBase orderBook, double minSize)
        {
            var asks = orderBook.GetAllAsks();
            var bids = orderBook.GetAllBids();

            List<AskBid> orderResults = new List<AskBid>();

            foreach (var ask in asks)
            {
                if (ask.Size >= minSize)
                {
                    orderResults.Add(ask);
                }
            }

            foreach (var bid in bids)
            {
                if (bid.Size >= minSize)
                {
                    orderResults.Add(bid);
                }
            }

            return orderResults;
        }

        /// <summary>
        /// Check if Open Orders are needed to be canceled and selled
        /// </summary>
        /// <param name="validBids">The order book valid bids</param>
        /// <param name="orderStatus">Status of the order</param>
        /// <param name="order">Scalping Order</param>
        /// <param name="orderBook">Current Order book</param>
        private async Task CheckOpenAndNewOrders(List<AskBid> validBids, OrderBase orderStatus, ScalpingModel order, OrderBookBase orderBook)
        {
            foreach (var bid in validBids)
            {
                if (bid.Size >= order.BigSizeBid.Size * 0.9 && bid.Price == order.BigSizeBid.Price)
                {
                    return;
                }
                else
                {
                    await CancelOrderAndSellCryptos(orderStatus, order, orderBook);
                    return;
                }
            }
        }

        /// <summary>
        /// Cancel the placed order and sell the filled cryptos
        /// </summary>
        /// <param name="orderStatus">Status of the order</param>
        /// <param name="order">Scalping Order</param>
        /// <param name="orderBook">Current Order book</param>
        private async Task CancelOrderAndSellCryptos(OrderBase orderStatus, ScalpingModel order, OrderBookBase orderBook)
        {
            //Cancel orders and sell bought cryptos
            var canceledOrder = await Service.CancelOrderAsync(orderStatus.GetOrder().Id.ToString());
            if (canceledOrder)
            {
                await SellCryptos(orderStatus, order, orderBook);
            }
        }

        /// <summary>
        /// Sell the Cryptos
        /// </summary>
        /// <param name="orderStatus">Status of the order</param>
        /// <param name="order">Scalping Order</param>
        /// <param name="orderBook">Current Order book</param>
        private async Task SellCryptos(OrderBase orderStatus, ScalpingModel order, OrderBookBase orderBook)
        {
            Orders.Remove(order);
            //Check bought cryptos balance
            var boughtCrypto = (await Service.GetBalance()).Balances.FirstOrDefault(x => orderStatus.GetOrder().Market.Contains(x.Coin));
            //Sell the cryptos for best price
            var bestAskPrice = orderBook.GetAllAsks().Min(x => x.Price);
            var placedOrder = await Service.PlaceOrderAsync(FTXMarkets.FUTURE_BTC_USD, FtxApi.Enums.SideType.sell, Convert.ToDecimal(bestAskPrice), FtxApi.Enums.OrderType.limit, Convert.ToDecimal(boughtCrypto.Free));
            Orders.Add(new ScalpingModel
            {
                PlacedOrder = placedOrder,
                RealPrice = bestAskPrice
            });
        }

        /// <summary>
        /// Check the closed Orders if it is needed to sell immediately
        /// </summary>
        /// <param name="validBids">The order book valid bids</param>
        /// <param name="orderStatus">Status of the order</param>
        /// <param name="order">Scalping Order</param>
        /// <param name="orderBook">Current Order book</param>
        /// <param name="configs">Configs for the scalping</param>
        /// <returns></returns>
        private async Task CheckClosedOrders(List<AskBid> validBids, OrderBase orderStatus, ScalpingModel order, OrderBookBase orderBook, ScalpingMarketsConfig configs)
        {
            if (order.PlacedOrder.GetOrder().Side == FtxApi.Enums.SideType.sell.ToString())
            {
                Orders.Remove(order);
                return;
            }
            foreach (var bid in validBids)
            {
                if (bid.Size >= order.BigSizeBid.Size * 0.3 && bid.Price == order.BigSizeBid.Price)
                {
                    //Make Ask
                    Orders.Remove(order);
                    var asks = Helper.CalculateAsks(bid, configs.RaisePercent);
                    foreach (var ask in asks)
                    {
                        var placedOrder = await Service.PlaceOrderAsync(FTXMarkets.FUTURE_BTC_USD, FtxApi.Enums.SideType.sell, Convert.ToDecimal(ask.Price), FtxApi.Enums.OrderType.limit, Convert.ToDecimal(ask.Size));
                        //Get maximum price for bid and minimum price for ask for calculating the real price of crypto
                        var bestBidPrice = orderBook.GetAllBids().Max(x => x.Price);
                        var bestAskPrice = orderBook.GetAllAsks().Min(x => x.Price);
                        Orders.Add(new ScalpingModel
                        {
                            PlacedOrder = placedOrder,
                            RealPrice = Helper.RealPrice(bestBidPrice, bestAskPrice)
                        });
                    }
                    return;
                }
            }
            await SellCryptos(orderStatus, order, orderBook);
        }

        /// <summary>
        /// Open the Scalping bid order
        /// </summary>
        /// <param name="validBids">The valid bids</param>
        /// <param name="orderBook">Current Order book</param>
        private async Task OpenNewBidOrder(List<AskBid> validBids, OrderBookBase orderBook)
        {
            #region Place Order
            //Get maximum price for bid and minimum price for ask for calculating the real price of crypto
            var bestBidPrice = orderBook.GetAllBids().Max(x => x.Price);
            var bestAskPrice = orderBook.GetAllAsks().Min(x => x.Price);
            //Calculate the Price and Size to bid
            var realPrice = Helper.RealPrice(bestBidPrice, bestAskPrice);
            var priceToBid = Helper.CalculatePriceToBid(validBids, realPrice);
            var balance = (await Service.GetBalance()).Balances.FirstOrDefault(x => x.Coin.ToLower() == "usd").Free;
            var amountToBid = Helper.AmountToBid(balance, priceToBid.Item2);
            //Place an order
            var placedOrder = await Service.PlaceOrderAsync(FTXMarkets.FUTURE_BTC_USD, FtxApi.Enums.SideType.buy, Convert.ToDecimal(priceToBid.Item2.Price), FtxApi.Enums.OrderType.limit, Convert.ToDecimal(amountToBid.Size));
            Orders.Add(new ScalpingModel
            {
                BigSizeBid = priceToBid.Item1,
                RealPrice = realPrice,
                PlacedOrder = placedOrder
            });
            #endregion
        }
    }
}
