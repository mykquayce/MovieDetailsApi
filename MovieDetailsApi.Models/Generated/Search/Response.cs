using System.Collections.Generic;

namespace MovieDetailsApi.Models.Generated.Search
{
	public class Response
	{
		public int page { get; set; }
		public int total_results { get; set; }
		public int total_pages { get; set; }
		public IList<Result> results { get; } = new List<Result>();
	}
}
