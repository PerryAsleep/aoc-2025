internal sealed class Part1
{
	private class Node : IEquatable<Node>
	{
		private readonly string _id;
		public readonly List<Node> Links = [];
		public long Count;

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

		public override string ToString()
		{
			return _id;
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
				node.Links.Add(neighbor);
			}
			line = sr.ReadLine();
		}

		nodes["out"].Count = 1L;
		var count = GetUniquePathsFromNodeToOut(nodes["you"]);
		Console.WriteLine(count);
	}

	private static long GetUniquePathsFromNodeToOut(Node n)
	{
		if (n.Count > 0)
			return n.Count;
		var numPaths = 0L;
		foreach (var neighbor in n.Links)
			numPaths += GetUniquePathsFromNodeToOut(neighbor);
		n.Count = numPaths;
		return n.Count;
	}
}
