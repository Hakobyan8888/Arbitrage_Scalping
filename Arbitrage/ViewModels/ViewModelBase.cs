using Arbitrage.Models;
using Arbitrage.Services;
using Arbitrage.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        protected List<AskBid> OrdersToBid { get; set; }
        protected ServiceBase Service { get; set; }

        private bool _isCompleted = true;
        private bool _isTimerCompleted = true;

        protected List<ScalpingMarketsConfig> ScalpingMarketsConfigs { get; set; }

        private Timer _timer;
        private Timer _mainTimer;
        public virtual void Start()
        {
            Orders = new List<ScalpingModel>();
            OrdersToBid = new List<AskBid>();
            using StreamReader r = new StreamReader("Assets/Jsons/ScalpingMarketsConfigs.json");
            string json = r.ReadToEnd();
            ScalpingMarketsConfigs = JsonConvert.DeserializeObject<List<ScalpingMarketsConfig>>(json);
            _timer = new Timer();
            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = 3000;
            _timer.Start();
            _mainTimer = new Timer();
            _mainTimer.Interval = 2000;
            _mainTimer.Elapsed += _mainTimer_Elapsed;
            _mainTimer.Start();
        }

        private async void _mainTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_isCompleted && _isTimerCompleted)
            {
                _isTimerCompleted = false;
                _isCompleted = false;
                _timer.Start();
                await MainFunction(null, null);
            }
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _isTimerCompleted = true;
            _timer.Stop();
        }

        public virtual void Stop()
        {
            //_timer.Elapsed -= Timer_Elapsed;
            //_timer?.Stop();
            //_timer = null;
        }

        private async Task MainFunction(object sender, ElapsedEventArgs e)
        {
            try
            {
                foreach (var configs in ScalpingMarketsConfigs)
                {
                    var orderBook = await Service.GetOrderBookAsync(configs.MarketName, Constants.ORDER_BOOK_DEPTH);
                    var validBids = GetValidBidsByGivenSize(orderBook, configs.MinSize);
                    var orders = Orders.Where(x => x.PlacedOrder.GetOrder().Market == configs.MarketName).ToList();
                    if (validBids.Count > 0)
                    {
                        if (orders.Any())
                        {
                            Logs.Log.StringBuilder.AppendLine($"There are open or completed orders");
                            for (int i = 0; i < orders.Count; i++)
                            {
                                var orderStatus = await Service.GetOrderStatusAsync(orders[i]?.PlacedOrder?.GetOrder()?.Id.ToString());
                                if (orderStatus?.GetOrder()?.Status == "new" || orderStatus?.GetOrder()?.Status == "open")
                                {
                                    Logs.Log.StringBuilder.AppendLine($"Order status is new or open");

                                    var isOrderRemoved = await CheckOpenAndNewOrders(validBids, orderStatus, orders[i], orderBook);
                                }
                                else if (orderStatus?.GetOrder()?.Status == "closed")
                                {
                                    Logs.Log.StringBuilder.AppendLine($"Order status is closed");
                                    var isOrderRemoved = await CheckClosedOrders(validBids, orderStatus, orders[i], orderBook, configs);
                                }
                            }
                        }
                        else
                        {
                            Logs.Log.StringBuilder.AppendLine("there are no Orders(not completed not open)");
                            await OpenNewBidOrder(validBids, orderBook);
                        }
                    }
                    else
                    {
                        if (orders != null)
                        {
                            for (int i = 0; i < orders.Count; i++)
                            {
                                var orderStatus = await Service.GetOrderStatusAsync(orders[i].PlacedOrder.GetOrder().Id.ToString());
                                if (orderStatus?.GetOrder()?.Status == "new" || orderStatus?.GetOrder()?.Status == "open")
                                {
                                    //cancel orders and sell bought cryptos
                                    var isOrderRemoved = await CancelOrderAndSellCryptos(orderStatus, orders[i], orderBook);
                                }
                                else if (orderStatus?.GetOrder()?.Status == "closed")
                                {
                                    validBids = GetValidBidsByGivenSize(orderBook, configs.MinSize * 0.3m);
                                    var isOrderRemoved = await CheckClosedOrders(validBids, orderStatus, orders[i], orderBook, configs);
                                }
                            }
                        }
                    }
                }

                var balances = await Service.GetBalance();
                Logs.Log.StringBuilder.AppendLine("");
                foreach (var balance in balances.Balances)
                {
                    Logs.Log.StringBuilder.AppendLine($"Balance amount-{balance.Free} : Coin-{balance.Coin}");
                }

            }
            catch (Exception ex)
            {
                Logs.Log.StringBuilder.AppendLine($"Exception {ex.Message}, {ex.StackTrace}");
            }
            finally
            {
                Logs.Log.LogInFile();
            }
            _isCompleted = true;
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
        private List<AskBid> GetValidBidsByGivenSize(OrderBookBase orderBook, decimal minSize)
        {
            List<AskBid> orderResults = new List<AskBid>();
            var bids = orderBook.GetAllBids();

            foreach (var bid in bids)
            {
                if (bid.Size >= minSize)
                {
                    orderResults.Add(bid);
                    Logs.Log.StringBuilder.AppendLine($"Valid bid {bid.MarketName} bid Size {bid.Size} and price {bid.Price}");
                }
            }

            return orderResults;
        }

        /// <summary>
        /// return all orders that match the condition of size
        /// </summary>
        /// <param name="orderBook">the order book which is going to be checked</param>
        /// <returns></returns>
        private List<AskBid> GetValidOrdersByGivenSize(OrderBookBase orderBook, decimal minSize)
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
        private async Task<bool> CheckOpenAndNewOrders(List<AskBid> validBids, OrderBase orderStatus, ScalpingModel order, OrderBookBase orderBook)
        {
            Logs.Log.StringBuilder.AppendLine($"Check open and new orders");

            foreach (var bid in validBids)
            {
                if (bid.Size >= order.BigSizeBid.Size * 0.9m && bid.Price == order.BigSizeBid.Price)
                {
                    Logs.Log.StringBuilder.AppendLine($"the big bid size is not changed don't cancel");
                    return false;
                }
                else
                {
                    Logs.Log.StringBuilder.AppendLine($"Cancel order because big bid size is changed");
                    return await CancelOrderAndSellCryptos(orderStatus, order, orderBook);

                }
            }
            return false;
        }

        /// <summary>
        /// Cancel the placed order and sell the filled cryptos
        /// </summary>
        /// <param name="orderStatus">Status of the order</param>
        /// <param name="order">Scalping Order</param>
        /// <param name="orderBook">Current Order book</param>
        private async Task<bool> CancelOrderAndSellCryptos(OrderBase orderStatus, ScalpingModel order, OrderBookBase orderBook)
        {
            //Cancel orders and sell bought cryptos
            Logs.Log.StringBuilder.AppendLine($"Cancel order");
            var canceledOrder = await Service.CancelOrderAsync(orderStatus.GetOrder().Id.ToString());
            if (canceledOrder)
            {
                Logs.Log.StringBuilder.AppendLine($"sell bought cryptos");
                await SellCryptos(orderStatus, order, orderBook);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sell the Cryptos
        /// </summary>
        /// <param name="orderStatus">Status of the order</param>
        /// <param name="order">Scalping Order</param>
        /// <param name="orderBook">Current Order book</param>
        private async Task<bool> SellCryptos(OrderBase orderStatus, ScalpingModel order, OrderBookBase orderBook)
        {
            Orders.Remove(order);
            //Check bought cryptos balance
            var bought = (await Service.GetBalance()).Balances;
            var boughtCrypto = bought.FirstOrDefault(x => orderStatus.GetOrder().Market.ToLower().Contains(x.Coin.ToLower()));
            //Sell the cryptos for best price
            var bestAskPrice = orderBook.GetAllAsks().Min(x => x.Price);
            var placedOrder = await Service.PlaceOrderAsync(order.PlacedOrder.GetOrder().Market, FtxApi.Enums.SideType.sell, Convert.ToDecimal(bestAskPrice), FtxApi.Enums.OrderType.limit, Convert.ToDecimal(boughtCrypto.Free));
            Orders.Add(new ScalpingModel
            {
                PlacedOrder = placedOrder,
                RealPrice = bestAskPrice
            });

            var balances = await Service.GetBalance();
            Logs.Log.StringBuilder.AppendLine("");
            foreach (var balance in balances.Balances)
            {
                Logs.Log.StringBuilder.AppendLine($"Balance amount-{balance.Free} : Coin-{balance.Coin}");
            }
            return true;
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
        private async Task<bool> CheckClosedOrders(List<AskBid> validBids, OrderBase orderStatus, ScalpingModel order, OrderBookBase orderBook, ScalpingMarketsConfig configs)
        {
            Logs.Log.StringBuilder.AppendLine($"Check closed orders");
            if (order.PlacedOrder.GetOrder().Side == FtxApi.Enums.SideType.sell.ToString())
            {
                Logs.Log.StringBuilder.AppendLine($"The crypto was selled successfuly");
                Orders.Remove(order);
                return true;
            }
            foreach (var bid in validBids)
            {
                Logs.Log.StringBuilder.AppendLine($"The crypto was bought successfully");
                if (bid.Size >= order.BigSizeBid.Size * 0.3m && bid.Price == order.BigSizeBid.Price)
                {
                    Logs.Log.StringBuilder.AppendLine($"The Big size bid is not changed make a profitable ask");

                    //Make Ask
                    Orders.Remove(order);
                    var orderBid = new AskBid
                    {
                        MarketName = order.PlacedOrder.GetOrder().Market,
                        OrderType = Enums.OrderType.Bid,
                        Price = order.PlacedOrder.GetOrder().Price,
                        Size = order.PlacedOrder.GetOrder().Size
                    };
                    var bought = (await Service.GetBalance()).Balances;
                    var boughtCrypto = bought.FirstOrDefault(x => order.PlacedOrder.GetOrder().Market.ToLower().Contains(x.Coin.ToLower()));
                    var asks = Helper.CalculateAsks(orderBid, configs.RaisePercent, boughtCrypto.Free);
                    foreach (var ask in asks)
                    {
                        var placedOrder = await Service.PlaceOrderAsync(ask.MarketName, FtxApi.Enums.SideType.sell, Convert.ToDecimal(ask.Price), FtxApi.Enums.OrderType.limit, Convert.ToDecimal(ask.Size));
                        //Get maximum price for bid and minimum price for ask for calculating the real price of crypto
                        var bestBidPrice = orderBook.GetAllBids().Max(x => x.Price);
                        var bestAskPrice = orderBook.GetAllAsks().Min(x => x.Price);
                        Orders.Add(new ScalpingModel
                        {
                            PlacedOrder = placedOrder,
                            RealPrice = Helper.RealPrice(bestBidPrice, bestAskPrice)
                        });
                        var balances = await Service.GetBalance();
                        Logs.Log.StringBuilder.AppendLine("");
                        foreach (var balance in balances.Balances)
                        {
                            Logs.Log.StringBuilder.AppendLine($"Balance amount-{balance.Free} : Coin-{balance.Coin}");
                        }
                    }
                    return true;
                }
            }
            Logs.Log.StringBuilder.AppendLine($"big size order is changed sell the cryptos");

            return await SellCryptos(orderStatus, order, orderBook);
        }

        /// <summary>
        /// Open the Scalping bid order
        /// </summary>
        /// <param name="validBids">The valid bids</param>
        /// <param name="orderBook">Current Order book</param>
        private async Task OpenNewBidOrder(List<AskBid> validBids, OrderBookBase orderBook)
        {
            Logs.Log.StringBuilder.AppendLine($"Open new bid order");

            #region Place Order
            //Get maximum price for bid and minimum price for ask for calculating the real price of crypto
            var bestBidPrice = orderBook.GetAllBids().Max(x => x.Price);
            var bestAskPrice = orderBook.GetAllAsks().Min(x => x.Price);
            //Calculate the Price and Size to bid
            var realPrice = Helper.RealPrice(bestBidPrice, bestAskPrice);
            var priceToBid = Helper.CalculatePriceToBid(validBids, realPrice);
            if (priceToBid == null)
            {
                return;
            }
            if (!OrdersToBid.Contains(priceToBid.Item1))
            {
                Logs.Log.StringBuilder.AppendLine("this bid is not checked to make a order");
                OrdersToBid.Add(priceToBid.Item1);
                return;
            }
            else
            {
                Logs.Log.StringBuilder.AppendLine("this bid is checked make an order");
                OrdersToBid.Remove(priceToBid.Item1);
            }
            var balances = (await Service.GetBalance()).Balances;
            var balance = balances.FirstOrDefault(x => x.Coin.ToLower() == "usd").Free;
            var amountToBid = Helper.AmountToBid(balance, priceToBid.Item2);
            //Place an order
            var placedOrder = await Service.PlaceOrderAsync(priceToBid.Item1.MarketName, FtxApi.Enums.SideType.buy, Convert.ToDecimal(priceToBid.Item2.Price), FtxApi.Enums.OrderType.limit, Convert.ToDecimal(amountToBid.Size));
            Logs.Log.StringBuilder.AppendLine($"Placed an order on {placedOrder.GetOrder().Market}");

            var allBalances = await Service.GetBalance();
            Logs.Log.StringBuilder.AppendLine("");
            foreach (var b in allBalances.Balances)
            {
                Logs.Log.StringBuilder.AppendLine($"Balance amount-{b.Free} : Coin-{b.Coin}");
            }

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
