using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace SteamMarketViewer.Market
{
	public static class Analytics
	{
		public static void Analyze(this Item item)
		{
			StreamWriter sw = new StreamWriter("data.txt", false);
			
			for (int i = 0; i < item.Histogram.Dates.Count; i++)
			{
				if (item.Histogram.Dates[i].Date >= DateTime.Today.Subtract(TimeSpan.FromDays(7)))
				{
					sw.Write(i+"-");
				}
			}
			sw.WriteLine();
			for (int i = 0; i < item.Histogram.Dates.Count; i++)
			{
				if (item.Histogram.Dates[i].Date >= DateTime.Today.Subtract(TimeSpan.FromDays(7)))
				{
					sw.Write(item.Histogram.Prices[i] + "-");
				}
			}
			sw?.Close();
		}
	}
}
