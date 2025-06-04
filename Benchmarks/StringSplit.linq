<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
</Query>

#LINQPad optimize+

void Main()
{
	BenchmarkRunner.Run<StringSplitBenchmark>();
}

[MemoryDiagnoser]
public class StringSplitBenchmark
{
	private readonly StringSpliters p = new StringSpliters();
	
	[Params(10, 100)]
	public int Iterations { get; set; }

	[Benchmark]
	public (string firstName, string lastName) B1()
	{
		return p.SplitName("abc cba foo");
	}

	[Benchmark]
	public (string firstName, string lastName) B2()
	{
		return p.SplitName1("abc cba foo");
	}
	
	public class StringSpliters
	{
		public (string firstName, string lastName) SplitName(string fullName)
		{
			var name = fullName.AsSpan();

			var index = name.LastIndexOf(' ');

			return index == -1
				? ((string firstName, string lastName))(fullName, string.Empty)
				: ((string firstName, string lastName))(name[..index].ToString(), name[(index + 1)..].ToString());
		}

		public (string firstName, string lastName) SplitName1(string fullName)
		{
			var index = fullName.LastIndexOf(' ');

			return index == -1
				? ((string firstName, string lastName))(fullName, string.Empty)
				: ((string firstName, string lastName))(fullName[..index], fullName[(index + 1)..]);
		}

	}
}

