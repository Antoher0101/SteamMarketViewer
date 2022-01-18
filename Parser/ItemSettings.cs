using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SteamMarketViewer.WebClient
{
	class ItemSettings : IWebSettings
	{
		public ItemSettings(Dictionary<string, string> dict)
		{
			var url = string.Format("https://steamcommunity.com/market/pricehistory?{0}",
				string.Join("&",
					dict.Select(kvp =>
						string.Format("{0}={1}", kvp.Key, kvp.Value))));
			URI = url;
		}

		public string URI { get; set; }

		bool IWebSettings.NoCachePolicy { get; set; } = false;
		bool IWebSettings.AcceptGZipEncoding { get; set; } = true;
		bool IWebSettings.UseUnsafeHeaderParsing { get; set; }
		string IWebSettings.Address { get; set; } = "";

		string IWebSettings.Accept { get; set; } =
			"text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";

		string IWebSettings.Referer { get; set; }
		string IWebSettings.Host { get; set; } = "steamcommunity.com";
		bool? IWebSettings.KeepAlive { get; set; } = true;
		string IWebSettings.ContentType { get; set; } = "application/json; charset=utf-8";
		bool? IWebSettings.Expect100Continue { get; set; }
		bool? IWebSettings.AllowAutoRedirect { get; set; } = false;

		string IWebSettings.UserAgent { get; set; } =
			"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.85 YaBrowser/21.11.2.773 Yowser/2.5 Safari/537.36";

		WebProxy IWebSettings.Proxy { get; set; }
		bool IWebSettings.TurnOffProxy { get; set; }
		int IWebSettings.TimeOut { get; set; }
	}
}
