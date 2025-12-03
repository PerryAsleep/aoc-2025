internal sealed class Part1
{
	public static void Run()
	{
		var sum = 0;

		var sr = new StreamReader("input.txt");
		var line = sr.ReadLine();
		while (!string.IsNullOrEmpty(line))
		{
			sum += GetJoltage(line);
			line = sr.ReadLine();
		}

		Console.WriteLine(sum);
	}

	private static int GetJoltage(string line)
	{
		var greatestDigitFound = 0;
		var greatestSecondDigitFound = 0;
		var numValues = line.Length;
		for (var i = 0; i < numValues; i++)
		{
			var v = (int)(line[i] - '0');
			if (i < numValues - 1)
			{
				if (v > greatestDigitFound)
				{
					greatestDigitFound = v;
					greatestSecondDigitFound = 0;
				}
				else if (v > greatestSecondDigitFound)
					greatestSecondDigitFound = v;
			}
			else if (v > greatestSecondDigitFound)
				greatestSecondDigitFound = v;
		}

		return greatestDigitFound * 10 + greatestSecondDigitFound;
	}
}