<Query Kind="Program" />

void Main()
{
	//for teaching about exceptions
	boundryExceptions();
	loopExceptions();

}

public void boundryExceptions()
{
	var emptyList = new List<int>();
	try
	{
		if (emptyList == null || emptyList.Count() == 0)
			throw new ArgumentNullException();


	}
	catch (Exception e)
	{
		throw new Exception();
	}
}

public void loopExceptions()
{
	var someList = new List<int>() { 1, 2, 1 };
	foreach (var element in someList)
	{
		try
		{
			if (element == 1)
			{
				Console.WriteLine(element.ToString());
			}
			else
			{
				throw new ArgumentException("oh no");
			}
		}
		catch
		{
			//will keep going
			Console.WriteLine("not so bad");
			//will stop 
			new ArgumentException(":(");
			//will throw orginal exception
			throw;
		}
	}
}

