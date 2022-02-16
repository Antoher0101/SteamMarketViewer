using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;

namespace SteamMarketViewer.Market
{
	public static class Analytics
	{
		public static double[] Analyze(this Item item)
        {
            List<int> indexes_p1 = new List<int>();
            List<double> cffs = new List<double>();
            double max_relev = 0;
            int max_i = 0;
            double[] p;
            for (int i = 0; i < item.Histogram.Dates.Count; i++)
            {
                if (item.Histogram.Dates[i].Date >= DateTime.Now.Subtract(TimeSpan.FromDays(14)))
                {
                    indexes_p1.Add(i);
                }
            }
            StreamWriter sw = new StreamWriter("data.txt", false);
            for (int i = 0; i < indexes_p1.Count; i++)
            {
	            cffs.Add(item.Histogram.Prices[indexes_p1[i]] * item.Histogram.Sold[indexes_p1[i]]);
	            if (cffs[i] > max_relev)
	            {
		            max_relev = cffs[i];
		            max_i = i;
	            }
                sw.Write(cffs[i]);
            }
            sw.WriteLine();
            p = new double[indexes_p1.Count];
            for (int i = 0; i < indexes_p1.Count; i++)
            {
                p[i] = item.Histogram.Prices[indexes_p1[i]];
                sw.Write($"{item.Histogram.Prices[indexes_p1[i]]};");
            }
            sw.Close();
            Console.WriteLine("Sell for " + item.Histogram.Prices[indexes_p1[max_i]]);
            return p;
        }
    }
}