using System;
using System.Collections.Generic;
using System.Text;

namespace Arbitrage.Models
{
    public abstract class OrderBase
    {
        public abstract FTXModels.FTXOrder GetOrder();
    }
}
