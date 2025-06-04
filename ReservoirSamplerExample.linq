<Query Kind="Program" />

void Main()
{
	int[] pop = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
	int sampleCount = 3;
	ReservoirSampler<int> sampler = new ReservoirSampler<int>(sampleCount);

	for (int i = 0; i < pop.Length; i++)
	{
		sampler.Add(pop[i], i + 1);
	}

	foreach (var item in sampler.GetSample())
	{
		item.Dump();
	}
}

public class ReservoirSampler<T>
{
	private int count;
	private List<T> reservoir;

	public ReservoirSampler(int sampleCount)
	{
		count = sampleCount;
		reservoir = new List<T>();
	}

	public void Add(T item, int n)
	{
		if (reservoir.Count < count)
		{
			reservoir.Add(item);
		}
		else
		{
			Random rnd = new Random();
			var items = rnd.Next(n);
			if (items < count)
			{
				reservoir[items] = item;
			}
		}
	}

	public List<T> GetSample()
	{
		return reservoir;
	}
}