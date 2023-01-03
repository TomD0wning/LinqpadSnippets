<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Running</Namespace>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
</Query>

#LINQPad optimize+

void Main()
{
	var runSummary = BenchmarkRunner.Run<CodeUnderTest>();
}

[MemoryDiagnoser]
public class CodeUnderTest
{
	[Params(10, 100, 1000)]
	public int TestRunCount { get; set; }

	[GlobalSetup]
	public void GlobalSetup()
	{

	}
}

