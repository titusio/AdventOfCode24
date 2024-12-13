namespace AdventOfCode.Day13;

public struct Direction
{
    public int X { get; set; }
    public int Y { get; set; }

    public Direction(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class Problem
{
    public Direction ButtonA { get; set; }
    public Direction ButtonB { get; set; }
    public Direction Prize { get; set; }
}

class Program
{
    private static List<Problem> ParseInput(string input)
    {
        List<Problem> problems = [];
        string[] lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < lines.Length; i += 3)
        {
            string[] buttonALine = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string[] buttonBLine = lines[i + 1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string[] prizeLine = lines[i + 2].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            int buttonAx = int.Parse(buttonALine[2].Split("+")[1].TrimEnd(','));
            int buttonAy = int.Parse(buttonALine[3].Split("+")[1]);

            int buttonBx = int.Parse(buttonBLine[2].Split("+")[1].TrimEnd(','));
            int buttonBy = int.Parse(buttonBLine[3].Split("+")[1]);

            int prizeX = int.Parse(prizeLine[1].Split("=")[1].TrimEnd(','));
            int prizeY = int.Parse(prizeLine[2].Split("=")[1]);

            Direction buttonA = new(buttonAx, buttonAy);
            Direction buttonB = new(buttonBx, buttonBy);
            Direction prize = new(prizeX, prizeY);

            problems.Add(new Problem()
            {
                ButtonA = buttonA,
                ButtonB = buttonB,
                Prize = prize
            });
        }

        return problems;
    }

    private static int SolveProblem1(Problem problem)
    {
        const int maxMoves = 100;

        int result = int.MaxValue;

        for (int b = 0; b < maxMoves; b++)
        {
            for (int a = 0; a < maxMoves; a++)
            {
                int x = problem.ButtonA.X * a + problem.ButtonB.X * b;
                int y = problem.ButtonA.Y * a + problem.ButtonB.Y * b;

                if (x == problem.Prize.X && y == problem.Prize.Y)
                {
                    result = Math.Min(3 * a + b, result);
                }
            }
        }

        return result == int.MaxValue ? 0 : result;
    }

    static void Main(string[] args)
    {
        string input = """
                       Button A: X+94, Y+34
                       Button B: X+22, Y+67
                       Prize: X=8400, Y=5400

                       Button A: X+26, Y+66
                       Button B: X+67, Y+21
                       Prize: X=12748, Y=12176

                       Button A: X+17, Y+86
                       Button B: X+84, Y+37
                       Prize: X=7870, Y=6450

                       Button A: X+69, Y+23
                       Button B: X+27, Y+71
                       Prize: X=18641, Y=10279
                       """;
        input = File.ReadAllText("Day13.txt");

        List<Problem> problems = ParseInput(input);

        int result = problems.Sum(SolveProblem1);

        Console.WriteLine(result);
    }
}