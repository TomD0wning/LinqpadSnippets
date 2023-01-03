<Query Kind="Program" />

void Main()
{
	var testList = new List<Foo>();
	for (int i = 0; i < 100; i++)
	{
		testList.Add(foo);
	}

	UpdateFlag(ref testList);
	testList.Any(l => l.IsHidden).Dump();

}

public void UpdateFlag<T>(ref List<T> someList) where T : Foo
{
	foreach (var item in someList)
	{
		item.IsHidden ^= true;
	}
}

public static Foo foo => new Foo
{ Id = Guid.NewGuid().ToString(), IsHidden = true };


public class Foo
{
	public string Id { get; set; }
	public bool IsHidden { get; set; }
}
