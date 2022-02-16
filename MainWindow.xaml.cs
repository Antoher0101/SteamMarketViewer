using ScottPlot;
using SteamMarketViewer.Market;
using SteamMarketViewer.SteamAuth;
using SteamMarketViewer.WebClient;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
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

			SteamControl.PageSize = 50;
			SteamControl.CurrentPage = 1;

			CommandBinding bind = new CommandBinding(MediaCommands.Select);
			bind.Executed += ChangeGame;
			this.CommandBindings.Add(bind);
		}

		private void Calc_Click(object sender, RoutedEventArgs e)
		{
			Calculator calc = new Calculator();
			calc.Show();
		}

		private async void LoadItemsPage(object sender, RoutedEventArgs e)
		{
			string q = ItemSearchField.Text == "Поиск" ? "" : ItemSearchField.Text;
            string page = PageSelectBox.Text == "" ? "" : PageSelectBox.Text;
			SteamSettings ss;
			page = Regex.Replace(page, "[^0-9]+", string.Empty);
			PageSelectBox.Text = page;
			if (page != string.Empty)
            {
				SteamControl.CurrentPage = Int32.Parse(page);
            }
			if (q != string.Empty)
			{
				ss = new SteamSettings(0, SteamControl.PageSize, q, 1, SteamControl.ChoosedGame);
			}
			else ss = new SteamSettings((SteamControl.CurrentPage-1)*SteamControl.PageSize, SteamControl.PageSize, null, 1, SteamControl.ChoosedGame);

			SteamControl.ItemList.Clear();
			SteamControl.Loading = true;
			MarketJson jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<MarketJson>(await SteamWeb.RequestAsync(ss.URI, "GET", data: null, SteamControl.LoginCookies));

			
			SteamControl.TotalResults = jsonResponse.total_count;
			SteamControl.SearchResultsStart = jsonResponse.start+1;
			SteamControl.SearchResultsEnd = jsonResponse.start+jsonResponse.pagesize<SteamControl.TotalResults? jsonResponse.start + jsonResponse.pagesize : SteamControl.TotalResults;
			SteamControl.PageCount = (SteamControl.TotalResults + SteamControl.PageSize) / SteamControl.PageSize;
			SteamControl.CanPrevPage = SteamControl.CurrentPage > 1;
			SteamControl.CanNextPage = SteamControl.CurrentPage + 1 <= SteamControl.PageCount;

			SteamControl.GeneratePageList();
			GenerateItemList(jsonResponse);
			
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
                PriceList.Plot.Clear();
				PriceList.Plot.Palette = Palette.PolarNight;
                double[] values = item.Analyze();
			
                PriceList.Plot.Clear();
				PriceList.Plot.AddSignal(values);
                PriceList.Refresh();

                popup1.IsOpen = false;
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

		private void Analyzator(object sender, RoutedEventArgs e)
        {
            
        }

		private void PrevPage_Click(object sender, RoutedEventArgs e)
		{
			SteamControl.CurrentPage--;
			LoadItemsPage(sender, e);
		}

		private void NextPage_Click(object sender, RoutedEventArgs e)
		{
			SteamControl.CurrentPage++;
			LoadItemsPage(sender, e);
		}
		private void SelectPage(object sender, MouseButtonEventArgs e)
		{
			var text = sender as TextBlock;
			if (text != null)
			{
				int page;
				if (int.TryParse(text.Text, out page))
				{
					SteamControl.CurrentPage = page;
					LoadItemsPage(sender, e);
				}
			}
		}
	}
	public enum Game
	{
		Dota2, CSGO
	}
}


