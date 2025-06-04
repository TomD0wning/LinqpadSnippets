<Query Kind="Program" />

void Main()
{
	ApplyOffset(200000, true).Dump();
	ApplyOffset(100000, false).Dump();
	var x = new Foo(){Baz = null};
	CheckDecline(x, x => x.Baz, IsValidLastService);
}

 private int? ApplyOffset(int? initialPrice, bool isPositive)
 {
	if(initialPrice is null) return null;
	return isPositive ? positive(initialPrice) : negative(initialPrice);
}

private Func<int?, int?> positive = price => (int)((double)price > 200000 ? price * 1.20d : price * 1.10d);
private Func<int?, int?> negative = price => (int)((double)price > 200000 ? price / 1.20d : price / 1.10d);

protected void CheckDecline<TProp>(Foo request, Expression<Func<Foo, TProp>> expression, TProp failValue) =>
	CheckForFailure(request, expression, failValue);

protected void CheckDecline<TProp>(Foo request, Expression<Func<Foo, TProp>> expression, Predicate<TProp> failCondition) =>
	CheckForFailure(request, expression, failCondition);

private static void CheckForFailure<TProp>(
	Foo request,
	Expression<Func<Foo, TProp>> expression,
	TProp failValue
	) => CheckForFailure(request, expression, x => EqualityComparer<TProp>.Default.Equals(x, failValue));

private static void CheckForFailure<TProp>(Foo request, Expression<Func<Foo, TProp>> expression, Predicate<TProp> failCondition)
{
	var value = expression.Compile().Invoke(request ?? default);

	request.Dump();
	if (failCondition(value))
	{
		"fail".Dump();
	}
}

private static bool IsValidLastService(int? service) => service == null || service.Value > 2;

public class Foo
{
	public Bar Bar { get; set; } = new();
	public int? Baz { get; set; }
}

public class Bar
{
	public string Baz { get; set; } = string.Empty;
}