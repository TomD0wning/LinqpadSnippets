<Query Kind="Program" />

void Main()
{

	arrayShift(new int[] { 3, 8, 9, 7, 6 }, 3).Dump();
	var y = "abcax".ToCharArray();
	var x = "xacba".Reverse();

	x.Dump();
	y.Dump();

	var n = x.SequenceEqual(y);
	n.Dump();

}

public int[] arrayShift(int[] a, int k)
{
	var x = 0;

	for (int n = 0; n < k; n++)
	{
		for (int i = 0; i < a.Length; i++)
		{
			var y = a[i];

			if (i == 0)
			{
				x = a[a.Length - 1];
			}

			a[i] = x;
			x = y;
		}
	}
	return a;
}