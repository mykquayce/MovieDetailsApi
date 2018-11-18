using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace MovieDetailsApi.Models.Tests
{
	public class SearchResponseTests
	{
		[Fact]
		public void SearchResponseTests_Response_HasAllTheValues()
		{
			string json;

			using (var reader = new StreamReader(Path.Combine(Environment.CurrentDirectory, "Data", "search.json")))
			{
				json = reader.ReadToEnd();
			}

			var result = JsonConvert.DeserializeObject<Models.Generated.Search.Response>(json);

			Assert.NotNull(result);
			Assert.Equal(1, result.page);
			Assert.Equal(9, result.total_results);
			Assert.Equal(1, result.total_pages);
			Assert.NotNull(result.results);
			Assert.NotEmpty(result.results);
			Assert.Equal(9, result.results.Count);
			Assert.All(result.results.Select(r => r.vote_count), i => Assert.InRange(i, 0, int.MaxValue));
			Assert.All(result.results.Select(r => r.id), i => Assert.NotEqual(0, i));
			Assert.All(result.results.Select(r => r.vote_average), i => Assert.InRange(i, 0, int.MaxValue));
			Assert.All(result.results.Select(r => r.title), Assert.NotNull);
			Assert.All(result.results.Select(r => r.title), Assert.NotEmpty);
			Assert.All(result.results.Select(r => r.popularity), i => Assert.NotEqual(0, i));
			Assert.All(result.results.Select(r => r.poster_path), Assert.NotNull);
			Assert.All(result.results.Select(r => r.poster_path), Assert.NotEmpty);
			Assert.All(result.results.Select(r => r.original_language), Assert.NotNull);
			Assert.All(result.results.Select(r => r.original_language), Assert.NotEmpty);
			Assert.All(result.results.Select(r => r.original_title), Assert.NotNull);
			Assert.All(result.results.Select(r => r.original_title), Assert.NotEmpty);
			Assert.All(result.results.Select(r => r.genre_ids), Assert.NotNull);
			Assert.All(result.results.Select(r => r.genre_ids), Assert.NotEmpty);
			Assert.All(result.results.SelectMany(r => r.genre_ids.Select(i => i)), i => Assert.NotEqual(0, i));
			Assert.All(result.results.Select(r => r.overview), Assert.NotNull);
			Assert.All(result.results.Select(r => r.overview), Assert.NotEmpty);
			Assert.All(result.results.Select(r => r.release_date), Assert.NotNull);
			Assert.All(result.results.Select(r => r.release_date), Assert.NotEmpty);
		}
	}
}
