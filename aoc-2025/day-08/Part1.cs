using AocUtils;

internal sealed class Part1
{
	private class Node
	{
		public int X;
		public int Y;
		public int Z;
		public List<Edge> Edges = [];
	}

	private class Edge
	{
		public Node NodeA;
		public Node NodeB;
		public double D;
	}

	public void Run()
	{
		var sr = new StreamReader("input.txt");
		var numEdgesToMake = 1000;

		// Parse input.
		var line = sr.ReadLine();
		var nodes = new List<Node>();
		var edges = new List<Edge>();
		while (!string.IsNullOrEmpty(line))
		{
			var parts = line.Split(',');
			nodes.Add(new Node()
			{
				X = int.Parse(parts[0]),
				Y = int.Parse(parts[1]),
				Z = int.Parse(parts[2]),
			});
			line = sr.ReadLine();
		}

		// Make edges.
		for (var i = 0; i < nodes.Count; i++)
		{
			for (var j = i + 1; j < nodes.Count; j++)
			{
				edges.Add(new Edge()
				{
					NodeA = nodes[i],
					NodeB = nodes[j],
					D = GetDistance(nodes[i], nodes[j]),
				});
			}
		}
		edges = edges.OrderBy(e => e.D).ToList();

		// Connect edges.
		var edgeIndex = 0;
		while (numEdgesToMake > 0)
		{
			var e = edges[edgeIndex];
			e.NodeA.Edges.Add(e);
			e.NodeB.Edges.Add(e);
			edgeIndex++;
			numEdgesToMake--;
		}

		var sizes = GetCircuitSizes(nodes);
		var result = 1L;
		for (var i = 0; i < 3; i++)
			result *= sizes[i];
		Console.WriteLine(result);
	}

	private List<int> GetCircuitSizes(List<Node> nodes)
	{
		var nodesToVisit = new HashSet<Node>();
		var visitedNodes = new HashSet<Node>();
		foreach (var node in nodes)
			nodesToVisit.Add(node);

		var circuitSizes = new List<int>();
		while (nodesToVisit.Count > 0)
		{
			var n = Utils.PopAny(nodesToVisit);
			var size = 0;
			GetCircuitSize(n, nodesToVisit, visitedNodes, ref size);
			circuitSizes.Add(size);
		}
		return circuitSizes.OrderByDescending(i => i).ToList();
	}

	private void GetCircuitSize(Node n, HashSet<Node> nodesToVisit, HashSet<Node> visitedNodes, ref int size)
	{
		if (visitedNodes.Contains(n))
			return;

		nodesToVisit.Remove(n);
		visitedNodes.Add(n);
		size++;
		foreach (var e in n.Edges)
		{
			var neighbor = e.NodeA == n ? e.NodeB : e.NodeA;
			if (visitedNodes.Contains(neighbor))
				continue;
			GetCircuitSize(neighbor, nodesToVisit, visitedNodes, ref size);
		}
	}

	private static double GetDistance(Node nodeA, Node nodeB)
	{
		var x = (long)nodeA.X - nodeB.X;
		var y = (long)nodeA.Y - nodeB.Y;
		var z = (long)nodeA.Z - nodeB.Z;
		return Math.Sqrt(x * x + y * y + z * z);
	}
}