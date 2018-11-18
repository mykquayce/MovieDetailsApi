using System;
using Dawn;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace MovieDetailsApi.Api.Controllers
{
	[Route("")]
	[ApiController]
	public class AliveController : ControllerBase
	{
		private readonly IHostingEnvironment _hostingEnvironment;

		public AliveController(
			IHostingEnvironment hostingEnvironment)
		{
			_hostingEnvironment = Guard.Argument(() => hostingEnvironment).NotNull().Value;
		}

		[HttpGet]
		public IActionResult Get()
		{
			return Ok(new
			{
				HttpStatusCode = System.Net.HttpStatusCode.OK,
				_hostingEnvironment.EnvironmentName,
				_hostingEnvironment.ApplicationName,
				ConrollerName = this.GetType().Name,
				DateTime = DateTime.UtcNow,
			});
		}
	}
}
