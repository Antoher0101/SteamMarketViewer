using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamMarketViewer.Parser;

namespace SteamMarketViewer.Market
{
	public class MarketJson
	{
		[Newtonsoft.Json.JsonProperty("success")]
		public bool success { get; set; }
		[Newtonsoft.Json.JsonProperty("start")]
		public int start { get; set; }
		[Newtonsoft.Json.JsonProperty("pagesize")]
		public int pagesize { get; set; }
		[Newtonsoft.Json.JsonProperty("total_count")]
		public int total_count { get; set; }
		[Newtonsoft.Json.JsonProperty("searchdata")]
		public Searchdata searchdata { get; set; }
		[Newtonsoft.Json.JsonProperty("results")]
		public Result[] results { get; set; }
	}

	public class Searchdata
	{
		[Newtonsoft.Json.JsonProperty("query")]
		public string query { get; set; }
		[Newtonsoft.Json.JsonProperty("search_descriptions")]
		public bool search_descriptions { get; set; }
		[Newtonsoft.Json.JsonProperty("total_count")]
		public int total_count { get; set; }
		[Newtonsoft.Json.JsonProperty("pagesize")]
		public int pagesize { get; set; }
		[Newtonsoft.Json.JsonProperty("prefix")]
		public string prefix { get; set; }
		[Newtonsoft.Json.JsonProperty("class_prefix")]
		public string class_prefix { get; set; }
	}

	public class Result
	{
		[Newtonsoft.Json.JsonProperty("name")]
		public string name { get; set; }
		[Newtonsoft.Json.JsonProperty("hash_name")]
		public string hash_name { get; set; }
		[Newtonsoft.Json.JsonProperty("sell_listings")]
		public int sell_listings { get; set; }
		[Newtonsoft.Json.JsonProperty("sell_price")]
		public int sell_price { get; set; }
		[Newtonsoft.Json.JsonProperty("sell_price_text")]
		public string sell_price_text { get; set; }
		[Newtonsoft.Json.JsonProperty("app_icon")]
		public string app_icon { get; set; }
		[Newtonsoft.Json.JsonProperty("app_name")]
		public string app_name { get; set; }
		[Newtonsoft.Json.JsonProperty("asset_description")]
		public Asset_Description asset_description { get; set; }
		[Newtonsoft.Json.JsonProperty("sale_price_text")]
		public string sale_price_text { get; set; }
	}

	public class Asset_Description
	{
		[Newtonsoft.Json.JsonProperty("appid")]
		public int appid { get; set; }
		[Newtonsoft.Json.JsonProperty("classid")]
		public string classid { get; set; }
		[Newtonsoft.Json.JsonProperty("instanceid")]
		public string instanceid { get; set; }
		[Newtonsoft.Json.JsonProperty("currency")]
		public int currency { get; set; }
		[Newtonsoft.Json.JsonProperty("background_color")]
		public string background_color { get; set; }
		[Newtonsoft.Json.JsonProperty("icon_url")]
		public string icon_url { get; set; }
		[Newtonsoft.Json.JsonProperty("icon_url_large")]
		public string icon_url_large { get; set; }
		[Newtonsoft.Json.JsonProperty("descriptions")]
		public Description[] descriptions { get; set; }
		[Newtonsoft.Json.JsonProperty("tradable")]
		public int tradable { get; set; }
		[Newtonsoft.Json.JsonProperty("actions")]
		public Action[] actions { get; set; }
		[Newtonsoft.Json.JsonProperty("name")]
		public string name { get; set; }
		[Newtonsoft.Json.JsonProperty("name_color")]
		public string name_color { get; set; }
		[Newtonsoft.Json.JsonProperty("type")]
		public string type { get; set; }
		[Newtonsoft.Json.JsonProperty("market_name")]
		public string market_name { get; set; }
		[Newtonsoft.Json.JsonProperty("market_hash_name")]
		public string market_hash_name { get; set; }
		[Newtonsoft.Json.JsonProperty("market_actions")]
		public Market_Actions[] market_actions { get; set; }
		[Newtonsoft.Json.JsonProperty("commodity")]
		public int commodity { get; set; }
		[Newtonsoft.Json.JsonProperty("market_tradable_restriction")]
		public int market_tradable_restriction { get; set; }
		[Newtonsoft.Json.JsonProperty("marketable")]
		public int marketable { get; set; }
	}

	public class Description
	{
		[Newtonsoft.Json.JsonProperty("type")]
		public string type { get; set; }
		[Newtonsoft.Json.JsonProperty("value")]
		public string value { get; set; }
		[Newtonsoft.Json.JsonProperty("color")]
		public string color { get; set; }
	}

	public class Action
	{
		[Newtonsoft.Json.JsonProperty("link")]
		public string link { get; set; }
		[Newtonsoft.Json.JsonProperty("name")]
		public string name { get; set; }
	}

	public class Market_Actions
	{
		[Newtonsoft.Json.JsonProperty("link")]
		public string link { get; set; }
		[Newtonsoft.Json.JsonProperty("name")]
		public string name { get; set; }
	}
}