<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Running</Namespace>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
</Query>

#LINQPad optimize+

void Main()
{
	var summary = BenchmarkRunner.Run<ConcatVsJoinBenchmark>();
}

[MemoryDiagnoser]
public class ConcatVsJoinBenchmark
{
	[Params(10, 100, 1000)]
	public int Iterations { get; set; }

	public List<string> BaseItems { get; set; } = new();
	public List<string> ItemsToAdd {get; set; } = new();

	[GlobalSetup]
	public void GlobalSetup()
	{
		BaseItems.AddRange(Enumerable.Range(0, Iterations).Select(_ => GenerateString()));
		BaseItems.AddRange(Enumerable.Range(0, Iterations).Select(_ => GenerateString()));
	}

	[Benchmark]
	public void ConcatTest()
	{
		for (int i = 0; i < BaseItems.Count; i++)
		{
			var item = string.Concat(BaseItems[i]," ", ItemsToAdd[i]); 
		}
	}

	[Benchmark]
	public void JoinTest()
	{
		for (int i = 0; i < BaseItems.Count; i++)
		{
			var item = string.Join(" ", BaseItems[i], ItemsToAdd[i]);
		}
	}
}

private static string GenerateString()
{
	return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 20);
}

