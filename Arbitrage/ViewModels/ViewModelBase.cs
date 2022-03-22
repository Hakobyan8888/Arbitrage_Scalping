using Arbitrage.Models;
using Arbitrage.Services;
using Arbitrage.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Arbitrage.ViewModels
{
    public abstract class ViewModelBase
    {
        protected ServiceBase Service { get; set; }
        private Timer _timer;
        public virtual void Start()
        {
            _timer = new Timer();
            _timer.Interval = 2500;
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = true;
            _timer.Start();
        }
        public virtual void Stop()
        {
            _timer.Elapsed -= Timer_Elapsed;
            _timer?.Stop();
            _timer = null;
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var btcOrderBook = await Service.GetOrderBookAsync(FTXMarkets.FUTURE_BTC_USD, Constants.ORDER_BOOK_DEPTH);
            var solanaOrderBook = await Service.GetOrderBookAsync(FTXMarkets.FUTURE_SOLANA_USD, Constants.ORDER_BOOK_DEPTH);
            var avaxOrderBook = await Service.GetOrderBookAsync(FTXMarkets.FUTURE_AVAX_USD, Constants.ORDER_BOOK_DEPTH);
            var atomOrderBook = await Service.GetOrderBookAsync(FTXMarkets.FUTURE_ATOM_USD, Constants.ORDER_BOOK_DEPTH);

            var btcValidOrder = GetValidBidsByGivenSize(btcOrderBook, Constants.MIN_BTC_SIZE);
            var solValidOrder = GetValidBidsByGivenSize(solanaOrderBook, Constants.MIN_SOL_SIZE);
            var avaxValidOrder = GetValidBidsByGivenSize(avaxOrderBook, Constants.MIN_AVAX_SIZE);
            var atomValidOrder = GetValidBidsByGivenSize(atomOrderBook, Constants.MIN_ATOM_SIZE);

            //Check if the valid orders are bots


            var bestBidPrice = btcOrderBook.GetAllBids().Max(x => x.Price);
            var bestAskPrice = btcOrderBook.GetAllAsks().Min(x => x.Price);

            //Calculate the Price to bid
            var priceToBid = Helper.CalculatePriceToBid(btcValidOrder, bestBidPrice, bestAskPrice);

            #region Telegram Messages
            foreach (var item in btcValidOrder)
            {
                await Program._telegramBotClient.SendTextMessageAsync(Program.ChatId, $"Market Name - {item.MarketName}\n Order Type - {item.OrderType} \n Size - {item.Size} \n Price - {item.Price}");
                Console.WriteLine($"{item.MarketName} {item.OrderType} : {item.Size} --- {item.Price}");
            }
            foreach (var item in solValidOrder)
            {
                await Program._telegramBotClient.SendTextMessageAsync(Program.ChatId, $"Market Name - {item.MarketName}\n Order Type - {item.OrderType} \n Size - {item.Size} \n Price - {item.Price}");
                Console.WriteLine($"{item.MarketName} {item.OrderType} : {item.Size} --- {item.Price}");
            }
            foreach (var item in avaxValidOrder)
            {
                await Program._telegramBotClient.SendTextMessageAsync(Program.ChatId, $"Market Name - {item.MarketName}\n Order Type - {item.OrderType} \n Size - {item.Size} \n Price - {item.Price}");
                Console.WriteLine($"{item.MarketName} {item.OrderType} : {item.Size} --- {item.Price}");
            }
            foreach (var item in atomValidOrder)
            {
                await Program._telegramBotClient.SendTextMessageAsync(Program.ChatId, $"Market Name - {item.MarketName}\n Order Type - {item.OrderType} \n Size - {item.Size} \n Price - {item.Price}");
                Console.WriteLine($"{item.MarketName} {item.OrderType} : {item.Size} --- {item.Price}");
            }
            #endregion
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
    }
}
