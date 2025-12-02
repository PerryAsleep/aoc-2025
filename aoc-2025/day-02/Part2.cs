internal sealed class Part2
{
	public static void Run()
	{
		var sr = new StreamReader("input.txt");

		var sum = 0L;

		var line = sr.ReadLine();
		var ranges = line.Split(',');
		foreach (var range in ranges)
		{
			var lowHigh = range.Split('-');
			var low = long.Parse(lowHigh[0]);
			var high = long.Parse(lowHigh[1]);
			for (var l = low; l <= high; l++)
				if (IsIdRepeating(l))
					sum += l;
		}

		Console.WriteLine(sum); // 34284458938
	}

	private static bool IsIdRepeating(long l)
	{
		var s = l.ToString();
		var maxLen = s.Length >> 1;
		for (var len = 1; len <= maxLen; len++)
		{
			if (s.Length % len != 0)
				continue;
			var subStr = s[..len];
			var repeatStr = "";
			var numRepeats = s.Length / len;
			for (var r = 0; r < numRepeats; r++)
				repeatStr += subStr;
			if (repeatStr == s)
				return true;
		}
		return false;
	}
}
