using Dawn;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieDetailsApi.Clients.Concrete
{
	public class ClientBase
	{
		private readonly HttpClient _httpClient;

		public ClientBase(
			IHttpClientFactory httpClientFactory)
		{
			Guard.Argument(() => httpClientFactory).NotNull();

			_httpClient = httpClientFactory.CreateClient(this.GetType().Name);
		}

		public Task<string> GetStringAsync(Uri uri)
		{
			Guard.Argument(() => uri).NotNull().Require(u => !string.IsNullOrWhiteSpace(u.OriginalString));

			return _httpClient.GetStringAsync(uri);
		}
	}
}
