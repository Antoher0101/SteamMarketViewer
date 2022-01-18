using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace SteamMarketViewer.Parser
{
	class HtmlLoader
	{
		readonly HttpClient client;
		readonly string url;

		public HtmlLoader(IParserSettings settings)
		{
			client = new HttpClient();
			url = $"{settings.BaseUrl}/{settings.Prefix}/";
		}

		public async Task<string> GetSourceByPageId(int id)
		{
			var currentUrl = url.Replace("{id}", id.ToString());
			
			var response = await client.GetAsync(currentUrl);
			string source = null;

			if (response != null && response.StatusCode == HttpStatusCode.OK)
			{
				source = await response.Content.ReadAsStringAsync();
			}

			return source;
		}
	}
}