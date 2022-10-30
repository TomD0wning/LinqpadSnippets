<Query Kind="Program" />

void Main()
{
	var tempString = "{\"question\": \"is the cake a lie?\", \"answer\": \"of course it was\"}";
	var y = CaesarCipher(tempString, 5);
	y.Dump();
	var n = CaesarCipher(y, -5);
	n.Dump();
}

public static string CaesarCipher(string inputString, int shiftOrUnshift)
{
	var charArray = inputString.ToCharArray();
	for (int i = 0; i < inputString.Length; i++)
	{
		charArray[i] =
			  Convert.ToChar((Convert.ToInt32(inputString[i]) + shiftOrUnshift + short.MaxValue) % short.MaxValue);
	}
	return new string(charArray);
}
