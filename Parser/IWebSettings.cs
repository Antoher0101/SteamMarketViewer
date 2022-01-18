using System.Net;

namespace SteamMarketViewer.WebClient
{
	public interface IWebSettings
	{
		string URI { get; set; }
		bool NoCachePolicy { get; set; }
		bool AcceptGZipEncoding { get; set; }
		bool UseUnsafeHeaderParsing { get; set; }
		string Address { get; set; }
		string Accept { get; set; }
		string Referer { get; set; }
		string Host { get; set; }
		bool? KeepAlive { get; set; }
		string ContentType { get; set; }
		bool? Expect100Continue { get; set; }
		bool? AllowAutoRedirect { get; set; }
		string UserAgent { get; set; }
		WebProxy Proxy { get; set; }
		bool TurnOffProxy { get; set; }
		int TimeOut { get; set; }
	}
}
