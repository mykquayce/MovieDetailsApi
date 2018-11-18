using System;

namespace MovieDetailsApi.Models
{
	public interface IDetails
	{
		int Id { get; }
		int Runtime { get; }
		string Title { get; }
		int Year { get; }
	}
}
