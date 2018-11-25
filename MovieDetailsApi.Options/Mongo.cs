using System.Collections.Generic;

namespace MovieDetailsApi.Options
{
	public class Mongo
	{
		public ICollection<string> ConnectionStrings { get; } = new List<string>();
		public string DatabaseName { get; set; }
		public string CollectionName { get; set; }
	}
}
