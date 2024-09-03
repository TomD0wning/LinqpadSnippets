<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Text.Json.Serialization</Namespace>
</Query>

void Main()
{
	//var jsonString = "{\"PrimaryFruit\":0,\"SecondaryFruit\":\"pear\",\"TertiaryFruit\":1}";

	//NewtonSoftTest(jsonString);
	//SystemJsonTest(jsonString);
	var EndDate = new DateTime(2023,10,12, 23, 59, 59).ToUniversalTime();
	var x = new Fruits { PrimaryFruit = Fruit.apple, SecondaryFruit = Fruit.orange, TertiaryFruit = Fruit.pear, TimeStamp = EndDate };
	JsonSerializer.Serialize(x).Dump();
}


public void NewtonSoftTest(string fruit)
{
	var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Fruits>(fruit).Dump();
	Newtonsoft.Json.JsonConvert.SerializeObject(result).Dump();
	
}

public void SystemJsonTest(string fruits)
{
	var options = new JsonSerializerOptions() { AllowTrailingCommas = true, PropertyNameCaseInsensitive = true };
	options.Converters.Add(new JsonStringEnumConverter());
	
	var result = JsonSerializer.Deserialize<Fruits>(fruits, options);
	result.Dump();
	JsonSerializer.Serialize(result, options).Dump();
}

public class Fruits
{
	public Fruit PrimaryFruit { get; set; }
	public Fruit SecondaryFruit { get; set; }
	public Fruit TertiaryFruit { get; set; }
	public DateTime TimeStamp { get; set; }
}

public enum Fruit
{
	apple,
	orange,
	pear
}
