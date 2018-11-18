using MovieDetailsApi.Models;
using System.Threading.Tasks;

namespace MovieDetailsApi.Services
{
	public interface ITheMovieDbService
	{
		Task<IDetails> GetDetailsAsync(string title, int year);
	}
}
