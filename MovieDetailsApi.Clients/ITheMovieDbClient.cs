using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieDetailsApi.Clients
{
	public interface ITheMovieDbClient
	{
		Task<string> DetailsAsync(int id);
		Task<string> SearchAsync(string query, int year);
	}
}
