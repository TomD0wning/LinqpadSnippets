<Query Kind="Program" />

void Main()
{
	var testList = new List<SearchableAdvertListing>();
	for (int i = 0; i < 100; i++)
	{
		testList.Add(searchable_advert_listing);
	}

	UpdateFlag(ref testList);
	testList.Dump();

}

// You can define other methods, fields, classes and namespaces here

public void UpdateFlag<T>(ref List<T> someList) where T : SearchableListingBase
{
	foreach (var item in someList)
	{
		item.IsHidden ^= true;
	}
}

public static SearchableAdvertListing searchable_advert_listing => new SearchableAdvertListing
{ BrandUri = "", BranchUri = "", ListingAliasId = Guid.NewGuid().ToString(), Postcode = "", Town = "", BranchTier = BranchTier.Standard, PublishedDate = DateTime.UtcNow, PropertyStatus = PropertyStatus.Available, IsHidden = true };


public class SearchableAdvertListing : SearchableListingBase
{
	public string ListingUri { get; set; }
	public string BrandUri { get; set; }
	public string PriceQualifier { get; set; }
	public string PrimaryImage { get; set; }
	public BranchTier? BranchTier { get; set; }
	public bool NewBuild { get; set; }
	public string[] KeyFeatures { get; set; }
	public string BrandName { get; set; }
	public string BranchName { get; set; }
	public string[] Parking { get; set; }
	public bool HasParking { get; set; }
	public PropertyStatus PropertyStatus { get; set; }
	public string Description { get; set; }
	public string[] ImageUrls { get; set; }
	public bool IsStudentAccommodation { get; set; }
	public bool IsHouseShare { get; set; }
	public DateTimeOffset? LettingDateAvailable { get; set; }
	public string LetType { get; set; }
}

public class SearchableListingBase
{
	public string ListingAliasId { get; set; }
	public string BranchUri { get; set; }
	public int Price { get; set; }
	public string BuildingNumber { get; set; }
	public string BuildingName { get; set; }
	public string PropertyType { get; set; }
	public string PropertyStyle { get; set; }
	public string StreetName { get; set; }
	public string Town { get; set; }
	public string Postcode { get; set; }
	public int Bedrooms { get; set; }
	public int Bathrooms { get; set; }
	public decimal Latitude { get; set; }
	public decimal Longitude { get; set; }
	public bool IsLetting { get; set; }
	public DateTime PublishedDate { get; set; }
	public bool IsHidden { get; set; }
}


public enum BranchTier
{
	Platinum,
	Gold,
	Standard
}

public enum PropertyStatus
{
	Unspecified,
	Available,
	SSTC,
	SSTCM,
	UnderOffer,
	Reserved,
	LetAgreed
}