internal sealed class Part1
{
	public void Run()
	{
		var sr = new StreamReader("input.txt");
		var line = sr.ReadLine();
		var columnsWithBeams = new HashSet<int>();
		var numSplits = 0;
		while (line != null)
		{
			for (var x = 0; x < line.Length; x++)
			{
				var c = line[x];
				switch (c)
				{
					case 'S':
					{
						columnsWithBeams.Add(x);
						break;
					}
					case '^':
					{
						if (columnsWithBeams.Contains(x))
						{
							numSplits++;
							columnsWithBeams.Remove(x);
							columnsWithBeams.Add(x - 1);
							columnsWithBeams.Add(x + 1);
						}
						break;
					}
				}
			}
			line = sr.ReadLine();
		}

		Console.WriteLine(numSplits);
	}
}