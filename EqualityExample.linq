<Query Kind="Program">
  <Namespace>System.Numerics</Namespace>
</Query>

void Main()
{
    var p1 = new Point(1, 2);
    var p2 = new Point(1, 2);

	var v1 = new ValuePoint(1, 2);
	var v2 = new ValuePoint(1, 2);

	var r1 = new PointRecord(1, 2);
	var r2 = new PointRecord(1, 2);

	var s1 = new PointStruct(1, 2);
	var s2 = new PointStruct(1, 2);

	DumpResults("Class (default)", p1, p2);
	DumpResults("Class with equality overrides", v1, v2);
	DumpResults("Record", r1, r2);
	DumpResults("Struct", s1, s2);
}

void DumpResults<T>(string title, T a, T b)
{
	Console.WriteLine($"- {title} -");
	Console.WriteLine($"a == b        : {SafeEqualsOperator(a, b)}");
	Console.WriteLine($"a.Equals(b)   : {a?.Equals(b)}");
	Console.WriteLine($"ReferenceEquals: {ReferenceEquals(a, b)}");
	Console.WriteLine();
}

bool SafeEqualsOperator<T>(T a, T b)
{
	try { return (dynamic)a == (dynamic)b; }
	catch { return false; }
}

class Point
{
	public int X { get; }
	public int Y { get; }

	public Point(int x, int y) => (X, Y) = (x, y);
}

class ValuePoint
{
	public int X { get; }
	public int Y { get; }

	public ValuePoint(int x, int y) => (X, Y) = (x, y);

	public override bool Equals(object? obj) =>
		obj is ValuePoint other && X == other.X && Y == other.Y;

	public override int GetHashCode() => HashCode.Combine(X, Y);

	public static bool operator ==(ValuePoint left, ValuePoint right) =>
		left.Equals(right);

	public static bool operator !=(ValuePoint left, ValuePoint right) =>
		!left.Equals(right);
}

record PointRecord(int X, int Y);

readonly struct PointStruct
{
	public int X { get; }
	public int Y { get; }

	public PointStruct(int x, int y) => (X, Y) = (x, y);
}
