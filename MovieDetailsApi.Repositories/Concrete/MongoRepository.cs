using Dawn;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MovieDetailsApi.Models;
using MovieDetailsApi.Models.Concrete;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MovieDetailsApi.Repositories.Concrete
{
	public class MongoRepository : IMongoRepository
	{
		private readonly IMongoCollection<Details> _collection;
		private static readonly InsertOneOptions _insertOneOptions = new InsertOneOptions { BypassDocumentValidation = false, };

		public MongoRepository(
			IOptions<Settings> settingsOptions)
		{
			Guard.Argument(() => settingsOptions).NotNull();
			Guard.Argument(() => settingsOptions.Value).NotNull();
			Guard.Argument(() => settingsOptions.Value.MongoConnectionString).NotNull().NotEmpty().NotWhiteSpace();
			Guard.Argument(() => settingsOptions.Value.MongoDatabaseName).NotNull().NotEmpty().NotWhiteSpace();
			Guard.Argument(() => settingsOptions.Value.MongoCollectionName).NotNull().NotEmpty().NotWhiteSpace();

			var client = new MongoClient(settingsOptions.Value.MongoConnectionString);

			var db = client.GetDatabase(settingsOptions.Value.MongoDatabaseName);

			_collection = db.GetCollection<Details>(settingsOptions.Value.MongoCollectionName);
		}

		public async Task CacheDetailsAsync(IDetails details)
		{
			Guard.Argument(() => details).NotNull();
			Guard.Argument(() => details.Id).NotNull().NotEmpty().NotWhiteSpace().Matches(@"^[0-9a-z]+\d{4}$");
			Guard.Argument(() => details.TheMovieDbId).InRange(1, int.MaxValue);
			Guard.Argument(() => details.Title).NotNull().NotEmpty().NotWhiteSpace();
			Guard.Argument(() => details.Year).InRange(1900, 9999);

			// check it's not cached already
			var cached = await GetDetailsAsync(details.Id)
				.ConfigureAwait(false);

			if (cached != default)
			{
				return;
			}

			var concrete = (Details)details;

			await _collection.InsertOneAsync(concrete, _insertOneOptions, CancellationToken.None)
				.ConfigureAwait(false);
		}

		public async Task<IDetails> GetDetailsAsync(string id)
		{
			Guard.Argument(() => id).NotNull().NotEmpty().NotWhiteSpace().Matches(@"^[0-9a-z]+\d{4}$");

			return await _collection
				.FindSync(d => d.Id == id)
				.FirstOrDefaultAsync()
				.ConfigureAwait(false);
		}
	}
}
