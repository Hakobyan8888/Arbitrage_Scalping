using Arbitrage.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageDesktopApp.ArbitrageWrappers
{
    public class ArbitrageWrapper
    {
        public ArbitrageWrapper()
        {
        }

        private static readonly object lockObject = new object();
        private static ArbitrageViewModelBase _instance = null;
        public static ArbitrageViewModelBase FTXViewModelBaseInstance
        {
            get
            {
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new FTXViewModel();
                    }
                    return _instance;
                }
            }
        }
    }
}
