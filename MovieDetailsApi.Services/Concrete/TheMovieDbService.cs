using Dawn;
using MovieDetailsApi.Clients;
using MovieDetailsApi.Models;
using MovieDetailsApi.Models.Concrete;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDetailsApi.Services.Concrete
{
	public class TheMovieDbService : ITheMovieDbService
	{
		private readonly ITheMovieDbClient _theMovieDbClient;
		private readonly IJsonSerializationService _jsonSerializationService;

		public TheMovieDbService(
			ITheMovieDbClient theMovieDbClient,
			IJsonSerializationService jsonSerializationService)
		{
			_theMovieDbClient = Guard.Argument(() => theMovieDbClient).NotNull().Value;
			_jsonSerializationService = Guard.Argument(() => jsonSerializationService).NotNull().Value;
		}

		public async Task<IDetails> GetDetailsAsync(string title, int year)
		{
			var resultsJson = await _theMovieDbClient.SearchAsync(title, year)
				.ConfigureAwait(false);

			var results = _jsonSerializationService.Deserialize<Models.Generated.Search.Response>(resultsJson);

			if (results.total_results == 0)
			{
				return default;
			}

			var response = new Details
			{
				TheMovieDbId = results.results[0].id,
				Title = results.results[0].title,
				Year = DateTime.Parse(results.results[0].release_date, default, System.Globalization.DateTimeStyles.AssumeUniversal).ToUniversalTime().Year,
			};

			foreach (var result in from r in results.results
								   orderby r.popularity descending
								   select r)
			{
				var detailsJson = await _theMovieDbClient.DetailsAsync(result.id)
					.ConfigureAwait(false);

				var details = _jsonSerializationService.Deserialize<Models.Generated.Details.Response>(detailsJson);

				response.Runtime = details.runtime;

				break;
			}

			return response;
		}
	}
}
