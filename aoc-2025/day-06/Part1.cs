internal sealed class Part1
{
	public void Run()
	{
		var sr = new StreamReader("input.txt");
		var line = sr.ReadLine();

		var equations = new List<List<long>>();
		var sum = 0L;
		while (line != null)
		{
			var parts = line.Split(' ');
			var i = 0;
			foreach (var part in parts)
			{
				var trimmed = part.Trim();
				if (trimmed.Length == 0)
					continue;

				if (trimmed[0] == '*')
				{
					var res = 1L;
					foreach (var v in equations[i])
						res *= v;
					sum += res;
				}
				else if (trimmed[0] == '+')
				{
					var res = 0L;
					foreach (var v in equations[i])
						res += v;
					sum += res;
				}
				else
				{
					var val = long.Parse(trimmed);
					if (i >= equations.Count)
						equations.Add([]);
					equations[i].Add(val);
				}
				i++;
			}

			line = sr.ReadLine();
		}

		Console.WriteLine(sum); // 5524274308182
	}
}