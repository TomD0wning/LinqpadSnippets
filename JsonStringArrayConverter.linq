<Query Kind="Program" />

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Search.Domain
{
	public class JsonStringToArrayConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return true;
		}

		public override bool CanWrite => false;

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JToken jToken = JToken.Load(reader);

			switch (jToken.Type)
			{
				case JTokenType.Null:
				case JTokenType.None:
					return null;
				case JTokenType.String:
					return new string[] { jToken.ToObject<string>() };
				case JTokenType.Array:
					return jToken.ToObject<string[]>();
			}
			return null;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}

	public class JsonStringToArrayConverterV2 : JsonConverter<string[]>
	{
		public override bool CanConvert(Type typeToConvert)
		{
			return typeToConvert.IsPrimitive || typeToConvert.IsArray;
		}

		public override string[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			switch (reader.TokenType)
			{
				case JsonTokenType.None:
				case JsonTokenType.Null:
					return null;
				case JsonTokenType.String:
					return new[] { reader.GetString() };
				case JsonTokenType.StartArray:
					var hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

					while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
					{
						hashSet.Add(reader.GetString());
					}
					return hashSet.ToArray();
			}
			return null;
		}

		public override void Write(Utf8JsonWriter writer, string[] value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			foreach (var item in value)
			{
				JsonSerializer.Serialize(writer, value, options);
			}
			writer.WriteEndArray();
		}
	}
}
