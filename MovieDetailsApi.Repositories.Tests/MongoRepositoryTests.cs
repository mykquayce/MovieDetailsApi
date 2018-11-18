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
			int id, int runtime, string title, int year)
		{
			var details = new Details
			{
				Id = id,
				Runtime = runtime,
				Title = title,
				Year = year,
			};

			_mongoRepository.CacheDetailsAsync(details)
				.ConfigureAwait(false).GetAwaiter().GetResult();
		}

		[Theory]
		[InlineData("Jaws", 1975)]
		public void MongoRepositoryTests_GetDetailsAsync_BehavesPredictably(
			string title, int year)
		{
			var details = _mongoRepository.GetDetailsAsync(title, year)
				.ConfigureAwait(false).GetAwaiter().GetResult();

			if (details == default)
			{
				return;
			}

			Assert.NotNull(details);
			Assert.InRange(details.Id, 1, int.MaxValue);
			Assert.InRange(details.Runtime, 1, int.MaxValue);
			Assert.Equal(title, details.Title);
			Assert.Equal(year, details.Year);
		}
	}
}
