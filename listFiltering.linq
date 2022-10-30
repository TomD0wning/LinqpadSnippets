<Query Kind="Program" />

void Main()
{
	var tempList = new List<randoObj>()
	{new randoObj()
		{intID = 1,strID = "abc"},
		new randoObj()
		{intID = 2,strID = "def"},
		new randoObj()
		{intID = 1,strID = "abc"}};
	
	var filteredList = tempList.Where(l => l.strID != "abc");

	var x = new objWithList();
	
	x.listOfRandos = tempList;
	
	
	x.Dump();
	
	filteredList.Dump();
}

public class randoObj
{
	public string strID { get; set; }
	public int intID { get; set; }
}

public class objWithList
{
	public List<randoObj> listOfRandos{get; set;}
}

public class someOtherObj
{
	public string id { get; set; }
	public string path { get; set; }
}