using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using SteamMarketViewer.WebClient;

namespace SteamMarketViewer.Market
{
	public class Item : INotifyPropertyChanged
	{
		public string Name { get; set; }
		public string HashName { get; set; }
		public int SellListings { get; set; }
		public double SellPrice { get; set; }
		public string SellPriceText { get; set; }
		public string GameName { get; set; }
		public string SalePrice { get; set; }
		public int Appid { get; set; }
		public string Link { get; set; }
		public string Preview { get; set; }

		public ItemOrderHistogram Histogram { get; set; } = new ItemOrderHistogram();

		public Item()
		{
		}

		public async void GetPriceHistory()
		{
			await Task.Run(() =>
			{
				try
				{
					PriceHistoryJson priceHistoryJson = Newtonsoft.Json.JsonConvert.DeserializeObject<PriceHistoryJson>(
						SteamAuth.SteamWeb.Request("https://steamcommunity.com/market/pricehistory/", "GET",
							new NameValueCollection()
							{
								{ "currency", "5" },
								{ "appid", Appid.ToString() },
								{ "market_hash_name", HashName }
							}, MainWindow.SteamControl.LoginCookies)
					);
					if (priceHistoryJson.success)
					{
						CreateHistogram(priceHistoryJson);
						Ready = true;
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}
			});
		}
		public void CreateHistogram(PriceHistoryJson jsonData)
		{
			for (int i = 0; i < jsonData.prices.Length; i++)
			{
				Histogram.Dates.Add(DateTime.ParseExact(jsonData.prices[i][0].ToString(), "MMM dd yyyy HH: z", CultureInfo.InvariantCulture));
				Histogram.Prices.Add(double.Parse(jsonData.prices[i][1].ToString(), NumberStyles.AllowDecimalPoint));
				Histogram.Sold.Add(int.Parse(jsonData.prices[i][2].ToString()));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		private bool _show;
		private bool _ready = false;

		public bool Show
		{
			get => _show;
			set
			{
				_show = value;
				NotifyPropertyChanged(nameof(Show));
			}
		}

		public bool Ready
		{
			get => _ready;
			set
			{
				_ready = value;
				NotifyPropertyChanged(nameof(Ready));
			}
		}
	}
}
