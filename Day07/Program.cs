namespace AdventOfCode.Day07;

public class Equation
{
    public required long Result { get; init; }
    public required long[] Terms { get; init; }
}

class Program
{
    private static Equation[] ParseInput(string input)
    {
        string[] lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        Equation[] equations = new Equation[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split(": ");
            long result = long.Parse(parts[0]);
            long[] terms = parts[1].Split(" ").Select(long.Parse).ToArray();

            equations[i] = new Equation
            {
                Result = result,
                Terms = terms
            };
        }

        return equations;
    }

    private static bool IsPossible(long result, long[] terms, long current = 0)
    {
        if (terms.Length == 1)
        {
            bool mul = current * terms[0] == result;
            bool add = current + terms[0] == result;
            return mul || add;
        }

        bool multiply = current != 0 && IsPossible(result, terms[1..], current * terms[0]);
        bool addition = IsPossible(result, terms[1..], current + terms[0]);

        return multiply || addition;
    }

    static void Main()
    {
        string input = """
                       190: 10 19
                       3267: 81 40 27
                       83: 17 5
                       156: 15 6
                       7290: 6 8 6 15
                       161011: 16 10 13
                       192: 17 8 14
                       21037: 9 7 18 13
                       292: 11 6 16 20
                       """;

        input = File.ReadAllText("Day07.txt");

        Equation[] equations = ParseInput(input);

        long sum = 0;
        foreach (Equation equation in equations)
        {
            if (IsPossible(equation.Result, equation.Terms))
            {
                sum += equation.Result;
            }
        }

        // long sum = equations.Where(e => IsPossible(e.Result, e.Terms)).Sum(e => e.Result);
        Console.WriteLine(sum);
    }
}