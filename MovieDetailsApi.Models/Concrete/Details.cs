namespace MovieDetailsApi.Models.Concrete
{
	public class Details : IDetails
	{
		public int Id { get; set; }
		public int Runtime { get; set; }
		public string Title { get; set; }
		public int Year { get; set; }
	}
}
