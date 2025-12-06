using AocUtils;

internal sealed class Part2
{
	public void Run()
	{
		var (grid, w, h) = Utils.ReadGrid("input.txt");

		var totalSum = 0L;
		var x = 0;
		while (x < w)
		{
			var mult = grid[x, h - 1] == '*';
			var res = mult ? 1L : 0L;
			var blankCol = false;
			while (!blankCol)
			{
				if (x == w)
					break;

				blankCol = true;
				var valStr = "";
				for (var y = 0; y < h - 1; y++)
				{
					if (grid[x, y] != ' ')
					{
						blankCol = false;
						valStr += grid[x, y];
					}
				}

				if (!blankCol)
				{
					if (mult)
						res *= long.Parse(valStr);
					else
						res += long.Parse(valStr);
				}
				x++;
			}

			totalSum += res;
		}

		Console.WriteLine(totalSum); // 8843673199391
	}
}
