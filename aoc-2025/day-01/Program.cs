
// Part 1
//var sr = new StreamReader("input.txt");

//var pos = 50;
//var numZeros = 0;

//var line = sr.ReadLine();
//while (!string.IsNullOrEmpty(line))
//{
//	var mult = line[0] == 'L' ? -1 : 1;
//	var rot = (int.Parse(line.Substring(1)) % 100) * mult;
//	pos += rot;
//	if (pos < 0)
//		pos += 100;
//	if (pos >= 100)
//		pos -= 100;
//	if (pos == 0)
//		numZeros++;
//	line = sr.ReadLine();
//}

//Console.WriteLine(numZeros);

// Part 2
var sr = new StreamReader("input.txt");

var pos = 50;
var prevPos = 50;
var numZeros = 0;

var line = sr.ReadLine();
while (!string.IsNullOrEmpty(line))
{
	var mult = line[0] == 'L' ? -1 : 1;
	var rot = int.Parse(line.Substring(1));
	var numPasses = rot / 100;
	rot = (rot % 100) * mult;
	pos += rot;
	if (pos < 0)
	{
		if (prevPos > 0)
			numZeros++;
		pos += 100;
	}
	else if (pos >= 100)
	{
		numZeros++;
		pos -= 100;
	}
	else if (pos == 0)
		numZeros++;

	prevPos = pos;
	numZeros += numPasses;
	line = sr.ReadLine();
}

Console.WriteLine(numZeros);