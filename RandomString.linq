<Query Kind="Program" />

public static string RandomString()
{
	return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 10).Dump();
}