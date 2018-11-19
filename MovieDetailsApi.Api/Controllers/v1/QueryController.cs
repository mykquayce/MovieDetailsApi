using System;
using System.Net;
using System.Threading.Tasks;
using Dawn;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MovieDetailsApi.Models;
using MovieDetailsApi.Repositories;
using MovieDetailsApi.Services;

namespace MovieDetailsApi.Api.Controllers.v1
{
	[Route("v1/[controller]")]
	[ApiController]
	public class QueryController : ControllerBase
	{
		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly IMongoRepository _mongoRepository;
		private readonly ITheMovieDbService _theMovieDbService;

		public QueryController(
			IHostingEnvironment hostingEnvironment,
			IMongoRepository mongoRepository,
			ITheMovieDbService theMovieDbService)
		{
			_hostingEnvironment = Guard.Argument(() => hostingEnvironment).NotNull().Value;
			_mongoRepository = Guard.Argument(() => mongoRepository).NotNull().Value;
			_theMovieDbService = Guard.Argument(() => theMovieDbService).NotNull().Value;
		}

		[HttpGet]
		public IActionResult Get()
		{
			return Ok(new
			{
				HttpStatusCode = HttpStatusCode.OK,
				_hostingEnvironment.EnvironmentName,
				_hostingEnvironment.ApplicationName,
				ConrollerName = this.GetType().Name,
				DateTime = DateTime.UtcNow,
			});
		}

		[HttpGet]
		[Route("{query:regex(^[[ 0-9A-Za-z]]+$)}/{year:int:range(1900,9999)}")]
		[ProducesResponseType(typeof(IDetails), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> GetAsync([FromRoute] string query, [FromRoute] int year)
		{
			var id = query.ToLowerInvariant().Replace(" ", string.Empty) + year.ToString();

			// try the cache
			var details = await _mongoRepository.GetDetailsAsync(id)
				.ConfigureAwait(false);

			if (details != default)
			{
				return Ok(details);
			}

			details = await _theMovieDbService.GetDetailsAsync(query, year)
				.ConfigureAwait(false);

			details.Id = id;

			await _mongoRepository.CacheDetailsAsync(details)
				.ConfigureAwait(false);

			return Ok(details);
		}
	}
}
