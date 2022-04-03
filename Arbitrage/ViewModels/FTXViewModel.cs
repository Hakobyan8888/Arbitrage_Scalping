using Arbitrage.Services;
using Arbitrage.Tests;
using Arbitrage.Utils;
using FtxApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrage.ViewModels
{
    public class FTXViewModel : ViewModelBase
    {
        private FTXService FTXService { get; set; }
        public override void Start()
        {
            Initialize();
            base.Start();
        }

        private void Initialize()
        {
            var client = new Client(Constants.API_KEY, Constants.API_SECRET);
            FTXService = new FTXService(client);
            Service = new Exchange();
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}
