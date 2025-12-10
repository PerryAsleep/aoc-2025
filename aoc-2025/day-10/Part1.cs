internal sealed class Part1
{
	private class Puzzle
	{
		private readonly string _goal;
		private readonly List<List<int>> _moves;

		public Puzzle(string goal, List<List<int>> moves)
		{
			_goal = goal;
			_moves = moves;
		}

		public int FindShortestSolution()
		{
			HashSet<string> visitedStates = [];
			HashSet<string> currentStates =
			[
				new ('.', _goal.Length),
			];

			var depth = 0;
			while (true)
			{
				depth++;
				HashSet<string> newStates = [];
				foreach (var s in currentStates)
				{
					foreach (var m in _moves)
					{
						var newState = ApplyMove(m, s);
						if (!visitedStates.Add(newState))
							continue;
						if (newState == _goal)
							return depth;
						newStates.Add(newState);
					}
				}
				currentStates = newStates;
			}
		}

		private string ApplyMove(List<int> move, string s)
		{
			var newStateChars = new char[s.Length];
			for (var i = 0; i < s.Length; i++)
				newStateChars[i] = s[i];
			foreach (var i in move)
				newStateChars[i] = s[i] == '#' ? '.' : '#';
			return new string(newStateChars);
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
			string goal = null;
			List<List<int>> moves = [];
			foreach (var part in parts)
			{
				if (part.StartsWith("["))
				{
					goal = part.Substring(1, part.Length - 2);
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