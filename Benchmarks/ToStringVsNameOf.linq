<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
</Query>

#LINQPad optimize+

void Main()
{
	BenchmarkRunner.Run<EnumToString>();
}

[MemoryDiagnoser]
public class EnumToString
{
	[Benchmark]
	public string GetStringEnum()
	{
		return Foo.Bar.ToString();
	}

	[Benchmark]
	public string GetNameOfEnum()
	{
		return nameof(Foo.Bar);
	}
}

public enum Foo
{
	Bar
}

