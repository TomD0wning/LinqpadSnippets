<Query Kind="Program" />

void Main()
{
	int[] numbers = { 10, 20, 30, 40, 50 };

	unsafe
	{
		fixed (int* ptr = numbers)
		{
			for (int i = 0; i < numbers.Length; i++)
			{
				Console.WriteLine($"Value at index {i}: {*(ptr + i)}");
				*(ptr + i) += 1;
			}
		}
	}

	foreach (int n in numbers)
	{
		Console.WriteLine(n);
	}
}

