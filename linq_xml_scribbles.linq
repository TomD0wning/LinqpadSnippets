<Query Kind="Program" />

async Task Main()
{
	//await ParseEuXmlStream();
	//await ParseOfacStream();
	await ParseHmtStream();
}

public async Task ParseHmtStream()
{
	var entList = new List<SanctionedEntity>();
	var nameList = new List<SanctionedEntityName>();

	var fileName = @"C:\apps\temp\ConList.xml";
	//var downloadedStream = await GetFileData("https://ofsistorage.blob.core.windows.net/publishlive/2022format/ConList.xml");

	var doc = XElement.Load(fileName);

	var res = FilterElementsByChild(doc, f => f.Value == "Individual");

	foreach (var node in res)
	{
		entList.Add(new SanctionedEntity(node.ToString()));
	}

	entList.Count.Dump();
	nameList.Count.Dump();
}

public async Task ParseOfacStream()
{
	var entList = new List<SanctionedEntity>();
	var nameList = new List<SanctionedEntityName>();

	var fileName = @"C:\apps\temp\sdn_advanced.xml";
	//var downloadStream = await GetFileData("https://www.treasury.gov/ofac/downloads/sanctions/1.0/sdn_advanced.xml");

	var doc = XElement.Load(fileName);

	var res = FilterElementsByChild(doc, xe => xe.Attribute("PartySubTypeID") != null && xe.Attribute("PartySubTypeID").Value == "4");

	foreach (var node in res)
	{
		entList.Add(new SanctionedEntity(node.ToString()));
	}

	entList.Count.Dump();
	nameList.Count.Dump();
}

public async Task ParseEuXmlStream()
{
	var entList = new List<SanctionedEntity>();
	var nameList = new List<SanctionedEntityName>();

	var fileName = @"C:\apps\temp\eu_data.xml";
	//var downloadStream = await GetFileData("https://webgate.ec.europa.eu/fsd/fsf/public/files/xmlFullSanctionsList_1_1/content?token=dG9rZW4tMjAxNw");

	var doc = XElement.Load(fileName);

	var res = FilterElementsByChild(doc, d => d.Attribute("code") != null && d.Attribute("code").Value == "person");

	foreach (var node in res)
	{
		nameList.AddRange(node.Descendants().Where(x => x.Attribute("wholeName") != null).Select(x => new SanctionedEntityName(x.Attribute("firstName").Value, x.Attribute("lastName").Value, x.Attribute("wholeName").Value)));
		entList.Add(new SanctionedEntity(node.ToString()));
	}

	entList.Count.Dump();
	nameList.Count.Dump();
}

private IEnumerable<XElement> FilterElementsByChild(XElement doc, Func<XElement, bool> elementFilter)
{
	return doc.Descendants()
		.Where(x => elementFilter(x))
		.Select(d => d.Parent);
}

private async Task<Stream> GetFileData(string downloadUrl)
{
	var client = new HttpClient();
	var resp = await client.GetAsync(downloadUrl);
	var respStream = await resp.Content.ReadAsStreamAsync();

	return respStream;
}

public class SanctionedEntity
{
	public SanctionedEntity(string src)
	{
		RawSource = src;
	}

	public int Id { get; set; }
	public int SanctionFileId { get; set; }
	public int FilePosition { get; set; }
	public int SourceId { get; set; }
	public string RawSource { get; set; }
}

public class SanctionedEntityName
{
	public SanctionedEntityName(string firstName, string lastName, string fullName)
	{
		FirstName = firstName;
		LastName = lastName;
		FulllName = firstName == null ? string.Join(' ', firstName, lastName) : fullName;
	}

	public int Id { get; set; }
	public int SanctionedEntityId { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string FulllName { get; set; }
}

