using MovieDetailsApi.Models;
using System.Threading.Tasks;

namespace MovieDetailsApi.Services
{
	public interface IMongoService
	{
		Task<IDetails> GetDetailsAsync(string title, int year);
		Task SaveDetailsAsync(IDetails details);
	}
}
