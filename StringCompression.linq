<Query Kind="Program">
  <Namespace>System.IO.Compression</Namespace>
</Query>

void Main()
{
	var y = "<table class=\"EmailTable\"><thead><tr><th>Header</th></thead><tbody><tr><td>Cell</td></tr></tbody></table>";

	var x = CompressString(y).Dump();
	DecompressString(x).Dump();
}

public static string CompressString(string text)
{
	byte[] buffer = Encoding.UTF8.GetBytes(text);
	var memoryStream = new MemoryStream();
	using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
	{
		gZipStream.Write(buffer, 0, buffer.Length);
	}

	memoryStream.Position = 0;

	var compressedData = new byte[memoryStream.Length];
	memoryStream.Read(compressedData, 0, compressedData.Length);

	var gZipBuffer = new byte[compressedData.Length + 4];
	Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
	Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
	return Convert.ToBase64String(gZipBuffer);
}

//Pre .net6 https://docs.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/6.0/partial-byte-reads-in-streams
public static string DecompressString(string compressedText)
{
	byte[] gZipBuffer = Convert.FromBase64String(compressedText);
	using (var memoryStream = new MemoryStream())
	{
		int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
		memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

		var buffer = new byte[dataLength];

		memoryStream.Position = 0;
		using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
		{
			gZipStream.Read(buffer, 0, buffer.Length);
		}

		return Encoding.UTF8.GetString(buffer);
	}
}

// post .net6
public string Decompress(string compressedString)
{
	byte[] byteStream = Convert.FromBase64String(compressedString);

	using var mStream = new MemoryStream();

	int bufferLength = BitConverter.ToInt32(byteStream, 0);
	mStream.Write(byteStream, 4, byteStream.Length - 4);

	var buffer = new byte[bufferLength];

	mStream.Position = 0;

	using (var gStream = new GZipStream(mStream, CompressionMode.Decompress))
	{
		int totalRead = 0;
		while (totalRead < buffer.Length)
		{
			int bytesRead = gStream.Read(buffer, totalRead, buffer.Length - totalRead);
			if (bytesRead == 0) break;
			totalRead += bytesRead;
		}
	}

	return Encoding.UTF8.GetString(buffer);
}

