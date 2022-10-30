<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

void Main()
{
	using var sha = SHA256.Create();
	var token = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes($"somestring"))).Replace("/", "_");
	token.Dump();
}

