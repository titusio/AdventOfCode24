namespace AdventOfCode.Day14;

public struct Direction
{
    public static readonly Direction North = new(0, -1);
    public static readonly Direction South = new(0, 1);
    public static readonly Direction East = new(1, 0);
    public static readonly Direction West = new(-1, 0);

    public int X { get; set; }
    public int Y { get; set; }

    public Direction()
    {
        X = 0;
        Y = 0;
    }

    public Direction(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static bool operator ==(Direction a, Direction b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Direction a, Direction b)
    {
        return a.X != b.X || a.Y != b.Y;
    }

    public static Direction operator +(Direction a, Direction b)
    {
        return new Direction(a.X + b.X, a.Y + b.Y);
    }

    public static Direction operator -(Direction a, Direction b)
    {
        return new Direction(a.X - b.X, a.Y - b.Y);
    }

    public override string ToString()
    {
        return $"[{X}, {Y}]";
    }
}

public class Robot
{
    public Direction Position { get; set; }
    public Direction Direction { get; set; }

    public static List<Robot> ParseFromInput(string input)
    {
        List<Robot> robots = [];

        string[] lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            string[] parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string[] position = parts[0].Replace("p=", string.Empty).Split(",", StringSplitOptions.RemoveEmptyEntries);
            string[] direction = parts[1].Replace("v=", string.Empty).Split(",", StringSplitOptions.RemoveEmptyEntries);

            Robot robot = new()
            {
                Position = new Direction
                {
                    X = int.Parse(position[0]),
                    Y = int.Parse(position[1])
                },
                Direction = new Direction
                {
                    X = int.Parse(direction[0]),
                    Y = int.Parse(direction[1])
                }
            };

            robots.Add(robot);
        }

        return robots;
    }
}

public class Room
{
    public List<Robot> Robots { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public void Simulate(int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            foreach (Robot robot in Robots)
            {
                Direction position = robot.Position;
                Direction direction = robot.Direction;

                position.X += direction.X;
                position.Y += direction.Y;

                position.X %= Width;
                position.Y %= Height;

                if (position.X < 0)
                {
                    while (position.X < 0)
                        position.X += Width;
                }

                if (position.Y < 0)
                {
                    while (position.Y < 0)
                        position.Y += Height;
                }

                robot.Position = position;
            }
        }
    }

    public override string ToString()
    {
        string result = "";

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                int count = Robots.Count(r => r.Position.X == x && r.Position.Y == y);
                result += count > 0 ? count.ToString() : ".";
            }

            if (y != Height - 1) result += "\n";
        }

        return result;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Room room = new()
        {
            Width = 101,
            Height = 103,
        };

        // string input = """
        //                p=0,4 v=3,-3
        //                p=6,3 v=-1,-3
        //                p=10,3 v=-1,2
        //                p=2,0 v=2,-1
        //                p=0,0 v=1,3
        //                p=3,0 v=-2,-2
        //                p=7,6 v=-1,-3
        //                p=3,0 v=-1,-2
        //                p=9,3 v=2,3
        //                p=7,3 v=-1,2
        //                p=2,4 v=2,-3
        //                p=9,5 v=-3,-3
        //                """;

        string input = File.ReadAllText("Day14.txt");

        room.Robots = Robot.ParseFromInput(input);

        Console.WriteLine($"simulating 100 steps...");
        room.Simulate(100);

        int middleX = room.Width / 2;
        int middleY = room.Height / 2;

        int topLeft = room.Robots.Count(r => r.Position.X < middleX && r.Position.Y < middleY);
        int topRight = room.Robots.Count(r => r.Position.X > middleX && r.Position.Y < middleY);
        int bottomLeft = room.Robots.Count(r => r.Position.X < middleX && r.Position.Y > middleY);
        int bottomRight = room.Robots.Count(r => r.Position.X > middleX && r.Position.Y > middleY);

        int result = topLeft * topRight * bottomLeft * bottomRight;
        Console.WriteLine(result);

        room.Robots = Robot.ParseFromInput(input);

        int steps = 0;
        while (true)
        {
            room.Simulate(1);
            steps++;
            // these requirements are from other solutions lol
            if (room.Robots.GroupBy(r => r.Position.Y).Count(g => g.Count() >= 31) != 2) continue;
            if (room.Robots.GroupBy(r => r.Position.X).Count(g => g.Count() >= 25) < 2) continue;
            Console.WriteLine(room);
            Console.WriteLine("steps: " + steps);
            break;
        }
    }
}