using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamMarketViewer.Market
{
	public class PriceHistoryJson
	{
		public bool success { get; set; }
		public string price_prefix { get; set; }
		public string price_suffix { get; set; }
		public object[][] prices { get; set; }
	}
}
