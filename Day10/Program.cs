namespace AdventOfCode.Day06;

public class Node
{
    public int X { get; init; }
    public int Y { get; init; }
    public int Value { get; set; }
    public List<Node> Neighbors { get; } = [];
}

public class Graph
{
    public List<Node> StartingPoints { get; } = [];
    public List<Node> Nodes { get; } = [];
}

class Program
{
    public static Graph ParseInput(string input)
    {
        string[] lines = input.Split("\n");

        Graph graph = new();

        for (int y = 0; y < lines.Length; y++)
        {
            string line = lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == ' ' || line[x] == '.')
                {
                    continue;
                }

                Node node = graph.Nodes.FirstOrDefault(n => n.X == x && n.Y == y) ??
                            new Node
                            {
                                X = x,
                                Y = y,
                            };
                node.Value = int.Parse(line[x].ToString());

                if (!graph.Nodes.Contains(node))
                {
                    graph.Nodes.Add(node);
                }

                if (node.Value == 0 && !graph.StartingPoints.Contains(node)) graph.StartingPoints.Add(node);

                AddNeighbour(node, x - 1, y);
                AddNeighbour(node, x + 1, y);
                AddNeighbour(node, x, y - 1);
                AddNeighbour(node, x, y + 1);
            }
        }

        return graph;

        void AddNeighbour(Node node, int x, int y)
        {
            if (x < 0 || x >= lines[0].Length || y < 0 || y >= lines.Length) return;

            Node neighbour = graph.Nodes.FirstOrDefault(n => n.X == x && n.Y == y) ??
                             new Node
                             {
                                 X = x,
                                 Y = y,
                             };

            if (!graph.Nodes.Contains(neighbour)) graph.Nodes.Add(neighbour);
            if (!node.Neighbors.Contains(neighbour)) node.Neighbors.Add(neighbour);
        }
    }

    private static int FindPathCount(Graph graph, out int distinctPathCount)
    {
        int result = 0;
        distinctPathCount = 0;
        foreach (Node startingPoint in graph.StartingPoints)
        {
            HashSet<ValueTuple<int, int>> reached = [];
            Queue<Node> queue = new();
            queue.Enqueue(startingPoint);

            int distinctPaths = 1;

            while (queue.Count > 0)
            {
                Node node = queue.Dequeue();

                if (node.Value == 9)
                {
                    reached.Add((node.X, node.Y));
                    continue;
                }

                List<Node> nextNodes = node.Neighbors.Where(n => n.Value == node.Value + 1).ToList();

                if (nextNodes.Count == 0) distinctPaths -= 1;

                nextNodes.ForEach(n => queue.Enqueue(n));

                if (nextNodes.Count > 1) distinctPaths += nextNodes.Count - 1;
            }

            result += reached.Count;
            distinctPathCount += distinctPaths;
        }

        return result;
    }

    static void Main()
    {
        string input = """
                       89010123
                       78121874
                       87430965
                       96549874
                       45678903
                       32019012
                       01329801
                       10456732
                       """;
        input = File.ReadAllText("Day10.txt");

        Graph graph = ParseInput(input);
        int pathCount = FindPathCount(graph, out int distinctPathCount);
        Console.WriteLine($"Found {pathCount} paths");
        Console.WriteLine($"Found {distinctPathCount} distinct paths");
    }
}