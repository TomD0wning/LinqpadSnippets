<Query Kind="Program">
  <NuGetReference>Microsoft.AspNetCore.WebUtilities</NuGetReference>
  <Namespace>Microsoft.AspNetCore.WebUtilities</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

void Main()
{
	
}

/// <summary>
/// Encrypt or decrypt a string with the provided password. Uses aes 256 <a href="https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-6.0">MS aes docs</a>
/// </summary>
internal class StringEncryption
{
	private readonly string _defaultToken = "SomeDefaultToken";
	private readonly byte[] _key;

	/// <summary>
	/// For information hiding only, for encrypting sensitive information use the non-default constructor <see cref="StringEncryption(string)"/>
	/// </summary>
	public StringEncryption()
	{
		_key = HashToken(_defaultToken);
	}

	public StringEncryption(string token)
	{
		_key = HashToken(token);
	}

	/// <summary>
	/// Encrypts a string using the specified key
	/// </summary>
	/// <param name="plainTextString"></param>
	/// <returns>An encrypted string encoded as base64</returns>
	public string EncryptString(string plainTextString)
	{
		if (string.IsNullOrEmpty(plainTextString))
		{
			return null;
		}

		var buffer = Encoding.UTF8.GetBytes(plainTextString);

		using var inputStream = new MemoryStream(buffer, false);
		using var outputStream = new MemoryStream();
		using var aes = Aes.Create();

		aes.Key = _key;

		var vector = aes.IV;
		outputStream.Write(vector, 0, vector.Length);
		outputStream.Flush();

		var encryptor = aes.CreateEncryptor(_key, vector);
		using (var cryptoStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write))
		{
			inputStream.CopyTo(cryptoStream);
		}

		return WebEncoders.Base64UrlEncode(outputStream.ToArray());
	}

	/// <summary>
	/// Decrypts a base64 encoded string using the specified key
	/// </summary>
	/// <param name="encryptedString"></param>
	/// <returns>The encrypted string as UTF-8</returns>
	public string DecryptString(string encryptedString)
	{
		if (string.IsNullOrEmpty(encryptedString))
		{
			return null;
		}

		try
		{
			var buffer = WebEncoders.Base64UrlDecode(encryptedString);

			using var inputStream = new MemoryStream(buffer, false);
			using var outputStream = new MemoryStream();
			using var aes = Aes.Create();

			aes.Key = _key;

			var vector = new byte[16];
			var bytesRead = inputStream.Read(vector, 0, 16);
			if (bytesRead < 16)
			{
				throw new CryptographicException("invalid initialisation vector");
			}

			var decryptor = aes.CreateDecryptor(_key, vector);
			using (var cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
			{
				cryptoStream.CopyTo(outputStream);
			}

			return Encoding.UTF8.GetString(outputStream.ToArray());
		}
		catch
		{
			return string.Empty;
		}
	}

	private byte[] HashToken(string token)
	{
		using var sha = SHA256.Create();
		return sha.ComputeHash(Encoding.UTF8.GetBytes(token));
	}
}
