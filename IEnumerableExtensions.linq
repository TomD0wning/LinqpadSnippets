<Query Kind="Program" />

public static class Filter
{
	public static IEnumerable<T> ExcludeExisting<T, T2>(this IEnumerable<T> src,
	IEnumerable<T> ex, Func<T, T2> keyFunc)
	{
		var excluded = new HashSet<T2>(ex.Select(keyFunc));
		return src.Where(x => !excluded.Contains(keyFunc(x)));
	}
}

