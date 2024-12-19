namespace Day19;

class Program
{
    public required string[] _availablePatterns;
    public required string[] _requestedCombinations;
    private readonly Dictionary<string, long> _cache = new();

    private static void ParseInput(string input, out string[] availablePatterns, out string[] requestedCombinations)
    {
        string[] lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        int line = 0;

        List<string> available = new();

        while (lines[line].Contains(','))
        {
            available.AddRange(lines[line].Split(", ", StringSplitOptions.RemoveEmptyEntries));
            line++;
        }

        List<string> requested = new();

        for (int l = line; l < lines.Length; l++)
        {
            requested.Add(lines[l]);
        }

        availablePatterns = available.ToArray();
        requestedCombinations = requested.ToArray();
    }


    private long GetCombinations(string goal)
    {
        if (string.IsNullOrWhiteSpace(goal))
        {
            return 1;
        }

        if (_cache.TryGetValue(goal, out long combinations))
        {
            return combinations;
        }

        combinations = _availablePatterns
            .Where(goal.StartsWith)
            .Select(pattern => GetCombinations(goal[pattern.Length..]))
            .Sum();

        _cache.TryAdd(goal, combinations);

        return combinations;
    }

    static void Main(string[] args)
    {
        string input = """
                       r, wr, b, g, bwu, rb, gb, br

                       brwrr
                       bggr
                       gbbr
                       rrbgbr
                       ubwu
                       bwurrg
                       brgr
                       bbrgwb
                       """;

        input = File.ReadAllText("Day19.txt");
        ParseInput(input, out string[] availablePatterns, out string[] requestedCombinations);

        Program program = new()
        {
            _availablePatterns = availablePatterns,
            _requestedCombinations = requestedCombinations
        };

        int possible = 0;
        long total = 0;

        foreach (string combination in requestedCombinations)
        {
            long combinations = program.GetCombinations(combination);
            if (combinations > 0) possible += 1;
            total += combinations;
        }

        Console.WriteLine($"{possible} combinations are possible.");
        Console.WriteLine($"Total number of combinations: {total}");
    }
}