internal sealed class Part1
{
	public static void Run()
	{
		var sr = new StreamReader("input.txt");

		var sum = 0L;

		var line = sr.ReadLine();
		var ranges = line.Split(',');
		foreach (var range in ranges)
		{
			Console.WriteLine(range);

			var lowHigh = range.Split('-');
			var lowStr = lowHigh[0];
			var highStr = lowHigh[1];

			// Convert the range values to numbers with even digits.
			long low, high;
			if (lowStr.Length % 2 != 0)
				low = (long)Math.Pow(10, lowStr.Length);
			else
				low = long.Parse(lowStr);

			if (highStr.Length % 2 == 0)
				high = long.Parse(highStr);
			else
				high = (long)Math.Pow(10, highStr.Length - 1) - 1;
			if (low > high)
			{
				continue;
			}

			Console.WriteLine($"  {low} {high}");

			// Get the first half of the range numbers.
			var repeatRangeLow = GetFirstHalfDigits(low);
			var repeatRangeHigh = GetFirstHalfDigits(high);
			if (repeatRangeLow > repeatRangeHigh)
				(repeatRangeLow, repeatRangeHigh) = (repeatRangeHigh, repeatRangeLow);

			Console.WriteLine($"  {repeatRangeLow} {repeatRangeHigh}");

			for (var i = repeatRangeLow; i <= repeatRangeHigh; i++)
			{
				var v = long.Parse(i.ToString() + i.ToString());
				if (v < low || v > high)
					continue;
				Console.WriteLine($"    {v}");
				sum += v;
			}
		}

		Console.WriteLine(sum); // 23701357374
	}

	private static long GetFirstHalfDigits(long i)
	{
		var s = i.ToString();
		return long.Parse(s[..(s.Length >> 1)]);
	}
}