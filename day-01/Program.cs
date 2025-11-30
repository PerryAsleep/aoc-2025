
var sr = new StreamReader("input.txt");
var line = sr.ReadLine();
while (!string.IsNullOrEmpty(line))
{
	//
	line = sr.ReadLine();
}

Console.WriteLine("Done");