<Query Kind="Program" />

void Main()
{
	Func<int,string> toBinary = number => Convert.ToString(number, 2).PadLeft(8,'0').ToString();
	
	toBinary(2);
	
	for (int i = 0; i < 15; i++)
	{
		toBinary(i);
	}
}