<Query Kind="Program" />

void Main()
{

	var x = new Foo()
	{
		Prop1 = new Bar { Prop2 = 1 }
	};

	nameof(x.Prop1).Dump();
}

public class Foo
{
	public Bar Prop1 { get; set; }
}

public class Bar
{
	public int Prop2 { get; set; }
}