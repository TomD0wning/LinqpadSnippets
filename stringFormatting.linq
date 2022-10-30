<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
	var price = 1000;
	$"£{price:N0}".Dump();
	price.ToString("C0", new CultureInfo("en-GB")).Dump();
	var emptyString = "";

	string.IsNullOrWhiteSpace(emptyString).Dump();
}

public string ToCamelCase(string str)
{
	return char.ToLower(str[0]) + str[1..];
}

// You can define other methods, fields, classes and namespaces here
