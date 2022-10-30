<Query Kind="Program">
  <NuGetReference>MassTransit</NuGetReference>
  <Namespace>System.Security.Cryptography</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>MassTransit.Serialization</Namespace>
  <Namespace>Newtonsoft.Json.Bson</Namespace>
</Query>

string DecryptMessageBody(string filePath, string key)
{
	using (Stream bodyStream = new MemoryStream(File.ReadAllBytes(filePath)))
	using (Stream decryptStream = GetDecryptStream(bodyStream, Convert.FromBase64String(key)))
	using (BsonDataReader bsonDataReader = new BsonDataReader(decryptStream))
	{
		var envelope = BsonMessageSerializer.Deserializer.Deserialize<MessageEnvelope>(bsonDataReader);
		var jsonBody = JsonConvert.SerializeObject(envelope.Message, Newtonsoft.Json.Formatting.Indented);
		return JsonConvert.SerializeObject(envelope.Message, Newtonsoft.Json.Formatting.Indented);
	}
}

void DecryptFolder(string folderPath, string key)
{
	foreach (var fileName in Directory.GetFiles(folderPath))
	{
		var decrypted = DecryptMessageBody(fileName, key);
		File.WriteAllText(fileName.Replace(".body", ".json"), decrypted);
	}
}

private static CryptoStream GetDecryptStream(Stream stream, byte[] key)
{
	var numArray = new byte[16];
	stream.Read(numArray, 0, numArray.Length);
	var decryptor = CreateDecryptor(key, numArray);
	return new CryptoStream(stream, decryptor, CryptoStreamMode.Read);
}

private static ICryptoTransform CreateDecryptor(byte[] key, byte[] iv)
{
	using var cryptoServiceProvider = Aes.Create();
	cryptoServiceProvider.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
	return cryptoServiceProvider.CreateDecryptor(key, iv);
}