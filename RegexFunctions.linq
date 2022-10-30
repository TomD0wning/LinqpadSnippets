<Query Kind="Program" />

void Main()
{
	string x = "SomePascalCaseString";
	var stringSplit = Regex.Replace(x, @"(?<=[A-Za-z])(?=[A-Z][a-z])|(?<=[a-z0-9])(?=[0-9]?[A-Z])", " ");
	stringSplit.Dump();
}