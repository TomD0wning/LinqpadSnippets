<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
</Query>

public static string JsonTimeStampToDate(string json)
{
	var queryRoot = JObject.Parse(json);
	
	IterateObjectTree(queryRoot, token =>
	{
		if (token is JProperty jProp &&
		jProp.HasValues &&
		jProp.Value.Type == JTokenType.Date && jProp.Value.Value<DateTime>() == default)
		{
			jProp.Remove();
			return -1;
		}
		return 0;
	});

	return json;
}

private static void IterateObjectTree(JToken token, Func<JToken, int> callback)
{
	var childProps = token.Children();
	for (int i = 0; i < childProps.Count(); i++)
	{
		var child = childProps.ElementAt(i);
		i += callback(child);
		IterateObjectTree(child, callback);
	}
}