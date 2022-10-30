<Query Kind="Program" />

void Main()
{
	var x = new Foo() { SampleOne = "abc", SampleTwo = 123 };
	var res = x.AsDictionary();

	res.Dump();

}

public static class DictionaryExtensions
{
	public static IDictionary<string, object> AsDictionary(this object source)
	{
		return source.GetType().GetProperties().Where(prop => prop.GetValue(source, null) != null).ToDictionary(
			propInfo => GetPropertyName(propInfo),
			propInfo => propInfo.GetValue(source, null) ?? string.Empty
		);
	}

	private static string GetPropertyName(PropertyInfo propertyInfo)
	{
		var namingAttribute = Attribute.GetCustomAttribute(propertyInfo, typeof(PropertyNameAttribute)) as PropertyNameAttribute;

		return namingAttribute?.PropertyName ?? propertyInfo!.Name;
	}
}

[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public sealed class PropertyNameAttribute : Attribute
{
	public PropertyNameAttribute(string propertyName)
	{
		PropertyName = propertyName;
	}

	public string PropertyName { get; }
}


public class Foo : IFoo
{
	[PropertyNameAttribute("sample_one")]
	public string SampleOne { get; set; }

	[PropertyNameAttribute("sample_two")]
	public int SampleTwo { get; set; }

}

public interface IFoo
{
	string SampleOne { get; set; }
	int SampleTwo { get; set; }
}


