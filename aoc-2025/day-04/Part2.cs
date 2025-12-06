using AocUtils;

internal sealed class Part2
{
	private int _w;
	private int _h;
	private char[,] _grid;

	public void Run()
	{
		(_grid, _w, _h) = Utils.ReadGrid("input.txt");

		HashSet<(int, int)> pending = [];
		for (var x = 0; x < _w; x++)
			for (var y = 0; y < _h; y++)
				if (IsAccessibleRoll(x, y))
					pending.Add((x, y));

		var numRemoved = 0;
		while (pending.Count > 0)
		{
			var (x, y) = Utils.PopAny(pending);
			_grid[x, y] = ' ';
			numRemoved++;
			foreach (var pos in GetAdjacentPositions(x, y))
				if (IsAccessibleRoll(pos.x, pos.y))
					pending.Add((pos.x, pos.y));
		}

		Console.WriteLine(numRemoved);
	}

	private bool IsAccessibleRoll(int x, int y)
	{
		if (_grid[x, y] != '@')
			return false;
		var numAdjacent = 0;
		foreach (var c in GetAdjacent(x, y))
			if (c == '@')
				numAdjacent++;
		return numAdjacent < 4;
	}

	private IEnumerable<(int x, int y)> GetAdjacentPositions(int x, int y)
	{
		for (var ix = x - 1; ix < x + 2; ix++)
		{
			for (var iy = y - 1; iy < y + 2; iy++)
			{
				if (ix == x && iy == y || !IsInBounds(ix, iy))
					continue;
				yield return (ix, iy);
			}
		}
	}

	private IEnumerable<char> GetAdjacent(int x, int y)
	{
		foreach (var pos in GetAdjacentPositions(x, y))
			yield return _grid[pos.x, pos.y];
	}

	private bool IsInBounds(int x, int y)
	{
		return x >= 0 && x < _w && y >= 0 && y < _h;
	}
}
