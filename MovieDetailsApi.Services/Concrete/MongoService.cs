using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dawn;
using MovieDetailsApi.Models;
using MovieDetailsApi.Repositories;

namespace MovieDetailsApi.Services.Concrete
{
	public class MongoService : IMongoService
	{
		private readonly IMongoRepository _mongoRepository;

		public MongoService(
			IMongoRepository mongoRepository)
		{
			_mongoRepository = Guard.Argument(() => mongoRepository).NotNull().Value;
		}

		public Task<IDetails> GetDetailsAsync(string title, int year)
		{
			Guard.Argument(() => title).NotNull().NotEmpty().NotWhiteSpace();
			Guard.Argument(() => year).InRange(1900, 9999);

			var id = BuildId(title, year);

			try
			{
				return _mongoRepository.GetDetailsAsync(id);
			}
			catch (TimeoutException)
			{
				return Task.FromResult<IDetails>(default);
			}
		}

		public Task SavetDetailsAsync(IDetails details)
		{
			Guard.Argument(() => details).NotNull();
			Guard.Argument(() => details.Id).Require(s => s == default || Regex.IsMatch(s, @"^[ 0-9a-z]+\d{4}$"));
			Guard.Argument(() => details.Runtime).NotNegative();
			Guard.Argument(() => details.TheMovieDbId).Positive();
			Guard.Argument(() => details.Title).NotNull().NotEmpty().NotWhiteSpace();
			Guard.Argument(() => details.Year).InRange(1900, 9999);

			if (string.IsNullOrWhiteSpace(details.Id))
			{
				details.Id = BuildId(details.Title, details.Year);
			}

			try
			{
				return _mongoRepository.CacheDetailsAsync(details);
			}
			catch (TimeoutException)
			{
				return Task.CompletedTask;
			}
		}

		private static string BuildId(string title, int year)
		{
			Guard.Argument(() => title).NotNull().NotEmpty().NotWhiteSpace();
			Guard.Argument(() => year).InRange(1900, 9999);

			return title.ToLowerInvariant().Replace(" ", string.Empty)
				+ year.ToString();
		}
	}
}
