using ScottPlot;
using SteamMarketViewer.Market;
using SteamMarketViewer.SteamAuth;
using SteamMarketViewer.WebClient;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Palette = ScottPlot.Drawing.Palette;

namespace SteamMarketViewer
{
	public partial class MainWindow : Window
	{
		public static SteamControl SteamControl { get; set; } = new SteamControl();

		public MainWindow()
		{
			InitializeComponent();

			this.DataContext = SteamControl;
			SteamControl.CheckLoggined();

			CommandBinding bind = new CommandBinding(MediaCommands.Select);
			bind.Executed += ChangeGame;
			this.CommandBindings.Add(bind);
			

		}

		private void Calc_Click(object sender, RoutedEventArgs e)
		{
			Calculator calc = new Calculator();
			calc.Show();
		}

		private async void StartBtn_Click(object sender, RoutedEventArgs e)
		{
			string q = ItemSearchField.Text == "Поиск" ? null : ItemSearchField.Text;
			SteamControl.ItemList.Clear();
			SteamControl.Loading = true;
			MarketJson sj = Newtonsoft.Json.JsonConvert.DeserializeObject<MarketJson>(await SteamWeb.RequestAsync(new SteamSettings(0,20, q, 1, SteamControl.ChoosedGame).URI
				, "GET", data: null, SteamControl.LoginCookies));
			GenerateItemList(sj);
			SteamControl.Loading = false;

		}
		private void GenerateItemList(MarketJson json)
		{
			for (int i = 0; i < json.results.Length; i++)
			{
				SteamControl.ItemList.Add(new Item()
				{
					Name = json.results[i].name,
					HashName = json.results[i].hash_name,
					SalePrice = json.results[i].sale_price_text,
					SellListings = json.results[i].sell_listings,
					SellPrice = json.results[i].sell_price / 100.0,
					SellPriceText = json.results[i].sell_price_text,
					Appid = json.results[i].asset_description.appid,
					Link = "https://steamcommunity.com/market/listings/730/" + json.results[i].hash_name.Replace(" ", "%20"),
					GameName = json.results[i].app_name,
					Show = true,
					Preview = "https://community.akamai.steamstatic.com/economy/image/" + json.results[i].asset_description.icon_url
				});
				SteamControl.ItemList[i].GetPriceHistory();
			}
		}

		private void Steam_LogIn_Click(object sender, RoutedEventArgs e)
		{
			Login login = new Login();
			login.Owner = this;
			login.Show();
		}
		private void Steam_LogOut_Click(object sender, RoutedEventArgs e)
		{
			Util.WriteCookiesToDisk("cookies.txt", new CookieContainer());
			SteamControl.LoginCookies = null;
			SteamControl.IsLoggined = false;
		}

		private void ItemList_OnMouseUp(object sender, MouseButtonEventArgs e)
		{
			var item = ((FrameworkElement)e.OriginalSource).DataContext as Item;
			if (item != null)
			{
				PriceList.Plot.Palette = Palette.OneHalfDark;
				int pointCount = item.Histogram.Dates.Count;
				double[] dates = item.Histogram.Dates.Select(x => x.ToOADate()).ToArray();

				double[] values = new double[pointCount];
				Random rand = new Random(0);
				for (int i = 1; i < pointCount; i++)
					values[i] = item.Histogram.Prices[i];
				PriceList.Plot.Clear();
				PriceList.Plot.AddScatter(dates, values, null, 1f, 2f, MarkerShape.filledCircle);
				PriceList.Plot.XAxis.DateTimeFormat(true);
				PriceList.Refresh();


				//popup1.IsOpen = true;
				//Console.WriteLine(item.HashName);
			}
		}


		private void RemoveText(object sender, RoutedEventArgs e)
		{
			if (ItemSearchField.Text == "Поиск")
			{
				ItemSearchField.Text = "";
			}
		}

		private void AddText(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(ItemSearchField.Text))
				ItemSearchField.Text = "Поиск";
		}

		private void ChangeGame(object sender, ExecutedRoutedEventArgs e)
		{
			SteamControl.ChoosedGame = (Game)e.Parameter;
		}
	}
	public enum Game
	{
		Dota2, CSGO
	}
}


