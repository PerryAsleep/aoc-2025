using AocUtils;

internal sealed class Part2
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
		while (true)
		{
			var e = edges[edgeIndex];
			e.NodeA.Edges.Add(e);
			e.NodeB.Edges.Add(e);
			edgeIndex++;

			if (edgeIndex > nodes.Count)
			{
				if (IsOneCircuit(nodes))
				{
					var result = (long)e.NodeA.X * e.NodeB.X;
					Console.WriteLine(result);
					return;
				}
			}
		}
	}

	private bool IsOneCircuit(List<Node> nodes)
	{
		var nodesToVisit = new HashSet<Node>();
		var visitedNodes = new HashSet<Node>();
		foreach (var node in nodes)
			nodesToVisit.Add(node);

		var n = Utils.PopAny(nodesToVisit);
		var size = 0;
		GetCircuitSize(n, nodesToVisit, visitedNodes, ref size);
		if (nodesToVisit.Count == 0 && visitedNodes.Count == nodes.Count)
			return true;
		return false;
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