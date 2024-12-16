using AdventOfCode.Day14;

namespace AdventOfCode.Day16;

public class Tile
{
    public Direction Position { get; set; }

    public Tile? North { get; set; }
    public Tile? South { get; set; }
    public Tile? East { get; set; }
    public Tile? West { get; set; }
}

public class Maze
{
    public Direction Start { get; set; }
    public Direction End { get; set; }

    private List<Tile> Tiles { get; } = [];

    public static Maze ParseFromInput(string input)
    {
        Maze maze = new();

        string[] lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        for (int y = 0; y < lines.Length; y++)
        {
            string line = lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                char c = line[x];
                Direction position = new()
                {
                    X = x,
                    Y = y
                };

                if (c == 'S')
                {
                    maze.Start = position;
                }
                else if (c == 'E')
                {
                    maze.End = position;
                }

                if (c is not ('.' or 'S' or 'E')) continue;

                Tile tile = maze.GetTile(position, true)!;

                // north
                if (GetNeighbour(lines, x, y - 1))
                {
                    Tile? north = maze.GetTile(new Direction(x, y - 1), true);
                    tile.North = north;
                }

                // south
                if (GetNeighbour(lines, x, y + 1))
                {
                    Tile? south = maze.GetTile(new Direction(x, y + 1), true);
                    tile.South = south;
                }

                // east
                if (GetNeighbour(lines, x + 1, y))
                {
                    Tile? east = maze.GetTile(new Direction(x + 1, y), true);
                    tile.East = east;
                }

                // west
                if (GetNeighbour(lines, x - 1, y))
                {
                    Tile? west = maze.GetTile(new Direction(x - 1, y), true);
                    tile.West = west;
                }
            }
        }

        return maze;

        bool GetNeighbour(string[] l, int dx, int dy)
        {
            char c = l[dy][dx];
            return c is '.' or 'S' or 'E';
        }
    }

    private Tile? GetTile(Direction position, bool createIfNotExists = false)
    {
        Tile? tile = Tiles.FirstOrDefault(t => position == t.Position);

        if (tile is not null || !createIfNotExists) return tile;

        tile = new Tile
        {
            Position = position
        };
        Tiles.Add(tile);

        return tile;
    }

    public int Djikstra()
    {
        Dictionary<Tile, Tile> previous = new();
        Dictionary<Tile, int> distances = Tiles.ToDictionary(tile => tile, _ => -1);

        Tile startTile = GetTile(Start)!;
        Tile dummyStart = new()
        {
            Position = Start - Direction.East,
            East = startTile
        };

        previous.Add(startTile, dummyStart);
        distances[startTile] = 0;

        PriorityQueue<Tile, int> queue = new();
        queue.Enqueue(startTile, 0);

        List<Tile> visited = [];

        while (queue.TryDequeue(out Tile? tile, out int dist))
        {
            visited.Add(tile);

            if (tile.North is not null && !visited.Contains(tile.North))
            {
                bool isStraight = previous[tile].North == tile;
                int alternative = dist + (isStraight ? 1 : 1001);

                if (distances[tile.North] == -1 || distances[tile.North] >= alternative)
                {
                    distances[tile.North] = alternative;
                    previous[tile.North] = tile;
                    queue.Enqueue(tile.North, alternative);
                }
            }

            if (tile.South is not null && !visited.Contains(tile.South))
            {
                bool isStraight = previous[tile].South == tile;
                int alternative = dist + (isStraight ? 1 : 1001);

                if (distances[tile.South] == -1 || distances[tile.South] >= alternative)
                {
                    distances[tile.South] = alternative;
                    previous[tile.South] = tile;
                    queue.Enqueue(tile.South, alternative);
                }
            }

            if (tile.East is not null && !visited.Contains(tile.East))
            {
                bool isStraight = previous[tile].East == tile;
                int alternative = dist + (isStraight ? 1 : 1001);

                if (distances[tile.East] == -1 || distances[tile.East] >= alternative)
                {
                    distances[tile.East] = alternative;
                    previous[tile.East] = tile;
                    queue.Enqueue(tile.East, alternative);
                }
            }

            if (tile.West is not null && !visited.Contains(tile.West))
            {
                bool isStraight = previous[tile].West == tile;
                int alternative = dist + (isStraight ? 1 : 1001);

                if (distances[tile.West] == -1 || distances[tile.West] >= alternative)
                {
                    distances[tile.West] = alternative;
                    previous[tile.West] = tile;
                    queue.Enqueue(tile.West, alternative);
                }
            }
        }

        return distances[GetTile(End)!];
    }
}

class Program
{
    static void Main(string[] args)
    {
        string input = """
                       ###############
                       #.......#....E#
                       #.#.###.#.###.#
                       #.....#.#...#.#
                       #.###.#####.#.#
                       #.#.#.......#.#
                       #.#.#####.###.#
                       #...........#.#
                       ###.#.#####.#.#
                       #...#.....#.#.#
                       #.#.#.###.#.#.#
                       #.....#...#.#.#
                       ##.##.#.#.#.#.#
                       #S..#.....#...#
                       ###############
                       """;

        input = File.ReadAllText("Day16.txt");
        Maze maze = Maze.ParseFromInput(input);
        int score = maze.Djikstra();
        Console.WriteLine(score);
    }
}