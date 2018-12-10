using Dawn;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MovieDetailsApi.Models;
using MovieDetailsApi.Services;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MovieDetailsApi.Api.Controllers.v1
{
	[Route("v1/[controller]")]
	[ApiController]
	public class QueryController : ControllerBase
	{
		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly IMongoService _mongoService;
		private readonly ITheMovieDbService _theMovieDbService;

		public QueryController(
			IHostingEnvironment hostingEnvironment,
			IMongoService mongoService,
			ITheMovieDbService theMovieDbService)
		{
			_hostingEnvironment = Guard.Argument(() => hostingEnvironment).NotNull().Value;
			_mongoService = Guard.Argument(() => mongoService).NotNull().Value;
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
		[Route("{title:regex(^[[ 0-9A-Za-z]]+$)}/{year:int:range(1900,9999)}")]
		[ProducesResponseType(typeof(IDetails), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> GetAsync([FromRoute] string title, [FromRoute] int year)
		{
			// try the cache
			var details = await _mongoService.GetDetailsAsync(title, year)
				.ConfigureAwait(false);

			if (details != default)
			{
				return Ok(details);
			}

			details = await _theMovieDbService.GetDetailsAsync(title, year)
				.ConfigureAwait(false);

			if (details == default)
			{
				return NotFound();
			}

			await _mongoService.SaveDetailsAsync(details)
				.ConfigureAwait(false);

			return Ok(details);
		}
	}
}
