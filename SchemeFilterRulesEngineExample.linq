<Query Kind="Program" />

void Main()
{
	var schemes = new SchemesWrapper
	{
		Schemes = new()
		{
			new Scheme { Foo = "a", Bar = true },
			new Scheme { Foo = "b", Bar = true },
			new Scheme { Foo = "c", Bar = false },
			new Scheme { Foo = "a", Bar = true }
		}
	};

	var engine = new SchemeFilterEngine();
	engine.AddRule(new FooFilterRule("a", "b"));
	engine.AddRule(new BarFilterRule());

	var filteredSchemes = engine.ApplyFilters(schemes);

	foreach (var scheme in filteredSchemes.Schemes)
	{
		Console.WriteLine($"Foo: {scheme.Foo}, Bar: {scheme.Bar}");
	}
}

public interface ISchemeFilterRule
{
	bool IsMatch(SchemesWrapper schemes);
	SchemesWrapper ApplyFilter(SchemesWrapper schemes);
}

public sealed class FooFilterRule : ISchemeFilterRule
{
	public FooFilterRule(params string[] fooFilter) => FooFilter = fooFilter;

	public string[] FooFilter { get; set; }

	public SchemesWrapper ApplyFilter(SchemesWrapper schemes) => new SchemesWrapper { Schemes = schemes.Schemes.Where(s => FooFilter.Contains(s.Foo)).ToList() };

	public bool IsMatch(SchemesWrapper schemes) => schemes.Schemes.Any(s => FooFilter.Contains(s.Foo));
}

public sealed class BarFilterRule : ISchemeFilterRule
{
	public bool IsMatch(SchemesWrapper schemes) => schemes.Schemes.Any(s => s.Bar);

	public SchemesWrapper ApplyFilter(SchemesWrapper schemes) => new SchemesWrapper { Schemes = schemes.Schemes.Where(s => s.Bar).ToList() };
}

public class Scheme
{
	public string Foo { get; set; }
	public bool Bar { get; set; }
}

public sealed class SchemesWrapper
{
	public List<Scheme> Schemes { get; set; }
}

public sealed class SchemeFilterEngine
{
	private readonly List<ISchemeFilterRule> _rules;

	public SchemeFilterEngine() => _rules = new List<ISchemeFilterRule>();

	public void AddRule(ISchemeFilterRule rule) => _rules.Add(rule);

	public SchemesWrapper ApplyFilters(SchemesWrapper schemes)
	{
		foreach (var rule in _rules)
		{
			if (rule.IsMatch(schemes))
			{
				schemes = rule.ApplyFilter(schemes);
			}
		}

		return schemes;
	}
}