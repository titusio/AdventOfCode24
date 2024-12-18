using AdventOfCode.Utils;

namespace AdventOfCode.Day15;

public enum WarehouseTile
{
    Wall = '#',
    Empty = '.',
    Box = 'O',
    Robot = '@',
    PackageLeft = '[',
    PackageRight = ']'
}

public enum Move
{
    Up = '^',
    Down = 'v',
    Left = '<',
    Right = '>'
}

public class Warehouse
{
    public required WarehouseTile[,] Grid { get; init; }
    public int Width { get; private init; }
    public int Height { get; private init; }

    private Direction _robotPosition;

    private Queue<Move> _moves = [];

    public static Warehouse FromInput(string input, bool extendPackages = false)
    {
        string[] lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        List<Move> moves = [];

        // read moves
        // start reading from the bottom so we can see when the 
        // warehouse map starts
        int y = lines.Length - 1;
        for (; y >= 0; y--)
        {
            // check if end of moves is reached
            if (lines[y][0] == '#') break;

            for (int i = lines[y].Length - 1; i >= 0; i--)
            {
                char moveChar = lines[y][i];
                Move move = moveChar switch
                {
                    '^' => Move.Up,
                    'v' => Move.Down,
                    '<' => Move.Left,
                    '>' => Move.Right,
                    _ => throw new Exception($"Unknown move '{moveChar}' at ({i}, {y})")
                };
                // insert at the beginning because we go through the moves
                // in reverse order
                moves.Insert(0, move);
            }
        }

        int width = lines[y].Length;
        int height = y + 1;
        WarehouseTile[,] grid = new WarehouseTile[width, height];
        Direction robotPosition = default;

        for (; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y] = lines[y][x] switch
                {
                    '#' => WarehouseTile.Wall,
                    '.' => WarehouseTile.Empty,
                    'O' => WarehouseTile.Box,
                    '@' => WarehouseTile.Robot,
                    _ => throw new Exception($"Unknown tile '{lines[y][x]}' at ({x}, {y})")
                };

                if (grid[x, y] == WarehouseTile.Robot)
                {
                    robotPosition = new Direction(x, y);
                }
            }
        }

        if (!extendPackages)
        {
            return new Warehouse()
            {
                Grid = grid,
                Width = width,
                Height = height,
                _robotPosition = robotPosition,
                _moves = new Queue<Move>(moves)
            };
        }

        WarehouseTile[,] extendedGrid = new WarehouseTile[width * 2, height];
        for (y = 0; y < height; y++)
        {
            int currentX = 0;

            for (int x = 0; x < width; x++)
            {
                WarehouseTile tile = grid[x, y];
                if (tile is WarehouseTile.Wall or WarehouseTile.Empty)
                {
                    extendedGrid[currentX, y] = tile;
                    extendedGrid[currentX + 1, y] = tile;
                    currentX += 2;
                    continue;
                }

                if (tile == WarehouseTile.Robot)
                {
                    extendedGrid[currentX, y] = tile;
                    extendedGrid[currentX + 1, y] = WarehouseTile.Empty;
                    robotPosition = new Direction(currentX, y);
                    currentX += 2;
                    continue;
                }

                if (tile == WarehouseTile.Box)
                {
                    extendedGrid[currentX, y] = WarehouseTile.PackageLeft;
                    extendedGrid[currentX + 1, y] = WarehouseTile.PackageRight;
                    currentX += 2;
                    continue;
                }
            }
        }

        return new Warehouse()
        {
            Grid = extendedGrid,
            Width = width * 2,
            Height = height,
            _robotPosition = robotPosition,
            _moves = new Queue<Move>(moves)
        };
    }

    public void ApplyMoves(bool debug = false)
    {
        while (_moves.TryDequeue(out Move nextMove))
        {
            Direction position = _robotPosition;
            Direction direction = nextMove switch
            {
                Move.Up => new Direction(0, -1),
                Move.Down => new Direction(0, 1),
                Move.Left => new Direction(-1, 0),
                Move.Right => new Direction(1, 0),
                _ => throw new Exception($"Unknown move '{nextMove}'")
            };

            TryMove(position, direction);
            
            if (debug)
            {
                Print();
                Console.WriteLine("Moved: " + nextMove);
                Console.WriteLine();
            }
        }
    }

    private bool TryMove(Direction position, Direction direction)
    {
        WarehouseTile tile = Grid[position.X, position.Y];
        Direction nextPosition = position + direction;

        // we can move into an empty space
        if (tile == WarehouseTile.Empty) return true;

        // we can't move anything into a wall
        if (tile == WarehouseTile.Wall) return false;

        if (tile is WarehouseTile.Robot or WarehouseTile.Box)
        {
            bool moved = TryMove(nextPosition, direction);
            
            if (!moved) return false;
            
            Grid[position.X, position.Y] = WarehouseTile.Empty;
            Grid[nextPosition.X, nextPosition.Y] = tile;

            if (tile == WarehouseTile.Robot)
            {
                _robotPosition = nextPosition;
            }

            return true;
        }

        if (tile == WarehouseTile.PackageLeft)
        {
            if (direction.X == 0)
            {
                bool leftMoved = TryMove(nextPosition, direction);
                bool rightMoved = TryMove(nextPosition + new Direction(1, 0), direction);
                bool moved = leftMoved && rightMoved;

                if (!moved) return false;
                
                Grid[position.X, position.Y] = WarehouseTile.Empty;
                Grid[position.X + 1, position.Y] = WarehouseTile.Empty;
                Grid[nextPosition.X, nextPosition.Y] = WarehouseTile.PackageLeft;
                Grid[nextPosition.X + 1, nextPosition.Y] = WarehouseTile.PackageRight;
            }
            else
            {
                bool moved = TryMove(position + direction * 2, direction);
                if (!moved) return false;
                
                Grid[position.X, position.Y] = WarehouseTile.Empty;
                // move the package to the right
                if (direction.X > 0)
                {
                    Grid[position.X + 1, position.Y] = WarehouseTile.PackageLeft;
                    Grid[position.X + 2, position.Y] = WarehouseTile.PackageRight;
                }
                // move the package to the left
                else
                {
                    Grid[position.X - 1, position.Y] = WarehouseTile.PackageRight;
                    Grid[position.X - 2, position.Y] = WarehouseTile.PackageLeft;
                }
            }
            
            return true;
        }

        if (tile == WarehouseTile.PackageRight)
        {
            if (direction.X == 0)
            {
                bool leftMoved = TryMove(nextPosition + new Direction(-1, 0), direction);
                bool rightMoved = TryMove(nextPosition, direction);
                bool moved = leftMoved && rightMoved;

                if (!moved) return false;
                
                Grid[position.X, position.Y] = WarehouseTile.Empty;
                Grid[position.X - 1, position.Y] = WarehouseTile.Empty;
                Grid[nextPosition.X, nextPosition.Y] = WarehouseTile.PackageRight;
                Grid[nextPosition.X - 1, nextPosition.Y] = WarehouseTile.PackageLeft;
            }
            else
            {
                bool moved = TryMove(position + direction * 2, direction);
                if (!moved) return false;
                
                Grid[position.X, position.Y] = WarehouseTile.Empty;
                // move the package to the right
                if (direction.X > 0)
                {
                    Grid[position.X + 1, position.Y] = WarehouseTile.PackageLeft;
                    Grid[position.X + 2, position.Y] = WarehouseTile.PackageRight;
                }
                else
                {
                    Grid[position.X - 1, position.Y] = WarehouseTile.PackageRight;
                    Grid[position.X - 2, position.Y] = WarehouseTile.PackageLeft;
                }
            }

            return true;
        }

        throw new Exception();
    }

    public void Print()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Console.Write(Grid[x, y] switch
                {
                    WarehouseTile.Wall => '#',
                    WarehouseTile.Empty => '.',
                    WarehouseTile.Box => 'O',
                    WarehouseTile.Robot => '@',
                    WarehouseTile.PackageLeft => '[',
                    WarehouseTile.PackageRight => ']',
                    _ => throw new Exception($"Unknown tile '{Grid[x, y]}' at ({x}, {y})")
                });
            }

            Console.WriteLine();
        }
    }

    public int CalculateBoxGps()
    {
        int result = 0;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Grid[x, y] is WarehouseTile.Box)
                {
                    result += x + 100 * y;
                }
                else if (Grid[x, y] is WarehouseTile.PackageLeft)
                {
                    result += x + 100 * y;
                }
            }
        }

        return result;
    }
}

class Program
{
    public static void Main()
    {
        string input = """
                       ##########
                       #....#.O.#
                       #.O.O..#.#
                       #...OOO.O#
                       #.#.#....#
                       ##O..@.#.#
                       #..OOO.OO#
                       #..OOOOO.#
                       #.....#O.#
                       ##########
                       
                       <^<<>v^><<><>><v>>^^^>>v^vv><>>vv^><v>^>
                       """;
        // input = File.ReadAllText("Day15.txt");
        Warehouse warehouse = Warehouse.FromInput(input);
        // warehouse.ApplyMoves();
        warehouse.Print();
        Console.WriteLine($"Gps of the boxes: {warehouse.CalculateBoxGps()}");

        Warehouse bigWarehouse = Warehouse.FromInput(input, true);
        bigWarehouse.ApplyMoves(true);
        bigWarehouse.Print();
        Console.WriteLine($"Gps of the boxes: {bigWarehouse.CalculateBoxGps()}");
    }
}