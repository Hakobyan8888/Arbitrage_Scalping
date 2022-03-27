using Arbitrage.Utils;
using Arbitrage.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace Arbitrage
{
    class Program
    {
        public static readonly TelegramBotClient _telegramBotClient = new TelegramBotClient("5288604151:AAG1djNRWJXIraVCcFJJ1BNW5AHu0FpbQ3c");
        private static FTXViewModel _ftxViewModel = new FTXViewModel();
        public static ChatId ChatId;

        [Obsolete]
        static void Main(string[] args)
        {
            //using (StreamReader r = new StreamReader("Assets/Jsons/ScalpingMarketsConfigs.json"))
            //{
            //    string json = r.ReadToEnd();
            //    List<MarketsScalpingConfig> items = JsonConvert.DeserializeObject<List<MarketsScalpingConfig>>(json);
            //}

            //_telegramBotClient.OnMessage += _telegramBotClient_OnMessage;
            //_telegramBotClient.OnMessageEdited += _telegramBotClient_OnMessage;
            //_telegramBotClient.StartReceiving();
            _ftxViewModel.Start();
            Console.Read();
            //_telegramBotClient.StopReceiving();
        }

        [Obsolete]
        private static async void _telegramBotClient_OnMessage(object sender, MessageEventArgs e)
        {
            ChatId = e.Message.Chat.Id;
            try
            {
                if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                {
                    if (e.Message.Text.ToLower().Contains("btcsizechange"))
                    {
                        double.TryParse(e.Message.Text.Split(" ")[1], out Constants.MIN_BTC_SIZE);
                        await _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, "successfuly changed the size of BTC");
                    }
                    else if (e.Message.Text.ToLower().Contains("solsizechange"))
                    {
                        double.TryParse(e.Message.Text.Split(" ")[1], out Constants.MIN_SOL_SIZE);
                        await _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, "successfuly changed the size of Sol");
                    }
                    else if (e.Message.Text.ToLower().Contains("avaxsizechange"))
                    {
                        double.TryParse(e.Message.Text.Split(" ")[1], out Constants.MIN_AVAX_SIZE);
                        await _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, "successfuly changed the size of Avax");
                    }
                    else if (e.Message.Text.ToLower().Contains("atomsizechange"))
                    {
                        double.TryParse(e.Message.Text.Split(" ")[1], out Constants.MIN_ATOM_SIZE);
                        await _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, "successfuly changed the size of Atom");
                    }
                    else if (e.Message.Text.ToLower().Contains("startapp"))
                    {
                        _ftxViewModel = new FTXViewModel();
                        _ftxViewModel.Start();
                        await _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, "started the app");
                    }
                    else if (e.Message.Text.ToLower().Contains("stop"))
                    {
                        _ftxViewModel?.Stop();
                        await _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, "stoped");
                        _ftxViewModel = null;
                    }
                    else
                    {
                        await _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, @"Usage :
                        /BtcSizeChange 200
/SolSizeChange 15000
/AvaxSizeChange 2000
/AtomSizeChange 15000
/StartApp
/Stop
/help"
      );
                    }
                }
            }
            catch (Exception ex)
            {
                await _telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, @"please look how to use the app in /help");
            }
        }
    }
}
