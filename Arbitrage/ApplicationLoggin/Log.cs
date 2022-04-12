using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Arbitrage.Logs
{
    public static class Log
    {
        public static StringBuilder StringBuilder = new StringBuilder();
        private static string path = @"C:\Users\hakobyan\source\repos\Arbitrage\Arbitrage\Logs.txt";
        public static void LogInFile()
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.Write(StringBuilder);
            }
            StringBuilder.Clear();
        }
    }
}
