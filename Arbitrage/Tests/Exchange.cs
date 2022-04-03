using Arbitrage.Models;
using Arbitrage.Models.FTXModels;
using Arbitrage.Services;
using Arbitrage.Utils;
using FtxApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrage.Tests
{
    public class Exchange : ServiceBase
    {
        public decimal USD
        {
            get;
            set;
        } = 10000;
        public decimal BTC
        {
            get;
            set;
        }
        public decimal SOL
        {
            get;
            set;
        }
        public decimal ATOM
        {
            get;
            set;
        }
        public decimal AVAX
        {
            get;
            set;
        }
        public decimal ZIL
        {
            get;
            set;
        }
        public decimal LUNA
        {
            get;
            set;
        }
        public decimal GMT
        {
            get;
            set;
        }
        public decimal WAVES
        {
            get;
            set;
        }
        public List<OrderBase> Orders { get; set; }
        public List<OrderBase> CompletedOrders { get; set; }
        public OrderBookBase OrderBook { get; set; }

        public Exchange()
        {
            Orders = new List<OrderBase>();
            CompletedOrders = new List<OrderBase>();
        }
        public override Task<bool> CancelOrderAsync(string orderId)
        {
            foreach (var order in Orders)
            {
                if (order.GetOrder().Id.ToString() == orderId)
                {
                    if (order.GetOrder().Side == SideType.buy.ToString())
                    {
                        USD += order.GetOrder().Price * order.GetOrder().Size;
                    }
                    else
                    {
                        if (order.GetOrder().Market == FTXMarkets.FUTURE_BTC_USD)
                        {
                            BTC += order.GetOrder().Size;
                        }
                        else if (order.GetOrder().Market == FTXMarkets.FUTURE_SOLANA_USD)
                        {
                            SOL += order.GetOrder().Size;
                        }
                        else if (order.GetOrder().Market == FTXMarkets.FUTURE_ATOM_USD)
                        {
                            ATOM += order.GetOrder().Size;
                        }
                        else if (order.GetOrder().Market == FTXMarkets.FUTURE_AVAX_USD)
                        {
                            AVAX += order.GetOrder().Size;
                        }
                        else if (order.GetOrder().Market == FTXMarkets.FUTURE_ZIL_USD)
                        {
                            ZIL += order.GetOrder().Size;
                        }
                        else if (order.GetOrder().Market == FTXMarkets.FUTURE_LUNA_USD)
                        {
                            LUNA += order.GetOrder().Size;
                        }
                        else if (order.GetOrder().Market == FTXMarkets.FUTURE_GMT_USD)
                        {
                            GMT += order.GetOrder().Size;
                        }
                        else if (order.GetOrder().Market == FTXMarkets.FUTURE_WAVES_USD)
                        {
                            WAVES += order.GetOrder().Size;
                        }
                    }


                    Logs.Log.StringBuilder.AppendLine($"Canceled {order.GetOrder().Side} order on {order.GetOrder().Market}; Price - {order.GetOrder().Price}; Size - {order.GetOrder().Size}");
                    Orders.Remove(order);
                    return Task.FromResult(true);
                }
            }
            return Task.FromResult(false);
        }

        public bool CheckIfOrderIsCompleted(OrderBookBase orderBook, AskBid askBid)
        {
            var asks = orderBook.GetAllAsks();
            var bids = orderBook.GetAllBids();

            var bestBidPrice = orderBook.GetAllBids().Max(x => x.Price);
            var bestAskPrice = orderBook.GetAllAsks().Min(x => x.Price);
            var price = Helper.RealPrice(bestBidPrice, bestAskPrice);
            if ((askBid.OrderType == Enums.OrderType.Ask && askBid.Price >= price) || (askBid.OrderType == Enums.OrderType.Bid && askBid.Price <= price))
            {
                if (askBid.OrderType == Enums.OrderType.Bid)
                {
                    if (askBid.MarketName == FTXMarkets.FUTURE_BTC_USD)
                    {
                        BTC += askBid.Size * 0.993m;
                    }
                    else if (askBid.MarketName == FTXMarkets.FUTURE_SOLANA_USD)
                    {
                        SOL += askBid.Size * 0.993m;
                    }
                    else if (askBid.MarketName == FTXMarkets.FUTURE_ATOM_USD)
                    {
                        ATOM += askBid.Size * 0.993m;
                    }
                    else if (askBid.MarketName == FTXMarkets.FUTURE_AVAX_USD)
                    {
                        AVAX += askBid.Size * 0.993m;
                    }
                    else if (askBid.MarketName == FTXMarkets.FUTURE_ZIL_USD)
                    {
                        ZIL += askBid.Size * 0.993m;
                    }
                    else if (askBid.MarketName == FTXMarkets.FUTURE_LUNA_USD)
                    {
                        LUNA += askBid.Size * 0.993m;
                    }
                    else if (askBid.MarketName == FTXMarkets.FUTURE_GMT_USD)
                    {
                        GMT += askBid.Size * 0.993m;
                    }
                    else if (askBid.MarketName == FTXMarkets.FUTURE_WAVES_USD)
                    {
                        WAVES += askBid.Size * 0.993m;
                    }
                }
                else
                {
                    USD += askBid.Size * askBid.Price * 0.993m;
                }
                Logs.Log.StringBuilder.AppendLine($"Order is completed {askBid.OrderType} order on {askBid.MarketName}; Price - {askBid.Price}; Size - {askBid.Size}");
                foreach (var balance in GetBalance().Result.Balances)
                {
                    Logs.Log.StringBuilder.AppendLine($"Balance amount-{balance.Free} : Coin-{balance.Coin}");
                }
                return true;
            }
            return false;
        }

        public override Task<Wallet> GetBalance()
        {
            var balances = new List<Balance>()
            {
                new Balance
                {
                    Free = USD,
                    Coin = "USD"
                },
                new Balance
                {
                    Free = BTC,
                    Coin = "btc"
                },
                new Balance
                {
                    Free = SOL,
                    Coin = "sol"
                },
                new Balance
                {
                    Free = AVAX,
                    Coin = "avax"
                },
                new Balance
                {
                    Free = ATOM,
                    Coin = "atom"
                },
                new Balance
                {
                    Free = ZIL,
                    Coin = "zil"
                },
                new Balance
                {
                    Free = LUNA,
                    Coin = "luna"
                },
                new Balance
                {
                    Free = GMT,
                    Coin = "gmt"
                },
                new Balance
                {
                    Free = WAVES,
                    Coin = "waves"
                },
            };
            var wallet = new Wallet
            {
                Balances = balances,
            };
            return Task.FromResult(wallet);
        }

        public override Task<OrderBase> PlaceOrderAsync(string instrument, SideType side, decimal price, OrderType orderType, decimal amount, bool reduceOnly = false)
        {
            var random = new Random();

            var orderBase = new FTXOrderResponse
            {
                Order = new FTXOrder
                {
                    Id = random.Next(int.MinValue, int.MaxValue),
                    Market = instrument,
                    Price = price,
                    Side = side.ToString(),
                    Size = amount,
                    Status = "new"
                },
                Success = true
            };
            if (side == SideType.buy)
            {
                USD -= price * amount;
            }
            else
            {
                if (instrument == FTXMarkets.FUTURE_BTC_USD)
                {
                    BTC -= amount;
                }
                else if (instrument == FTXMarkets.FUTURE_SOLANA_USD)
                {
                    SOL -= amount;
                }
                else if (instrument == FTXMarkets.FUTURE_ATOM_USD)
                {
                    ATOM -= amount;
                }
                else if (instrument == FTXMarkets.FUTURE_AVAX_USD)
                {
                    AVAX -= amount;
                }
                else if (instrument == FTXMarkets.FUTURE_ZIL_USD)
                {
                    ZIL -= amount;
                }
                else if (instrument == FTXMarkets.FUTURE_LUNA_USD)
                {
                    LUNA -= amount;
                }
                else if (instrument == FTXMarkets.FUTURE_GMT_USD)
                {
                    GMT -= amount;
                }
                else if (instrument == FTXMarkets.FUTURE_WAVES_USD)
                {
                    WAVES -= amount;
                }
            }

            Logs.Log.StringBuilder.AppendLine($"Placed {orderBase.Order.Side} order on {orderBase.Order.Market}; Price - {orderBase.Order.Price}; Size - {orderBase.Order.Size} ");
            Orders.Add(orderBase);
            return Task.FromResult(orderBase as OrderBase);
        }

        public override async Task<OrderBookBase> GetOrderBookAsync(string marketName, int depth = 20)
        {
            OrderBook = await FTXService.FtxService.GetOrderBookAsync(marketName, depth);
            return OrderBook;
        }

        public override Task<OrderBase> GetOrderStatusAsync(string id)
        {
            foreach (var order in Orders)
            {
                if (order.GetOrder().Id.ToString() == id)
                {
                    var askBid = new AskBid
                    {
                        MarketName = order.GetOrder().Market,
                        OrderType = order.GetOrder().Side == SideType.buy.ToString() ? Enums.OrderType.Bid : Enums.OrderType.Ask,
                        Price = order.GetOrder().Price,
                        Size = order.GetOrder().Size,
                    };
                    var isCompleted = CheckIfOrderIsCompleted(OrderBook, askBid);
                    order.GetOrder().Status = isCompleted ? "closed" : "open";
                    if (isCompleted)
                    {
                        Orders.Remove(order);
                        CompletedOrders.Add(order);
                    }
                    return Task.FromResult(order);
                }
            }
            foreach (var order in CompletedOrders)
            {
                if (order.GetOrder().Id.ToString() == id)
                {
                    return Task.FromResult(order);
                }
            }
            return Task.FromResult(new FTXOrderResponse() as OrderBase);
        }

        public override Task<OrderBase> ModifyOrderAsync(string orderId, decimal size, decimal triggerPrice)
        {
            throw new NotImplementedException();
        }

        public override Task<OrderBase> ModifyOrderPriceAsync(string orderId, decimal triggerPrice)
        {
            throw new NotImplementedException();
        }

        public override Task<OrderBase> ModifyOrderSizeAsync(string orderId, decimal size)
        {
            throw new NotImplementedException();
        }
    }
}
