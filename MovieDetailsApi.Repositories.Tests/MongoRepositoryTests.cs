using Microsoft.Extensions.DependencyInjection;
using MovieDetailsApi.Api;
using MovieDetailsApi.Models.Concrete;
using Xunit;

namespace MovieDetailsApi.Repositories.Tests
{
	public class MongoRepositoryTests
	{
		private readonly IMongoRepository _mongoRepository;

		public MongoRepositoryTests()
		{
			var provider = IocHelpers.BuildServices().BuildServiceProvider();

			_mongoRepository = provider.GetRequiredService<IMongoRepository>();
		}

		[Theory]
		[InlineData(578, 124, "Jaws", 1975)]
		public void MongoRepositoryTests_CacheDetailsAsync_BehavesPredictably(
			int theMovieDbId, int runtime, string title, int year)
		{
			var details = new Details
			{
				Id = title.ToLowerInvariant() + year.ToString(),
				TheMovieDbId = theMovieDbId,
				Runtime = runtime,
				Title = title,
				Year = year,
			};

			_mongoRepository.CacheDetailsAsync(details)
				.ConfigureAwait(false).GetAwaiter().GetResult();
		}

		[Theory]
		[InlineData("jaws1975")]
		public void MongoRepositoryTests_GetDetailsAsync_BehavesPredictably(
			string id)
		{
			var details = _mongoRepository.GetDetailsAsync(id)
				.ConfigureAwait(false).GetAwaiter().GetResult();

			if (details == default)
			{
				return;
			}

			Assert.NotNull(details);
			Assert.InRange(details.TheMovieDbId, 1, int.MaxValue);
			Assert.InRange(details.Runtime, 1, int.MaxValue);
			Assert.Equal(id, details.Id);
		}
	}
}
