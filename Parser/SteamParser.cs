using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;

namespace SteamMarketViewer.Parser
{
	class SteamParser : IParser<string[]>
	{
		public string[] Parse(IHtmlDocument document)
		{
			var list = new List<string>();
			var items = document.QuerySelectorAll("a").Where(item => item.ClassName != null && item.ClassName.Contains("market_listing_row_link"));

			foreach (var item in items)
			{
				list.Add(item.GetAttribute("href"));
			}
			return list.ToArray();
		}
	}
}
