<Query Kind="Program" />

void Main()
{
	var postcode = "W1D 888";
	var streetName = "Oxford Street";
	var town = "Rugby";
	var res = AddressMapping.BuildAddress(streetName, town, postcode);
	res.Dump();
	
}

public static class AddressMapping
{
	private const char CommaDeliminator = ',';
	private const char SpaceDeliminator = ' ';

	public static string BuildAddress(string streetName, string town, string postcode)
	{
		var bufferState = new AddressData(streetName, town, postcode);

		var bufferLength = bufferState.StreetName.Length + bufferState.Town.Length + bufferState.Outcode.Length + 4;

		return string.Create(bufferLength, bufferState, (charBuffer, state) =>
		{
			var cursor = 0;

			if (state.StreetName.Length > 0)
			{
				state.StreetName.AsSpan().CopyTo(charBuffer);
				cursor += state.StreetName.Length;

				charBuffer[cursor++] = CommaDeliminator;
				charBuffer[cursor++] = SpaceDeliminator;
			}

			if (state.Town.Length > 0)
			{
				state.Town.AsSpan().CopyTo(charBuffer[cursor..]);
				cursor += state.Town.Length;

				charBuffer[cursor++] = CommaDeliminator;
				charBuffer[cursor++] = SpaceDeliminator;
			}

			state.Outcode.AsSpan().CopyTo(charBuffer[cursor..]);
		});
	}

	private static string getOutcode(string postcode)
	{
		if (string.IsNullOrEmpty(postcode))
		{
			return string.Empty;
		}
		
		var trimmedPostcode = postcode.Replace(" ", string.Empty);
		var outcode = trimmedPostcode.AsSpan();
		if (outcode.Length > 4)
		{
			return outcode[0..(outcode.Length - 3)].ToString();
		}

		return postcode;
	}

	protected internal struct AddressData
	{
		public AddressData(string streetName, string town, string postCode)
		{
			StreetName = streetName ?? string.Empty;
			Town = town ?? string.Empty;
			Outcode = getOutcode(postCode);
		}

		public string StreetName { get; set; }
		public string Town { get; set; }
		public string Outcode { get; set; }
	}
}

