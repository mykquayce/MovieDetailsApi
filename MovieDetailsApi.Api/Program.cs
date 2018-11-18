using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace MovieDetailsApi.Api
{
	public static class Program
	{
		public static Task Main(string[] args)
		{
			return CreateWebHostBuilder(args)
				.Build()
				.RunAsync();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}
