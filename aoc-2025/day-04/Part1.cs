internal sealed class Part1
{
	private int _w;
	private int _h;
	private char[,] _grid;

	public void Run()
	{
		ReadGrid("input.txt");

		var numAccessible = 0;
		for (var x = 0; x < _w; x++)
		{
			for (var y = 0; y < _h; y++)
			{
				if (_grid[x, y] != '@')
					continue;

				var numAdjacent = 0;
				foreach (var c in GetAdjacent(x, y))
					if (c == '@')
						numAdjacent++;
				if (numAdjacent < 4)
					numAccessible++;
			}
		}

		Console.WriteLine(numAccessible);
	}

	private IEnumerable<char> GetAdjacent(int x, int y)
	{
		for (var ix = x - 1; ix < x + 2; ix++)
		{
			for (var iy = y - 1; iy < y + 2; iy++)
			{
				if (ix == x && iy == y || !IsInBounds(ix, iy))
					continue;
				yield return _grid[ix, iy];
			}
		}
	}

	private bool IsInBounds(int x, int y)
	{
		return x >= 0 && x < _w && y >= 0 && y < _h;
	}

	private void ReadGrid(string file)
	{
		var sr = new StreamReader(file);
		var line = sr.ReadLine();
		while (!string.IsNullOrEmpty(line))
		{
			_w = line.Length;
			_h++;
			line = sr.ReadLine();
		}

		_grid = new char[_w, _h];
		sr.DiscardBufferedData();
		sr.BaseStream.Seek(0, SeekOrigin.Begin);
		var y = 0;
		line = sr.ReadLine();
		while (!string.IsNullOrEmpty(line))
		{
			var x = 0;
			foreach (var c in line)
			{
				_grid[x, y] = c;
				x++;
			}
			y++;

			line = sr.ReadLine();
		}
	}
}