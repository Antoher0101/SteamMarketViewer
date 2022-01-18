using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamMarketViewer.Parser
{
	enum SortColumn
	{
		Quantity,
		Price,
		Name
	}

	enum SortDir
	{
		Asc,
		Desc
	}
	interface ISearch
	{
		NameValueCollection GetCollection();
		string Query { get; set; }
		int Start { get; set; }
		int Count { get; set; }
		bool SearchDescriptions { get; set; }
		SortColumn Sort_column { get; set; }
		SortDir Sort_dir { get; set; }
		string Desc { get; set; }
		int Appid { get; set; }
		int Norender { get; set; }
		
	}
}
