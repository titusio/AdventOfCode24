namespace AdventOfCode.Day06;

public enum Tile
{
    Empty,
    Occupied
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

class Program
{
    private static Tile[,] ParseInput(string input, out (int x, int y) startPosition, out Direction startDirection)
    {
        startPosition = (0, 0);
        startDirection = Direction.Up;

        string[] lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        Tile[,] grid = new Tile[lines[0].Length, lines.Length];

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                grid[x, y] = lines[y][x] switch
                {
                    '#' => Tile.Occupied,
                    _ => Tile.Empty
                };

                if (!"^v<>".Contains(lines[y][x])) continue;

                startPosition = (x, y);
                startDirection = lines[y][x] switch
                {
                    '^' => Direction.Up,
                    'v' => Direction.Down,
                    '<' => Direction.Left,
                    '>' => Direction.Right,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        return grid;
    }

    static void Main()
    {
        string input = """
                       ....#.....
                       .........#
                       ..........
                       ..#.......
                       .......#..
                       ..........
                       .#..^.....
                       ........#.
                       #.........
                       ......#...
                       """;

        input = File.ReadAllText("Day06.txt");
        Tile[,] grid = ParseInput(input, out (int x, int y) currentPosition, out Direction currentDirection);
        HashSet<(int x, int y)> visited = [];

        int width = grid.GetLength(0);
        int height = grid.GetLength(1);
        while (currentPosition.x > 0 && currentPosition.x < width && currentPosition.y > 0 &&
               currentPosition.y < height)
        {
            visited.Add(currentPosition);
            Console.WriteLine($"Current position: {currentPosition}");

            switch (currentDirection)
            {
                case Direction.Up:
                {
                    if (currentPosition.y == 0)
                    {
                        currentPosition.y--;
                        break;
                    }

                    if (grid[currentPosition.x, currentPosition.y - 1] == Tile.Empty)
                    {
                        currentPosition.y--;
                    }
                    else
                    {
                        currentDirection = Direction.Right;
                    }

                    break;
                }
                case Direction.Down:
                {
                    if (currentPosition.y == height - 1)
                    {
                        currentPosition.y++;
                        break;
                    }

                    if (grid[currentPosition.x, currentPosition.y + 1] == Tile.Empty)
                    {
                        currentPosition.y++;
                    }
                    else
                    {
                        currentDirection = Direction.Left;
                    }

                    break;
                }
                case Direction.Left:
                {
                    if (currentPosition.x == 0)
                    {
                        currentPosition.x--;
                        break;
                    }

                    if (grid[currentPosition.x - 1, currentPosition.y] == Tile.Empty)
                    {
                        currentPosition.x--;
                    }
                    else
                    {
                        currentDirection = Direction.Up;
                    }

                    break;
                }
                case Direction.Right:
                {
                    if (currentPosition.x == width - 1)
                    {
                        currentPosition.x++;
                        break;
                    }

                    if (grid[currentPosition.x + 1, currentPosition.y] == Tile.Empty)
                    {
                        currentPosition.x++;
                    }
                    else
                    {
                        currentDirection = Direction.Down;
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        Console.WriteLine($"Visited {visited.Count} tiles");
    }
}