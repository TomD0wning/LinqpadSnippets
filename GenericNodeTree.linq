<Query Kind="Program" />

void Main()
{
	var NodeTree = new Tree<Foo>(new Foo());
}

public class Foo
{
	public Foo()
	{
		Bar = 1;
		Baz = "someString";
	}
	public int Bar { get; set; }
	public string Baz { get; set; }	= string.Empty;
}

public class Node<T>
{
	public T Value { get; set; }
	public List<Node<T>> Children { get; set; } = new List<Node<T>>();

	public Node(T value)
	{
		Value = value;
	}

	public void AddChild(Node<T> child)
	{
		Children.Add(child);
	}
}

public class Tree<T>
{
	public Node<T> Root { get; set; }

	public Tree(T rootValue)
	{
		Root = new Node<T>(rootValue);
	}

	public void Traverse(Action<T> action)
	{
		Traverse(Root, action);
	}

	public List<Node<T>> FindNodes(Func<T, bool> criteria)
	{
		return FindNodes(Root, criteria);
	}

	private void Traverse(Node<T> node, Action<T> action)
	{
		action(node.Value);
		foreach (var child in node.Children)
		{
			Traverse(child, action);
		}
	}

	private List<Node<T>> FindNodes(Node<T> node, Func<T, bool> criteria)
	{
		var result = new List<Node<T>>();
		if (criteria(node.Value))
		{
			result.Add(node);
		}
		foreach (var child in node.Children)
		{
			result.AddRange(FindNodes(child, criteria));
		}
		return result;
	}
}