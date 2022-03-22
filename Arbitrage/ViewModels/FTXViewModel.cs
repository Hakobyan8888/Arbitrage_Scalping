using Arbitrage.Services;
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
        public override void Start()
        {
            Initialize();
            base.Start();
        }

        private void Initialize()
        {
            var client = new Client(Constants.API_KEY, Constants.API_SECRET);
            Service = new FTXService(client);
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}
