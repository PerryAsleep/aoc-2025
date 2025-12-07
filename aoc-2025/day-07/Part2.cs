internal sealed class Part2
{
	public void Run()
	{
		var sr = new StreamReader("input.txt");
		var line = sr.ReadLine();
		var timelinesPerColumns = new Dictionary<int, long>();
		while (line != null)
		{
			for (var x = 0; x < line.Length; x++)
			{
				var c = line[x];
				switch (c)
				{
					case 'S':
					{
						timelinesPerColumns.Add(x, 1);
						break;
					}
					case '^':
					{
						if (timelinesPerColumns.ContainsKey(x))
						{
							var timelines = timelinesPerColumns[x];
							timelinesPerColumns.Remove(x);

							if (!timelinesPerColumns.ContainsKey(x - 1))
								timelinesPerColumns.Add(x - 1, timelines);
							else
								timelinesPerColumns[x - 1] += timelines;

							if (!timelinesPerColumns.ContainsKey(x + 1))
								timelinesPerColumns.Add(x + 1, timelines);
							else
								timelinesPerColumns[x + 1] += timelines;
						}
						break;
					}
				}
			}
			line = sr.ReadLine();
		}

		var sum = 0L;
		foreach (var kvp in timelinesPerColumns)
			sum += kvp.Value;

		Console.WriteLine(sum);
	}
}
