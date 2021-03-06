﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;

namespace MovieDetailsApi.Api
{
	public static class IocHelpers
	{
		public static IConfiguration BuildConfiguration()
		{
			return new ConfigurationBuilder()
				.SetBasePath(Environment.CurrentDirectory)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();
		}

		public static IServiceCollection BuildServices(IServiceCollection services = default, IConfiguration configuration = default)
		{
			if (services == default)
			{
				services = new ServiceCollection();
			}

			if (configuration == default)
			{
				configuration = BuildConfiguration();
			}

			services
				.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			// settings
			services
				.Configure<Options.Mongo>(configuration.GetSection(nameof(Options.Mongo)))
				.Configure<Options.TheMovieDbApi>(configuration.GetSection(nameof(Options.TheMovieDbApi)));

			// http clients
			services
				.AddHttpClient(nameof(Clients.Concrete.TheMovieDbClient))
				.ConfigureHttpClient((provider, client) =>
				{
					var settings = provider.GetRequiredService<IOptions<Options.TheMovieDbApi>>().Value;
					client.BaseAddress = new Uri(settings.UriString, UriKind.Absolute);
					client.DefaultRequestHeaders.Clear();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				});

			// clients
			services
				.AddTransient<Clients.ITheMovieDbClient, Clients.Concrete.TheMovieDbClient>();

			// services
			services
				.AddTransient<Services.IJsonSerializationService, Services.Concrete.JsonSerializationService>()
				.AddTransient<Services.ISerializationService, Services.Concrete.JsonSerializationService>()
				.AddTransient<Services.IMongoService, Services.Concrete.MongoService>()
				.AddTransient<Services.ITheMovieDbService, Services.Concrete.TheMovieDbService>();

			// repositories
			services
				.AddTransient<Repositories.IMongoRepository, Repositories.Concrete.MongoRepository>();

			return services;
		}
	}
}
