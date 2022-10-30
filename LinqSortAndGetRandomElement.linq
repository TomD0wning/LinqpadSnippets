<Query Kind="Program">
  <NuGetReference>BenchmarkDotNet</NuGetReference>
</Query>

void Main()
{
	var branchList = new List<branch>{
	new branch{
		branchUri = "1",
		IsOffline = false,
		Distance = 5,
	},
	new branch{
		branchUri = "2",
		IsOffline = true,
		Distance = 3,
	},
	new branch{
		branchUri = "3",
		IsOffline = false,
		Distance = 2,
	},
	new branch{
		branchUri = "4",
		IsOffline = true,
		Distance = 1,
	},
	new branch{
		branchUri = "5",
		IsOffline = false,
		Distance = 6,
	},
	};
	
	var branchRatings = new List<branchRating>{
		new branchRating{
			branchUri = "5",
			Rating = 4
		},
		new branchRating{
			branchUri = "4",
			Rating = 1
		},
		new branchRating{
			branchUri = "3",
			Rating = 3
		},
		new branchRating{
			branchUri = "2",
			Rating = 1
		},
		new branchRating{
			branchUri = "1",
			Rating = 6
		}
		
	};

	var x = branchList.Join(branchRatings, a => a.branchUri,
	r => r.branchUri,
	(a, r) => new { a.branchUri, a.Distance, a.IsOffline, r.Rating })
	.OrderBy(l => l.IsOffline)
	.ThenBy(l => l.Distance)
	.Take(5)
	.OrderBy(l => l.IsOffline)
	.ThenBy(l => l.Rating);
	x.Dump();
	
	x.Take(1).Dump();
}

public class branch
{
	public string branchUri { get; set; }
	public bool IsOffline { get; set; }
	public int Distance { get; set; }
}

public class branchRating
{
	public string branchUri { get; set; }
	public int Rating { get; set; }
}



