namespace MovieDetailsApi.Services
{
	public interface ISerializationService
	{
		T DeepClone<T>(T o);
		T Deserialize<T>(string s);
		string Serialize<T>(T o);
	}
}
