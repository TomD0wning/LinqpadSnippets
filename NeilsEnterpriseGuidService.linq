<Query Kind="Program" />

void Main()
{
	string input = Util.ReadLine("# GUIDs to generate: ");

	if (!int.TryParse(input, out int numberOfGuidsToGenerate) || numberOfGuidsToGenerate <= 0)
	{
		"Invalid input. If the issue persists, raise a service desk support ticket".Dump();
		return;
	}

	IGuidGeneratorFactory factory = new GuidGeneratorFactory();
	var service = new NeilsEnterpriseGuidService(factory);
	var guids = service.GenerateMultipleGuids(numberOfGuidsToGenerate);
	guids.Dump();
}

public interface IGuidGenerator
{
	Guid GenerateGuid();
}

public class GuidGenerator : IGuidGenerator
{
	public Guid GenerateGuid()
	{
		return Guid.NewGuid();
	}
}

public interface IGuidGeneratorFactory
{
	IGuidGenerator CreateGenerator();
}

public class GuidGeneratorFactory : IGuidGeneratorFactory
{
	public IGuidGenerator CreateGenerator()
	{
		return new GuidGenerator();
	}
}

public abstract class GuidService
{
	protected IGuidGenerator Generator { get; }

	protected GuidService(IGuidGenerator generator)
	{
		Generator = generator;
	}

	public abstract IEnumerable<Guid> GenerateMultipleGuids(int count);
}

public class NeilsEnterpriseGuidService : GuidService
{
	public NeilsEnterpriseGuidService(IGuidGeneratorFactory factory)
		: base(factory.CreateGenerator())
	{
	}

	public override IEnumerable<Guid> GenerateMultipleGuids(int count)
	{
		var guids = new List<Guid>();
		for (int i = 0; i < count; i++)
		{
			guids.Add(Generator.GenerateGuid());
		}
		return guids;
	}
}