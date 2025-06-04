<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
</Query>

#LINQPad optimize+

void Main()
{
	BenchmarkRunner.Run<DecimalFormatting>();
}

[MemoryDiagnoser]
public class DecimalFormatting
{
	[Params(10, 100, 1000)]
	public int Iterations { get; set; }
	
	[Benchmark]
	public string DecimalToString()
	{
		return 35.00M.ToString("0");
	}

	[Benchmark]
	public string StringToDecimal()
	{
		var overide = "35.00";
		var span = overide.AsSpan();
		var i = span.IndexOf('.');
		return (i >= 0) ? span[..i].ToString() : overide;
	}
}

