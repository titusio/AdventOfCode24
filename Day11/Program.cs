namespace AdventOfCode.Day11;

class Program
{
    private static List<long> GetInput(string input)
    {
        return input.Split(' ').Select(long.Parse).ToList();
    }

    private static void Blink(List<long> stones)
    {
        int i = 0;

        while (i < stones.Count)
        {
            if (stones[i] == 0)
            {
                stones[i] = 1;
                i += 1;
                continue;
            }

            string num = stones[i].ToString();
            if (num.Length % 2 == 0)
            {
                string first = num[..(num.Length / 2)];
                string second = num[(num.Length / 2)..];
                stones[i] = long.Parse(first);
                stones.Insert(i + 1, long.Parse(second));
                i += 2;
                continue;
            }

            stones[i] *= 2024;
            i += 1;
        }
    }

    static void Main(string[] args)
    {
        string input = "125 17";
        input = File.ReadAllText("Day11.txt");
        List<long> stones = GetInput(input);

        for (int i = 0; i < 25; i++)
        {
            Console.WriteLine($"step {i}");
            Blink(stones);
        }

        Console.WriteLine($"we have {stones.Count} stones");
    }
}