internal sealed class Part2
{
	private class Node : IEquatable<Node>
	{
		private readonly string _id;
		public readonly List<Node> Links = [];

		public Node(string id)
		{
			_id = id;
		}

		public bool Equals(Node? other)
		{
			if (other is null)
				return false;
			if (ReferenceEquals(this, other))
				return true;
			return _id == other._id;
		}

		public override bool Equals(object? obj)
		{
			if (obj is null)
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != GetType())
				return false;
			return Equals((Node)obj);
		}

		public override int GetHashCode()
		{
			return _id.GetHashCode();
		}
	}

	public void Run()
	{
		var sr = new StreamReader("input.txt");

		var nodes = new Dictionary<string, Node>();

		var line = sr.ReadLine();
		while (!string.IsNullOrEmpty(line))
		{
			var parts = line.Split(":");
			var id = parts[0].Trim();
			nodes.TryAdd(id, new Node(id));
			var node = nodes[id];
			parts = parts[1].Split(" ");
			foreach (var neighborId in parts)
			{
				nodes.TryAdd(neighborId, new Node(neighborId));
				var neighbor = nodes[neighborId];
				neighbor.Links.Add(node);
			}
			line = sr.ReadLine();
		}

		Node a, b;
		var mid = GetUniquePaths(nodes["dac"], nodes["fft"], new Dictionary<Node, long>());
		if (mid > 0)
		{
			a = nodes["dac"];
			b = nodes["fft"];
		}
		else
		{
			mid = GetUniquePaths(nodes["fft"], nodes["dac"], new Dictionary<Node, long>());
			a = nodes["fft"];
			b = nodes["dac"];
		}
		var end = GetUniquePaths(b, nodes["out"], new Dictionary<Node, long>());
		var start = GetUniquePaths(nodes["svr"], a, new Dictionary<Node, long>());

		var total = start * mid * end;
		Console.WriteLine(total);
	}

	private static long GetUniquePaths(Node n, Node end, Dictionary<Node, long> counts)
	{
		if (n.Equals(end))
			return 1;
		if (counts.TryGetValue(n, out var count))
			return count;
		var numPaths = 0L;
		foreach (var neighbor in n.Links)
			numPaths += GetUniquePaths(neighbor, end, counts);
		counts[n] = numPaths;
		return counts[n];
	}
}
