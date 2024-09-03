<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
	var postcode = "BB2 5 JR ".FormatPostcode();
	postcode.Dump();
	char.IsWhiteSpace(postcode[^4]).Dump();
}

public string ToCamelCase(string str)
{
	return char.ToLower(str[0]) + str[1..];
}


public static class StringExtensions
{
	private const string SpaceDeliminator = " ";
	private const int MinPostCodeLength = 3;

	public static string FormatPostcode(this string postcode)
	{
		if (!string.IsNullOrWhiteSpace(postcode))
		{
			postcode = postcode.Replace(SpaceDeliminator, string.Empty);
			
			if (postcode.Length > 3)
			{
				postcode = postcode.Insert(postcode.Length - MinPostCodeLength, SpaceDeliminator);
			}
		}

		return postcode;
	}

	public static string TrimPostCode(this string postcode)
	{
		if (!string.IsNullOrWhiteSpace(postcode))
		{
			postcode = postcode.Replace(SpaceDeliminator, string.Empty).ToUpper();
		}

		return postcode;
	}

	public static string RemoveSpecialCharacters(this string input) =>
		string.IsNullOrWhiteSpace(input)
			? string.Empty
			: input.Replace(" ", "").Replace("-", "").Replace("'", "").Replace("/", "").Replace("(", "").Replace(")", "").Replace("&", "");

	public static string ToCamelCase(this string str) => char.ToLower(str[0]) + str[1..];
}

// You can define other methods, fields, classes and namespaces here
