<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Collections.Specialized</Namespace>
  <Namespace>System.Web</Namespace>
</Query>


void Main()
{
	var x = new Foo()
	{
		BoundingBox = new BoundingBox()
		{
			BottomLeft = new GeoPoint(52.02357943295799m, -0.7807043551790073m),
			TopRight = new GeoPoint(52.05269714839155m, -0.7345838571342367m)
		},
		Place = "Leeds",
		Type = "City",
		Min = 1,
		Max = 10,
		Keywords = new string[] { "Cities, Towns, Villages" },
		Include = false,
		Range = 50M
	};

	var res = ToQueryString(x);
	res.Dump();
}

public string ToQueryString(Foo foo)
{
	if (foo == null)
		throw new ArgumentNullException("foo");

	var properties = foo.GetType().GetProperties()
		.Where(x => x.CanRead && x.Name != "BoundingBox" && x.GetValue(foo, null) != null)
		.ToDictionary(x => toCamelCase(x.Name), x => x.GetValue(foo, null));

	helpers.BoundingBoxToQueryString(properties, foo.BoundingBox);

	var propertyNames = properties
		.Where(x => !(x.Value is string) && x.Value is IEnumerable)
		.Select(x => x.Key)
		.ToList();

	foreach (var key in propertyNames)
	{
		var valueType = properties[key].GetType();
		var valueElemType = valueType.IsGenericType
								? valueType.GetGenericArguments()[0]
								: valueType.GetElementType();
		if (valueElemType.IsPrimitive || valueElemType == typeof(string))
		{
			var enumerable = properties[key] as IEnumerable;
			var index = 0;
			foreach (var e in enumerable)
			{
				properties.Add($"{key}[{index}]", e.ToString());
				index++;
			}
			properties.Remove(key);
		}
	}

	return string.Join("&", properties
		.Select(x => string.Concat(
			Uri.EscapeDataString(x.Key), "=",
			Uri.EscapeDataString(formatParameterValues(x.Value)))));
}

public static class helpers
{

	public static Dictionary<string, object> BoundingBoxToQueryString(this Dictionary<string, object> obj, BoundingBox bbox)
	{
		if (bbox == null) return null;

		var bboxDict = new Dictionary<string, decimal>
		{
			{ "boundingBox.topRight.latitude", bbox.TopRight.Latitude },
			{ "boundingBox.topRight.longitude", bbox.TopRight.Longitude },
			{ "boundingBox.bottomLeft.latitude", bbox.BottomLeft.Latitude },
			{ "boundingBox.bottomLeft.longitude", bbox.BottomLeft.Longitude }
		};

		foreach (var element in bboxDict)
		{
			obj.Add(element.Key, element.Value);
		}

		return obj;
	}
}

public string formatParameterValues(object obj)
{
	if (obj.GetType() == typeof(bool))
	{
		return obj.ToString().ToLower();
	}

	return obj.ToString();
}


public string toCamelCase(string str)
{
	return char.ToLower(str[0]) + str[1..];
}

public class Foo
{
	public BoundingBox BoundingBox { get; set; }

	public string Place { get; set; }

	public string[] Keywords { get; set; }

	public int? Min { get; set; }

	public int? Max { get; set; }

	public string Type { get; set; }

	public bool Include { get; set; }

	public decimal Range { get; set; }
}

public class BoundingBox
{
	public GeoPoint BottomLeft { get; set; }

	public GeoPoint TopRight { get; set; }
}

public class GeoPoint
{
	public decimal Latitude { get; set; }

	public decimal Longitude { get; set; }

	public GeoPoint()
	{
	}

	public GeoPoint(decimal lat, decimal lon)
	{
		Latitude = lat;
		Longitude = lon;
	}
}

