<Query Kind="Program" />

void Main()
{
	ApplyOffset(200000, true).Dump();
	ApplyOffset(100000, false).Dump();
}

 int? ApplyOffset(int? initialPrice, bool isPositive){
	if(initialPrice is null) return null;
	
	return isPositive ? positive(initialPrice)
	: negative(initialPrice);
	
	}

private Func<int?, int?> positive = price => (int)((double)price > 200000 ? price * 1.20d : price * 1.10d);
private Func<int?, int?> negative = price => (int)((double)price > 200000 ? price / 1.20d : price / 1.10d);
