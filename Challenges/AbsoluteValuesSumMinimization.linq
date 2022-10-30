<Query Kind="Program" />

void Main()
{
	
}

public int AbsoluteValuesSumMinimization(int[] inputArray){
	int minValue = 0;
	int max = int.MaxValue;
	int score = 0;
	for (int i = 0; i < inputArray.Length -1; i++)
	{
		minValue = inputArray[i] - inputArray[i+1];
		if(minValue < max){
			score = minValue;	
		}
	}
	return score;
}
