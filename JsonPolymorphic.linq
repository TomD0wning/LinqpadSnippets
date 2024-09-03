<Query Kind="Program">
  <Namespace>System.Text.Json.Serialization</Namespace>
  <Namespace>System.Text.Json</Namespace>
</Query>

void Main()
{
	var m = new Message { Method = "Target.createTarget" };
	var n = m.ToJson();
	n.Dump();
}

[JsonDerivedType(typeof(MessageBase), typeDiscriminator: "base")]
[JsonDerivedType(typeof(Message))]
internal class MessageBase
{
	[JsonPropertyName("id")]
	public int Id { get; set; }
	public static MessageBase FromJson(string json) => JsonSerializer.Deserialize<MessageBase>(json);
	public string ToJson() => JsonSerializer.Serialize(this);
}

internal class Message : MessageBase
{
	[JsonPropertyName("method")]
	public string Method { get; set; }
	[JsonPropertyName("params")]
	public Dictionary<string, object> Parameters { get; set; }

	public Message()
	{
		Id = 1;
		Parameters = new Dictionary<string, object>();
	}

	public void AddParameter(string name, object value) => Parameters.Add(name, value);
	public static new Message FromJson(string json) => JsonSerializer.Deserialize<Message>(json);
}