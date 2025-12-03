internal sealed class Part2
{
	public static void Run()
	{
		var sum = 0L;

		var sr = new StreamReader("input.txt");
		var line = sr.ReadLine();
		while (!string.IsNullOrEmpty(line))
		{
			sum += GetJoltage(line);
			line = sr.ReadLine();
		}

		Console.WriteLine(sum); // 172681562473501
	}

	private static long GetJoltage(string line)
	{
		var numDigits = 12;
		var digits = new int[numDigits];
		var insertIndexes = new int[10];

		var numValues = line.Length;
		for (var i = 0; i < numValues; i++)
		{
			var v = line[i] - '0';
			if (insertIndexes[v] >= numDigits)
				continue;
			var insertIndex = Math.Max(insertIndexes[v], numDigits - (numValues - i));
			digits[insertIndex] = v;
			insertIndex++;
			for (var j = 0; j <= v; j++)
				insertIndexes[j] = insertIndex;
		}

		var result = 0L;
		var mult = 1L;
		for (var i = numDigits - 1; i >= 0; i--)
		{
			result += digits[i] * mult;
			mult *= 10;
		}

		return result;
	}
}
