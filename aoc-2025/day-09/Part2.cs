internal sealed class Part2
{
	private class Point
	{
		public readonly long X;
		public readonly long Y;

		public Point(long x, long y)
		{
			X = x;
			Y = y;
		}
	};

	private class Line
	{
		public readonly Point A;
		public readonly Point B;

		public Line(Point a, Point b)
		{
			A = a;
			B = b;
		}
	};

	private readonly List<Point> _points = [];
	private readonly List<Line> _lines = [];

	public void Run()
	{
		var sr = new StreamReader("input.txt");

		var line = sr.ReadLine();
		while (!string.IsNullOrEmpty(line))
		{
			var parts = line.Split(",");
			_points.Add(new Point(long.Parse(parts[0]), long.Parse(parts[1])));
			if (_points.Count > 1)
				_lines.Add(new Line(_points[^2], _points[^1]));
			line = sr.ReadLine();
		}

		_lines.Add(new Line(_points[^1], _points[0]));

		var numPoints = _points.Count;
		var largestArea = 0L;
		for (var i = 0; i < numPoints; i++)
		{
			for (var j = i + 1; j < numPoints; j++)
			{
				var area = GetArea(_points[i], _points[j]);
				if (area < largestArea)
					continue;

				// Check for corners being inside.
				// We know the two points in question are already inside so we only need to check
				// the other two points.
				var p3 = new Point(_points[i].X, _points[j].Y);
				if (!WithinArea(p3))
					continue;
				var p4 = new Point(_points[j].X, _points[i].Y);
				if (!WithinArea(p4))
					continue;

				// All four corners are inside. Check the edges for intersections.
				if (IntersectsEdge(new Line(_points[i], p3)))
					continue;
				if (IntersectsEdge(new Line(p3, _points[j])))
					continue;
				if (IntersectsEdge(new Line(_points[j], p4)))
					continue;
				if (IntersectsEdge(new Line(p4, _points[i])))
					continue;

				// This rectangle is within the enclosed area.
				largestArea = area;
			}
		}

		Console.WriteLine(largestArea);
	}

	private long GetArea(Point p1, Point p2)
	{
		var x = Math.Abs(p1.X - p2.X) + 1;
		var y = Math.Abs(p1.Y - p2.Y) + 1;
		return x * y;
	}

	private bool WithinArea(Point p)
	{
		// If point falls on a line then it is within the area.
		foreach (var l in _lines)
			if (DoesPointFallOnLine(p, l))
				return true;

		// Examine all intersections to the right.
		var intersections = 0;
		for (var lineIndex = 0; lineIndex < _lines.Count; lineIndex++)
		{
			var l = _lines[lineIndex];

			// Line is horizontal.
			if (l.A.Y == l.B.Y)
			{
				var fullyToRight = (p.Y == l.A.Y && l.A.X > p.X && l.B.X > p.X);
				if (fullyToRight)
				{
					var previousLine = _lines[lineIndex - 1 < 0 ? _lines.Count - 1 : lineIndex - 1];
					var nextLine = _lines[lineIndex + 1 == _lines.Count ? 0 : lineIndex + 1];

					var previousLineGoesUp = previousLine.A.Y < l.A.Y || previousLine.B.Y < l.A.Y;
					var nextLineGoesUp = nextLine.A.Y < l.A.Y || nextLine.B.Y < l.A.Y;

					// If the line is fully to our right, and it is not a U turn, then it counts as a single
					// intersection. We will count both its end lines later in the loop. Subtract 1 to make it odd.
					if (nextLineGoesUp != previousLineGoesUp)
					{
						intersections--;
					}
				}
				continue;
			}

			// Line is to the left.
			if (l.A.X < p.X)
				continue;
			// Line is fully above point.
			if (l.A.Y < p.Y && l.B.Y < p.Y)
				continue;
			// Line is fully below point.
			if (l.A.Y > p.Y && l.B.Y > p.Y)
				continue;
			// Line intersects.
			intersections++;
		}
		return intersections % 2 == 1;
	}

	bool DoesPointFallOnLine(Point p, Line l)
	{
		if (l.A.X == l.B.X)
			return p.X == l.A.X && ((p.Y >= l.A.Y && p.Y <= l.B.Y) || (p.Y >= l.B.Y && p.Y <= l.A.Y));
		return p.Y == l.A.Y && ((p.X >= l.A.X && p.X <= l.B.X) || (p.X >= l.B.X && p.X <= l.A.X));
	}

	bool IntersectsEdge(Line line)
	{
		var horizontal = line.A.Y == line.B.Y;
		foreach (var l in _lines)
		{
			if (horizontal)
			{
				if (l.A.Y == l.B.Y)
					continue;
				var y = line.A.Y;
				var yIntersects = (l.A.Y < y && l.B.Y > y) || (l.B.Y < y && l.A.Y > y);
				if (!yIntersects)
					continue;
				var x = l.A.X;
				var xIntersects = (x > line.A.X && x < line.B.X) || (x > line.B.X && x < line.A.X);
				if (xIntersects)
					return true;
			}
			else
			{
				if (l.A.X == l.B.X)
					continue;

				var x = line.A.X;
				var xIntersects = (l.A.X < x && l.B.X > x) || (l.B.X < x && l.A.X > x);
				if (!xIntersects)
					continue;
				var y = l.A.Y;
				var yIntersects = (y > line.A.Y && y < line.B.Y) || (y > line.B.Y && y < line.A.Y);
				if (yIntersects)
					return true;
			}
		}
		return false;
	}
}
