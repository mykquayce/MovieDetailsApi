namespace MovieDetailsApi.Models.Concrete
{
	public class Settings
	{
		public string MongoConnectionString { get; set; }
		public string MongoDatabaseName { get; set; }
		public string MongoCollectionName { get; set; }
		public string TheMovieDbApiKey { get; set; }
		public string TheMovieDbApiUrl { get; set; }
	}
}
