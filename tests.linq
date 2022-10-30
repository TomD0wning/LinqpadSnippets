<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
	double price = 40.12;
	
	var x = price.ToString("C0", new CultureInfo("en-GB"));
	
	x.Dump();
}

