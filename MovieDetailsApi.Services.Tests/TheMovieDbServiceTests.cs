using Microsoft.Extensions.DependencyInjection;
using MovieDetailsApi.Api;
using System;
using Xunit;

namespace MovieDetailsApi.Services.Tests
{
	public class TheMovieDbServiceTests
	{
		private readonly ITheMovieDbService _theMovieDbService;

		public TheMovieDbServiceTests()
		{
			var provider = IocHelpers.BuildServices().BuildServiceProvider();

			_theMovieDbService = provider.GetRequiredService<ITheMovieDbService>();
		}

		[Theory]
		[InlineData("Jaws", 1975)]
		public void TheMovieDbServiceTests_GetDetailsAsync_BehavesPredictably(string title, int year)
		{
			// Act
			var actual = _theMovieDbService.GetDetailsAsync(title, year)
				.ConfigureAwait(false).GetAwaiter().GetResult();

			// Assert
			Assert.NotNull(actual);
			Assert.InRange(actual.TheMovieDbId, 1, int.MaxValue);
			Assert.NotNull(actual.Title);
			Assert.NotEmpty(actual.Title);
			Assert.InRange(actual.Runtime, 1, int.MaxValue);
			Assert.InRange(actual.Year, 1900, 9999);
		}
	}
}
