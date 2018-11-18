using Newtonsoft.Json;

namespace MovieDetailsApi.Services.Concrete
{
	public class JsonSerializationService : IJsonSerializationService
	{
		public T DeepClone<T>(T o) => Deserialize<T>(Serialize(o));
		public T Deserialize<T>(string s) => JsonConvert.DeserializeObject<T>(s);
		public string Serialize<T>(T o) => JsonConvert.SerializeObject(o);
	}
}
