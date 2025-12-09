internal sealed class Part1
{
	public void Run()
	{
		var sr = new StreamReader("input.txt");

		List<(long, long)> points = [];
		var line = sr.ReadLine();
		while (!string.IsNullOrEmpty(line))
		{
			var parts = line.Split(",");
			points.Add((long.Parse(parts[0]), long.Parse(parts[1])));
			line = sr.ReadLine();
		}

		var numPoints = points.Count;
		var largestArea = 0L;
		for (var i = 0; i < numPoints; i++)
			for (var j = i + 1; j < numPoints; j++)
				largestArea = Math.Max(largestArea, GetArea(points[i], points[j]));
		Console.WriteLine(largestArea);
	}

	public long GetArea((long, long) p1, (long, long) p2)
	{
		var x = Math.Abs(p1.Item1 - p2.Item1) + 1;
		var y = Math.Abs(p1.Item2 - p2.Item2) + 1;
		return x * y;
	}
}