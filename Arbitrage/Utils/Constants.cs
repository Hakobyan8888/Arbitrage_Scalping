using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrage.Utils
{
    //API KEY - 7ClGMaKOnJK0JPuj1v6ArXP6RZ4errs3OY5p7sLJ
    //API Secret - L1qtM_29qWwAvsWR9QV1jFbsbaefCRmzyDn34cRw
    public class Constants
    {
        public const string API_KEY = "7ClGMaKOnJK0JPuj1v6ArXP6RZ4errs3OY5p7sLJ";
        public const string API_SECRET = "L1qtM_29qWwAvsWR9QV1jFbsbaefCRmzyDn34cRw";

        public const int ORDER_BOOK_DEPTH = 80;

        public static double MIN_BTC_SIZE = 300;
        public static double MIN_SOL_SIZE = 20000;
        public static double MIN_AVAX_SIZE = 15000;
        public static double MIN_ATOM_SIZE = 20000;
    }
}
