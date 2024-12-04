using System.Text.RegularExpressions;

namespace AdventOfCode.Day03;

class Program
{
    static void Main(string[] args)
    {
        // 161 = (2*4 + 5*5 + 11*8 + 8*5).
        string input = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";
        // input = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";
        input = File.ReadAllText("Day03.txt");
        // input = input.Replace(System.Environment.NewLine, "");

        int result = GetMultiplication(input);
        Console.WriteLine($"The result is {result}"); // 174336360

        const string expression = @"don't\(\).*?do\(\)";
        Regex regex = new(expression, RegexOptions.Singleline);
        Console.WriteLine(regex.Matches(input).Count);
        input = regex.Replace(input, "");
        result = GetMultiplication(input);
        Console.WriteLine($"The result is {result}");
    }

    private static int GetMultiplication(string input)
    {
        MatchCollection matches = Regex.Matches(input, @"mul\([0-9]*,[0-9]*\)");

        int result = 0;

        foreach (Match match in matches)
        {
            string value = match.Value.Replace("mul", "");
            value = value.Replace("(", "");
            value = value.Replace(")", "");
            string[] values = value.Split(",");
            int a = int.Parse(values[0]);
            int b = int.Parse(values[1]);

            result += a * b;
        }

        return result;
    }
}