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

		var numValues = line.Length;
		for (var i = 0; i < numValues; i++)
		{
			var v = line[i] - '0';

			// digits is sorted descending, could do a log(N) search for the index to set v.
			// But numDigits is small and this is simple.

			for (var di = 0; di < numDigits; di++)
			{
				if (v > digits[di] && numDigits - di <= (numValues - i))
				{
					digits[di] = v;
					for (var ri = di + 1; ri < numDigits; ri++)
						digits[ri] = 0;
					break;
				}
			}
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
