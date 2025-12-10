using System.Diagnostics;
using Microsoft.Z3;

internal sealed class Part2
{
	private class Puzzle
	{
		private readonly List<int> _goal;
		private readonly List<List<int>> _moves = [];

		public Puzzle(List<int> goal, List<List<int>> moves)
		{
			_goal = goal;

			// Translate moves from indexes in goal dimensions to be 0/1 in
			// the same dimensions as the goal, effectively a vector moving towards
			// the goal.
			foreach (var move in moves)
			{
				var vector = new List<int>(_goal.Count);
				foreach (var _ in _goal)
					vector.Add(0);
				foreach (var index in move)
					vector[index] = 1;
				_moves.Add(vector);
			}
		}

		public int FindShortestSolution()
		{
			using var ctx = new Context();
			using var opt = ctx.MkOptimize();

			// Variables representing counts of each move vector to minimize.
			var variables = new IntExpr[_moves.Count];
			for (var i = 0; i < _moves.Count; i++)
			{
				variables[i] = ctx.MkIntConst($"x_{i}");
				opt.Add(ctx.MkGe(variables[i], ctx.MkInt(0)));
			}

			// For each dimension in the goal, the sum of the variables for each move
			// in that dimension must equal the goal value.
			for (var dimension = 0; dimension < _goal.Count; dimension++)
			{
				var sum = (ArithExpr)ctx.MkInt(0);
				for (var moveIndex = 0; moveIndex < _moves.Count; moveIndex++)
					if (_moves[moveIndex][dimension] != 0)
						sum = ctx.MkAdd(sum, ctx.MkMul(variables[moveIndex], ctx.MkInt(_moves[moveIndex][dimension])));
				opt.Add(ctx.MkEq(sum, ctx.MkInt(_goal[dimension])));
			}

			// Minimize the number of moves used by minimizing the sum of the variables.
			ArithExpr movesUsed = ctx.MkInt(0);
			foreach (var v in variables)
				movesUsed = ctx.MkAdd(movesUsed, v);
			opt.MkMinimize(movesUsed);

			// Solve.
			Debug.Assert(opt.Check() == Status.SATISFIABLE);
			var model = opt.Model;
			return ((IntNum)model.Evaluate(movesUsed)).Int;
		}
	}


	public void Run()
	{
		var sr = new StreamReader("input.txt");

		var puzzles = new List<Puzzle>();

		var line = sr.ReadLine();
		while (!string.IsNullOrEmpty(line))
		{
			var parts = line.Split(" ");
			var i = 0;
			List<int> goal = [];
			List<List<int>> moves = [];
			foreach (var part in parts)
			{
				if (part.StartsWith("{"))
				{
					var goalParts = part.Substring(1, part.Length - 2).Split(",");
					foreach (var goalStr in goalParts)
						goal.Add(int.Parse(goalStr));
				}
				else if (part.StartsWith("("))
				{
					var moveIndexParts = part.Substring(1, part.Length - 2).Split(",");
					var moveButtons = new List<int>();
					foreach (var moveStr in moveIndexParts)
						moveButtons.Add(int.Parse(moveStr));
					moves.Add(moveButtons);
				}
			}
			puzzles.Add(new Puzzle(goal, moves));
			line = sr.ReadLine();
		}

		var sum = 0L;
		foreach (var puzzle in puzzles)
			sum += puzzle.FindShortestSolution();
		Console.WriteLine(sum);
	}
}