<Query Kind="Program" />

void Main()
{
	DateTime.Now.Dump();
	var n = new DateTime(2021, 05, 20, 12, 20, 56);
	var now = new DateTime(2021, 05, 21, 12, 0, 0);
	var x = DateTime.Now.AddDays(7);
	var res = ValidThreeDayInterval(n);
	res.Dump();
}

public bool ValidThreeDayInterval(DateTime ts)
{
	var now = new DateTime(2021, 05, 21, 12, 0, 0);

	if ((now - ts).TotalHours < 25)
	{
		return false;
	}

	var elaspedTime = Math.Floor((now - ts).TotalDays).Dump();

	return elaspedTime % 3 == 0;
}