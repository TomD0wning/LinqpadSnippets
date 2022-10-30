<Query Kind="Program" />

private void ReadStream(MemoryStream s)
{
	string x = "";
	s.Position = 0;
	using StreamReader sr = new StreamReader(s);
	while (!sr.EndOfStream)
	{
		x += sr.ReadLine();

	}
}