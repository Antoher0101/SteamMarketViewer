using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SteamMarketViewer.WebClient;

namespace SteamMarketViewer.WebClient
{
	public class SteamSettings : IWebSettings
	{
		/// <summary>
		/// {render} 0 - html, 1 - json
		/// {start} - first item id
		/// {count} - count of items on page
		/// {appid} 730 - csgo, 570 - dota2
		/// </summary>
		public SteamSettings(int start = 0, int count = 10, string query = "", int norender = 1, Game app = Game.CSGO)
		{
			// string sort = "price";
			// string sort_dir = "asc";
			string sort = "quantity";
			string sort_dir = "desc";
			string appid = "";
			switch (app)
			{
				case Game.CSGO: appid = "730"; break;
				case Game.Dota2: appid = "570"; break;
			}
			string addr =
				"https://steamcommunity.com/market/search/render/?query={query}&norender={render}&start={start}&count={count}&search_descriptions=0&sort_column=price&sort_dir=asc&appid={appid}&language=russian";
			URI = addr
				.Replace("{query}", query)
				.Replace("{render}", norender.ToString())
				.Replace("{start}", start.ToString())
				.Replace("{count}", count.ToString())
				.Replace("{appid}", appid);
		}

		public string URI { get; set; }

		bool IWebSettings.NoCachePolicy { get; set; } = false;
		bool IWebSettings.AcceptGZipEncoding { get; set; } = true;
		bool IWebSettings.UseUnsafeHeaderParsing { get; set; }
		string IWebSettings.Address { get; set; } = "";
		string IWebSettings.Accept { get; set; } = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
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
