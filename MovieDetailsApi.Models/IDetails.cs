namespace MovieDetailsApi.Models
{
	public interface IDetails
	{
		string Id { get; set; }
		int TheMovieDbId { get; }
		int Runtime { get; }
		string Title { get; }
		int Year { get; }
	}
}
