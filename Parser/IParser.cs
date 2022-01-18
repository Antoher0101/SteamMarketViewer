using AngleSharp.Html.Dom;

namespace SteamMarketViewer.Parser
{
	interface IParser<T> where T : class
	{
		T Parse(IHtmlDocument document);
	}
}