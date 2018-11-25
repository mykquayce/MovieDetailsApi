using Dawn;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MovieDetailsApi.Models;
using MovieDetailsApi.Models.Concrete;
using System.Threading;
using System.Threading.Tasks;

namespace MovieDetailsApi.Repositories.Concrete
{
	public class MongoRepository : IMongoRepository
	{
		private readonly IMongoCollection<IDetails> _collection;
		private static readonly InsertOneOptions _insertOneOptions = new InsertOneOptions { BypassDocumentValidation = false, };

		public MongoRepository(
			IOptions<Options.Mongo> mongoOptions)
		{
			Guard.Argument(() => mongoOptions).NotNull();
			Guard.Argument(() => mongoOptions.Value).NotNull();
			Guard.Argument(() => mongoOptions.Value.ConnectionStrings).NotNull().NotEmpty().DoesNotContainNull();
			Guard.Argument(() => mongoOptions.Value.DatabaseName).NotNull().NotEmpty().NotWhiteSpace();
			Guard.Argument(() => mongoOptions.Value.CollectionName).NotNull().NotEmpty().NotWhiteSpace();

			IMongoDatabase db = default;

			foreach (var mongoConnectionString in mongoOptions.Value.ConnectionStrings)
			{
				var client = new MongoClient(mongoConnectionString);

				db = client.GetDatabase(mongoOptions.Value.DatabaseName);

				var success = db.RunCommandAsync((Command<BsonDocument>)"{ping:1}")
					.Wait(millisecondsTimeout: 1_000);

				if (success)
				{
					break;
				}
			}

			if (db == default)
			{
				throw new System.InvalidOperationException("Unable to connect to Mongo DB");
			}

			_collection = db.GetCollection<IDetails>(mongoOptions.Value.CollectionName);
		}

		public async Task CacheDetailsAsync(IDetails details)
		{
			Guard.Argument(() => details).NotNull();
			Guard.Argument(() => details.Id).NotNull().NotEmpty().NotWhiteSpace().Matches(@"^[0-9a-z]+\d{4}$");
			Guard.Argument(() => details.Runtime).NotNegative();
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

		public Task<IDetails> GetDetailsAsync(string id)
		{
			Guard.Argument(() => id).NotNull().NotEmpty().NotWhiteSpace().Matches(@"^[0-9a-z]+\d{4}$");

			return _collection
				.FindSync(d => ((Details)d).Id == id)
				.FirstOrDefaultAsync();
		}
	}
}
