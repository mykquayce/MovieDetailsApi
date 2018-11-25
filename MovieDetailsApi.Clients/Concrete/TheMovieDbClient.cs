using Dawn;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieDetailsApi.Clients.Concrete
{
	public class TheMovieDbClient : ClientBase, ITheMovieDbClient
	{
		private readonly string _apiKey;

		public TheMovieDbClient(
			IHttpClientFactory httpClientFactory,
			IOptions<Options.TheMovieDbApi> theMovieDbApiOptions)
			: base(httpClientFactory)
		{
			Guard.Argument(() => theMovieDbApiOptions).NotNull();
			Guard.Argument(() => theMovieDbApiOptions.Value).NotNull();

			_apiKey = Guard
				.Argument(() => theMovieDbApiOptions.Value.ApiKey)
				.NotNull()
				.NotEmpty()
				.NotWhiteSpace()
				.Require(s => string.Equals(s, s.Trim()), _ => nameof(Options.TheMovieDbApi.ApiKey) + " cannot have leading/trailing whitespace")
				.Value;
		}

		public Task<string> DetailsAsync(int id)
		{
			Guard.Argument(() => id).Positive();

			var uri = new Uri($"/3/movie/{id:D}?api_key={_apiKey}", UriKind.Relative);

			return base.GetStringAsync(uri);
		}

		public Task<string> SearchAsync(string query, int year)
		{
			Guard
				.Argument(() => query)
				.NotNull()
				.NotEmpty()
				.NotWhiteSpace()
				.Require(s => string.Equals(s, s.Trim()), _ => nameof(query) + " cannot have leading/trailing whitespace");

			Guard.Argument(() => year).InRange(1900, DateTime.UtcNow.Year);

			var encodedQuery = System.Web.HttpUtility.UrlEncode(query);

			var uri = new Uri($"/3/search/movie?api_key={_apiKey}&query={encodedQuery}&year={year:D}", UriKind.Relative);

			return base.GetStringAsync(uri);
		}
	}
}
