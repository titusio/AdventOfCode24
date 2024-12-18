using AdventOfCode.Utils;

namespace AdventOfCode.Day15;

public enum WarehouseTile
{
    Wall = '#',
    Empty = '.',
    Box = 'O',
    Robot = '@'
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

    public static Warehouse FromInput(string input)
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

        Warehouse warehouse = new()
        {
            Grid = grid,
            Width = width,
            Height = height,
            _robotPosition = robotPosition,
            _moves = new Queue<Move>(moves) 
        };
        return warehouse;
    }

    public void ApplyMoves()
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
        }
    }

    private bool TryMove(Direction position, Direction direction)
    {
        WarehouseTile tile = Grid[position.X, position.Y];

        // we can move into an empty space
        if (tile == WarehouseTile.Empty) return true;
        
        // we can't move anything into a wall
        if (tile == WarehouseTile.Wall) return false;
        
        Direction nextPosition = position + direction;
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
                if (Grid[x, y] == WarehouseTile.Box)
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
                       #..O..O.O#
                       #......O.#
                       #.OO..O.O#
                       #..O@..O.#
                       #O#..O...#
                       #O..O..O.#
                       #.OO.O.OO#
                       #....O...#
                       ##########
                       
                       <vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
                       vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
                       ><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
                       <<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
                       ^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
                       ^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
                       >^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
                       <><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
                       ^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
                       v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
                       """;
        input = File.ReadAllText("Day15.txt");
        Warehouse warehouse = Warehouse.FromInput(input);
        warehouse.ApplyMoves();
        Console.WriteLine(warehouse.CalculateBoxGps());
    }
}