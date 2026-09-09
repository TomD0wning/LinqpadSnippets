<Query Kind="Program">
  <Namespace>System.Numerics</Namespace>
</Query>

void Main()
{
	var limit = 56;

	const string header =
		"| Fibonacci(n) | Result       | Time (ms) | Memory (bytes) |\n" +
		"|--------------|--------------|-----------|----------------|";

	Console.WriteLine();
	Console.WriteLine(header);

	GC.Collect();
	GC.WaitForPendingFinalizers();
	GC.Collect();

	var startingMemory = GC.GetTotalMemory(true);
	var watch = Stopwatch.StartNew();

	var result = Fibonacci(limit);

	watch.Stop();

	var elapsed = watch.ElapsedTicks * (1000000.0 / Stopwatch.Frequency);
	var finalMemory = GC.GetTotalMemory(true);
	var memoryUsed = finalMemory - startingMemory;

	var resultOutput = string.Create(72, (limit, result, elapsed, memoryUsed),
		static (span, state) =>
		{
			var (n, result, time, memory) = state;
			var s = FormattableString.Invariant(
				$"| {n,-12} | {result,-12} | {time,-9} | {memory,-14:N0} |");
			s.AsSpan().CopyTo(span);
		});

	Console.WriteLine(resultOutput);
}

static BigInteger Fibonacci(int n)
{
	if (n == 0) return 0;
	if (n == 1) return 1;

	BigInteger a = 0;
	BigInteger b = 1;

	for (int i = 2; i <= n; i++)
	{
		BigInteger temp = a + b;
		a = b;
		b = temp;
	}

	return b;
}

