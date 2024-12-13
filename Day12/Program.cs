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

        garden.Nodes.Add(new Plant()
        {
            X = -1,
            Y = -1,
            IsOutside = true,
        });

        garden.Nodes.Add(new Plant()
        {
            X = garden.Width,
            Y = -1,
            IsOutside = true,
        });

        garden.Nodes.Add(new Plant()
        {
            X = -1,
            Y = garden.Height,
            IsOutside = true,
        });

        garden.Nodes.Add(new Plant()
        {
            X = garden.Width,
            Y = garden.Height,
            IsOutside = true,
        });

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

    public (int, int) GetConnected()
    {
        HashSet<Plant> visited = new();
        int first = 0;
        int second = 0;

        foreach (Plant plant in Nodes)
        {
            if (plant.IsOutside || visited.Contains(plant)) continue;
            (int, int) result = GetConnected(visited, plant);
            first += result.Item1;
            second += result.Item2;
        }

        return new ValueTuple<int, int>(first, second);
    }

    private ValueTuple<int, int> GetConnected(HashSet<Plant> visited, Plant plant)
    {
        if (visited.Contains(plant)) return (0, 0);

        Queue<Plant> queue = [];
        queue.Enqueue(plant);

        char type = plant.Type;
        int bordersFound = 0;
        int plantsFound = 0;

        HashSet<Plant?> corners = [];

        while (queue.TryDequeue(out Plant? currentPlant))
        {
            if (currentPlant is null) throw new Exception("Plant is null");

            plantsFound += 1;
            visited.Add(currentPlant);

            foreach (Plant neighbor in currentPlant.Neighbors)
            {
                if (neighbor.IsOutside || neighbor.Type != currentPlant.Type)
                {
                    bordersFound += 1;
                }

                if (!visited.Contains(neighbor) && neighbor.Type == currentPlant.Type && !queue.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                }
            }

            AddCorner(plant, 1, 1);
            AddCorner(plant, -1, 1);
            AddCorner(plant, 1, -1);
            AddCorner(plant, -1, -1);
        }

        Console.WriteLine($"Plant {type} has {corners.Count} sides!");

        return (bordersFound * plantsFound, plantsFound * corners.Count);

        void AddCorner(Plant p, int x, int y)
        {
            Plant side = GetPlant(p.X + x, p.Y);
            Plant top = GetPlant(p.X, p.Y + y);
            Plant corner = GetPlant(p.X + x, p.Y + y);

            bool sideGood = side.IsOutside || side.Type != type;
            bool topGood = top.IsOutside || top.Type != type;
            bool cornerGood = corner.IsOutside || corner.Type != type;

            if (sideGood && topGood && cornerGood)
            {
                if (corners.Add(corner))
                    Console.WriteLine($"Plant {type} has the corner {corner.X}, {corner.Y}");
            }
        }
    }

    private Plant GetPlant(int x, int y)
    {
        Plant? plant = Nodes.FirstOrDefault(n => n.X == x && n.Y == y);

        if (plant is null)
        {
            throw new Exception($"Plant at {x}, {y} not found!");
        }

        return plant;
    }
}

class Program
{
    static void Main(string[] args)
    {
        string input = """
                       AAAA
                       BBCD
                       BBCC
                       EEEC
                       """;

        // input = File.ReadAllText("Day12.txt");
        Garden garden = Garden.CreateFromInput(input);
        (int, int) results = garden.GetConnected();
        Console.WriteLine($"need {results.Item1} fences");
        Console.WriteLine($"need {results.Item2} cheap fences");
    }
}