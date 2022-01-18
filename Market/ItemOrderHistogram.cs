using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamMarketViewer.Market
{
	public class ItemOrderHistogram
	{
		public List<DateTime> Dates { get; private set; } = new List<DateTime>();
		public List<double> Prices { get; private set; } = new List<double>();
		public List<int> Sold { get; private set; } = new List<int>();
	}
}
