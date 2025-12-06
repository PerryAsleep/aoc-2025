namespace AocUtils;

public static class Utils
{
	public static (char[,] grid, int w, int h) ReadGrid(string file)
	{
		var w = 0;
		var h = 0;
		var sr = new StreamReader(file);
		var line = sr.ReadLine();
		while (!string.IsNullOrEmpty(line))
		{
			w = line.Length;
			h++;
			line = sr.ReadLine();
		}

		var grid = new char[w, h];
		sr.DiscardBufferedData();
		sr.BaseStream.Seek(0, SeekOrigin.Begin);
		var y = 0;
		line = sr.ReadLine();
		while (!string.IsNullOrEmpty(line))
		{
			var x = 0;
			foreach (var c in line)
			{
				grid[x, y] = c;
				x++;
			}
			y++;

			line = sr.ReadLine();
		}

		return (grid, w, h);
	}

	public static T PopAny<T>(HashSet<T> s)
	{
		var e = s.GetEnumerator();
		e.MoveNext();
		var r = e.Current;
		s.Remove(r);
		return r;
	}
}

