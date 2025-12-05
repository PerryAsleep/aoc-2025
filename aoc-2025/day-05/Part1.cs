internal sealed class Part1
{
	private sealed class Range
	{
		public Range(long low, long high)
		{
			Low = low;
			High = high;
		}
		public long Low;
		public long High;
	}

	public void Run()
	{
		List<Range> ranges = [];
		List<long> values = [];

		var sr = new StreamReader("input.txt");
		var line = sr.ReadLine();
		while (line != null)
		{
			if (line.Length == 0)
			{
				line = sr.ReadLine();
				continue;
			}

			var parts = line.Split('-');
			if (parts.Length == 2)
				ranges.Add(new Range(long.Parse(parts[0]), long.Parse(parts[1])));
			else
				values.Add(long.Parse(line));
			line = sr.ReadLine();
		}

		var numInAnyRange = 0;
		foreach (var v in values)
		{
			foreach (var r in ranges)
			{
				if (v >= r.Low && v <= r.High)
				{
					numInAnyRange++;
					break;
				}
			}
		}

		Console.WriteLine(numInAnyRange);
	}
}