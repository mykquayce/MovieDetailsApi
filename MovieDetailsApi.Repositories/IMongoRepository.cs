using System.Threading.Tasks;
using MovieDetailsApi.Models;

namespace MovieDetailsApi.Repositories
{
	public interface IMongoRepository
	{
		Task CacheDetailsAsync(IDetails details);
		Task<IDetails> GetDetailsAsync(string id);
	}
}
