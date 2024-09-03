<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
</Query>

#LINQPad optimize+

public static void Main(string[] args)
{
	var summary = BenchmarkRunner.Run<SpaceReplacementBenchmarks>();
}

public class SpaceReplacementBenchmarks
{
    private const string testString = "This   is  a   test     string.";
    
    private static readonly Regex compiledRegex = new Regex("\\s+", RegexOptions.Compiled);

    [Benchmark]
    public string ReplaceUsingCompiledRegex()
    {
        return compiledRegex.Replace(testString, " ");
    }

    [Benchmark]
    public string ReplaceUsingStringMethod()
    {
        string result = testString.Replace("  ", " ");
        while (result.Contains("  "))
		{
			result = result.Replace("  ", " ");
		}
		return result;
	}
}
