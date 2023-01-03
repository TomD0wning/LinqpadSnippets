<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
  <Namespace>BenchmarkDotNet.Attributes</Namespace>
  <Namespace>BenchmarkDotNet.Running</Namespace>
</Query>

#LINQPad optimize+

void Main()
{
	var summary = BenchmarkRunner.Run<ConvertAllvsSelectBenchmark>();
}

[MemoryDiagnoser]
public class ConvertAllvsSelectBenchmark
{
	[Params(10, 100, 1000)]
	public int Iterations { get; set; }

	public List<SampleClassDto> Items { get; set; } = new();

	[GlobalSetup]
	public void GlobalSetup()
	{
		Items.AddRange(Enumerable.Range(0, Iterations).Select((_, i) => new SampleClassDto(i)));
	}

	[Benchmark]
	public void ConvertAllTest()
	{
		var items = Items.ConvertAll(x => new SampleClass(x));
	}

	[Benchmark]
	public void SelectTest()
	{
		var items = Items.Select(x => new SampleClass(x)).ToList();
	}
}

public class SampleClass
{
	public string Id { get; set; }
	public string Caption { get; set; }
	public bool IsPrimary { get; set; }
	public string Uri { get; set; }
	public string Name { get; set; }
	public int FileSize { get; set; }
	public string FileType { get; set; }

	public SampleClass(SampleClassDto imageDto)
	{
		Id = imageDto.Id;
		Caption = imageDto.Caption;
		IsPrimary = imageDto.IsPrimary;
		Uri = imageDto.Uri;
		Name = imageDto.Name;
		FileSize = imageDto.FileSize;
		FileType = imageDto.FileType;
	}

	public void ProcessCost() { }
}

public class SampleClassDto
{
	public string Id { get; set; }

	public string Caption { get; set; } = "Some Caption";

	public bool IsPrimary { get; set; } = false;

	public string Uri { get; set; } = "/some/uri/to/an/image";

	public string Name { get; set; } = "AnImage";

	public int FileSize { get; set; } = 35046536;

	public string FileType { get; set; } = "jpg";

	public SampleClassDto(int i)
	{
		Id = i.ToString();
	}

	public void ProcessCost() { }
}

