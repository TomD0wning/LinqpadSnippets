<Query Kind="Program">
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
</Query>

public static class JsonContractResolver
{
	public static object ToJsonObject(this object o)
	{
		var casedObject = JsonConvert.SerializeObject(o, Formatting.Indented,
			new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new List<JsonConverter>()
				{
					new Newtonsoft.Json.Converters.StringEnumConverter()
				}
			});
		return JObject.Parse(casedObject);
	}
}