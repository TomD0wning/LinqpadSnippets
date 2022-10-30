<Query Kind="Program" />

void Main()
{

	var x = new AddUpdateListing()
	{
		Listing = new SearchableAdvertListing { Listing = 1 }
	};
	
	nameof(x.Listing).Dump();
}

public class AddUpdateListing
{
	public SearchableAdvertListing Listing { get; set; }
}

public class SearchableAdvertListing
{
	public int Listing { get; set; }
}