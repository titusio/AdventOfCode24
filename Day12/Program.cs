namespace AdventOfCode.Day12;

public class Plant
{
    public int X { get; init; }
    public int Y { get; init; }
    public char Type { get; set; }

    /// <summary>
    /// we need to know if the plant is outside the garden
    /// </summary>
    public bool IsOutside { get; set; }

    public List<Plant> Neighbors { get; } = [];
}

public class Garden
{
    public List<Plant> Nodes { get; } = [];

    public int Width { get; private set; }
    public int Height { get; private set; }

    public static Garden CreateFromInput(string input)
    {
        Garden garden = new();
        string[] lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        garden.Height = lines.Length;
        garden.Width = lines.Max(l => l.Length);

        for (int y = 0; y < garden.Height; y++)
        {
            for (int x = 0; x < garden.Height; x++)
            {
                Plant plant = garden.Nodes.FirstOrDefault(n => n.X == x && n.Y == y) ??
                              new Plant
                              {
                                  X = x,
                                  Y = y,
                              };

                if (!garden.Nodes.Contains(plant))
                    garden.Nodes.Add(plant);

                plant.Type = lines[y][x];

                garden.AddNeighbour(x - 1, y, plant);
                garden.AddNeighbour(x + 1, y, plant);
                garden.AddNeighbour(x, y - 1, plant);
                garden.AddNeighbour(x, y + 1, plant);
            }
        }

        IEnumerable<Plant> duplicates = garden.Nodes.GroupBy(s => s)
            .SelectMany(grp => grp.Skip(1));

        foreach (Plant duplicate in duplicates)
        {
            Console.WriteLine($"Duplicate found at {duplicate.X}, {duplicate.Y}");
        }

        return garden;
    }

    private void AddNeighbour(int x, int y, Plant plant)
    {
        Plant neighbour = Nodes.FirstOrDefault(n => n.X == x && n.Y == y)
                          ?? new Plant
                          {
                              X = x,
                              Y = y,
                          };
        neighbour.IsOutside = x < 0 || x >= Width || y < 0 || y >= Height;

        if (!plant.Neighbors.Contains(neighbour))
            plant.Neighbors.Add(neighbour);
        if (!Nodes.Contains(neighbour))
            Nodes.Add(neighbour);
    }

    public int GetConnected()
    {
        HashSet<Plant> visited = new();
        int total = 0;

        foreach (Plant plant in Nodes)
        {
            if (plant.IsOutside || visited.Contains(plant)) continue;
            total += GetConnected(visited, plant);
        }

        return total;
    }

    private int GetConnected(HashSet<Plant> visited, Plant plant)
    {
        if (visited.Contains(plant)) return 0;

        Queue<Plant> queue = [];
        queue.Enqueue(plant);

        char type = plant.Type;
        int bordersFound = 0;
        int plantsFound = 0;

        while (queue.TryDequeue(out Plant? currentPlant))
        {
            if (currentPlant is null) throw new Exception("Plant is null");

            plantsFound += 1;
            visited.Add(currentPlant);

            foreach (Plant neighbor in currentPlant.Neighbors)
            {
                if (neighbor.IsOutside || neighbor.Type != currentPlant.Type)
                    bordersFound += 1;

                if (!visited.Contains(neighbor) && neighbor.Type == currentPlant.Type && !queue.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                }
            }
        }

        Console.WriteLine(
            $"Found {plantsFound} plants of type {type} and {bordersFound} borders resulting in {plantsFound * bordersFound}");
        return bordersFound * plantsFound;
    }
}

class Program
{
    static void Main(string[] args)
    {
        string input = """
                       RRRRIICCFF
                       RRRRIICCCF
                       VVRRRCCFFF
                       VVRCCCJFFF
                       VVVVCJJCFE
                       VVIVCCJJEE
                       VVIIICJJEE
                       MIIIIIJJEE
                       MIIISIJEEE
                       MMMISSJEEE
                       """;

        input = File.ReadAllText("Day12.txt");
        Garden garden = Garden.CreateFromInput(input);
        Console.WriteLine($"Found {garden.GetConnected()} connected plants");
    }
}