internal sealed class Part1
{
	public class Shape
	{
		public bool[,] Occupied;
		public readonly int NumOccupied;

		public Shape(bool[,] occupied)
		{
			Occupied = occupied;
			NumOccupied = 0;
			foreach (var tileOccupied in Occupied)
				if (tileOccupied)
					NumOccupied++;
		}
	}

	public class Puzzle
	{
		public readonly int W;
		public readonly int H;
		public readonly int[] Counts;
		public bool? Solvable = null;
		public int NumFreeSpaces;

		public Puzzle(int w, int h, int[] counts)
		{
			W = w;
			H = h;
			Counts = counts;
		}
	}

	private const int _numShapes = 6;
	private Shape[] _shapes = new Shape[_numShapes];
	private List<Puzzle> _puzzles = [];
	private int _numSolvable = 0;

	public void Run()
	{
		ParseInput();
		PruneTooSmallPuzzles();

		// Not even necessary.
		MarkPuzzlesThatCanBeSolvedWithNoOverlaps();
		
		// The input to this puzzle is a special case where all puzzles are either:
		// 1) Trivially known to not work because the sum of the occupied spaces of the presents
		//    even if you compress them perfectly is too great for the number of board spaces.
		// 2) Have a very large amount of remaining space. So large that even with just a small
		//    amount of clever arranging to fill gaps a solution is easily available.
		Console.WriteLine(_numSolvable + _puzzles.Count);
	}

	private void PruneTooSmallPuzzles()
	{
		var prunedPuzzles = new List<Puzzle>();
		for (var i = 0; i < _puzzles.Count; i++)
		{
			var puzzle = _puzzles[i];
			var a = puzzle.W * puzzle.H;

			var totalAreaOfShapes = 0;
			for (var s = 0; s < _numShapes; s++)
			{
				totalAreaOfShapes += _shapes[s].NumOccupied * puzzle.Counts[s];
			}

			puzzle.NumFreeSpaces = a - totalAreaOfShapes;

			if (puzzle.NumFreeSpaces > 0)
			{
				prunedPuzzles.Add(puzzle);
			}
		}
		Console.WriteLine($"Pruned {prunedPuzzles.Count}/{_puzzles.Count} puzzles.");
		_puzzles = prunedPuzzles.OrderBy(p => p.NumFreeSpaces).ToList();
	}

	private void MarkPuzzlesThatCanBeSolvedWithNoOverlaps()
	{
		var prunedPuzzles = new List<Puzzle>();
		var originalNumSolvable = _numSolvable;
		foreach (var puzzle in _puzzles)
		{
			var numPossibleWithNoOverlap = (puzzle.W / 3) * (puzzle.H / 3);
			var totalShapes = 0;
			foreach (var c in puzzle.Counts)
				totalShapes += c;
			if (numPossibleWithNoOverlap <= totalShapes)
				_numSolvable++;
			else
				prunedPuzzles.Add(puzzle);
		}

		Console.WriteLine($"{_numSolvable - originalNumSolvable}/{_puzzles.Count} puzzles are solvable with no overlapping needed. {prunedPuzzles.Count} remaining.");
		_puzzles = prunedPuzzles.OrderBy(p => p.NumFreeSpaces).ToList();
	}

	#region Input

	private void ParseInput()
	{
		AddShapes();

		var sr = new StreamReader("input.txt");
		var line = sr.ReadLine();
		var shapesIndex = -1;
		while (line != null)
		{
			if (!line.Contains("x"))
			{
				line = sr.ReadLine();
				continue;
			}

			var parts = line.Split(":");
			var dimensionParts = parts[0].Split("x");
			var w = int.Parse(dimensionParts[0]);
			var h = int.Parse(dimensionParts[1]);

			var countParts = parts[1].Trim().Split(" ");
			var counts = new int[_numShapes];
			for (var i = 0; i < _numShapes; i++)
				counts[i] = int.Parse(countParts[i]);
			_puzzles.Add(new Puzzle(w, h, counts));

			line = sr.ReadLine();
		}
	}

	private void AddShapes()
	{
		_shapes[0] = new Shape(new [,]
		{
			{ true, true, false },
			{ false, true, true },
			{ false, false, true },
		});
		_shapes[1] = new Shape(new[,]
		{
			{ true, false, true },
			{ true, true, true },
			{ true, false, true },
		});
		_shapes[2] = new Shape(new[,]
		{
			{ true, false, true },
			{ true, true, true },
			{ false, true, true },
		});
		_shapes[3] = new Shape(new[,]
		{
			{ true, false, true},
			{ true, false, true },
			{ true, true, true },
		});
		_shapes[4] = new Shape(new[,]
		{
			{ true, true, true },
			{ false, true, true },
			{ false, false, true },
		});
		_shapes[5] = new Shape(new[,]
		{
			{ false, false, true },
			{ true, true, true },
			{ true, true, true },
		});
	}

	#endregion Input
}
