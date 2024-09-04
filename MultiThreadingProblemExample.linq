<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	await Threading.Run();
}

public class Threading
{
	private static int _counter = 0;

	public static async Task Run()
	{
		Task task1 = Task.Run(() => IncrementCounter());
		Task task2 = Task.Run(() => IncrementCounter());

		await Task.WhenAll(task1, task2);

		Console.WriteLine($"Value: {_counter}");
		_counter = 0;
	}

	static void IncrementCounter()
	{
		for (int i = 0; i < 10000; i++)
		{
			_counter++;
		}
	}
}