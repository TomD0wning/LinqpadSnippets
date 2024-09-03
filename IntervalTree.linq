<Query Kind="Program" />

void Main()
{
	
}

public class Interval
{
	public int Start { get; set; }
	public int End { get; set; }

	public Interval(int start, int end)
	{
		Start = start;
		End = end;
	}

	public bool Overlaps(Interval other)
	{
		return Start <= other.End && End >= other.Start;
	}

	public override string ToString()
	{
		return $"[{Start}, {End}]";
	}
}

public class IntervalNode
{
	public Interval Interval { get; set; }
	public int MaxEnd { get; set; }
	public IntervalNode Left { get; set; }
	public IntervalNode Right { get; set; }

	public IntervalNode(Interval interval)
	{
		Interval = interval;
		MaxEnd = interval.End;
	}
}

public class IntervalTree
{
	private IntervalNode root;

	public void Insert(Interval interval)
	{
		root = Insert(root, interval);
	}

	private IntervalNode Insert(IntervalNode node, Interval interval)
	{
		if (node == null)
			return new IntervalNode(interval);

		int l = node.Interval.Start;

		if (interval.Start < l)
			node.Left = Insert(node.Left, interval);
		else
			node.Right = Insert(node.Right, interval);

		if (node.MaxEnd < interval.End)
			node.MaxEnd = interval.End;

		return node;
	}

	public IEnumerable<Interval> Query(Interval interval)
	{
		return Query(root, interval).ToList();
	}

	private IEnumerable<Interval> Query(IntervalNode node, Interval interval)
	{
		List<Interval> result = new List<Interval>();

		if (node == null)
			return result;

		if (node.Left != null && node.Left.MaxEnd >= interval.Start)
			result.AddRange(Query(node.Left, interval));

		if (node.Interval.Overlaps(interval))
			result.Add(node.Interval);

		if (node.Right != null && node.Right.Interval.Start <= interval.End)
			result.AddRange(Query(node.Right, interval));

		return result;
	}
}


