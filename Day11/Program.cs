namespace AdventOfCode.Day11;

class Program
{
    private readonly Dictionary<ulong, bool> _isEvenLength = new();
    private readonly Dictionary<ulong, ValueTuple<ulong, ulong>> _splitNumbers = new();


    private static List<ulong> GetInput(string input)
    {
        return input.Split(' ').Select(ulong.Parse).ToList();
    }

    private void Blink(List<ulong> stones)
    {
        int i = 0;

        int stonesCount = stones.Count;
        while (i < stonesCount)
        {
            ulong currentStone = stones[i];
            if (currentStone == 0)
            {
                stones[i] = 1;
                i += 1;
                continue;
            }

            if (!_isEvenLength.TryGetValue(currentStone, out bool evenLength))
            {
                string str = currentStone.ToString();
                evenLength = str.Length % 2 == 0;

                _isEvenLength.Add(currentStone, evenLength);

                if (evenLength)
                {
                    string first = str[..(str.Length / 2)];
                    string second = str[(str.Length / 2)..];
                    _splitNumbers[currentStone] = (ulong.Parse(first), ulong.Parse(second));
                }
            }

            if (evenLength)
            {
                ValueTuple<ulong, ulong> splits = _splitNumbers[currentStone];
                stones[i] = splits.Item1;
                stones.Add(splits.Item2);
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
        List<ulong> stones = GetInput(input);

        Program program = new();

        for (int i = 0; i < 75; i++)
        {
            Console.WriteLine($"step {i}");
            program.Blink(stones);
        }

        Console.WriteLine($"we have {stones.Count} stones");
    }
}