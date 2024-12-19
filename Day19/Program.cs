namespace Day19;

class Program
{
    public required string[] _availablePatterns;
    public required string[] _requestedCombinations;
    private readonly Dictionary<string, int> _cache = new();

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


    private int GetCombinations(string goal)
    {
        if (_cache.TryGetValue(goal, out int combinations))
        {
            return combinations;
        }

        combinations = 0;

        if (string.IsNullOrWhiteSpace(goal))
        {
            return 1;
        }

        foreach (string pattern in _availablePatterns)
        {
            if (!goal.StartsWith(pattern)) continue;

            string newGoal = goal[pattern.Length..];   
            combinations += GetCombinations(newGoal);
            if (combinations > 0) break;
        }

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

        foreach (string combination in requestedCombinations)
        {
            int combinations = program.GetCombinations(combination);
            if (combinations > 0) possible += 1;
        }

        Console.WriteLine($"{possible} combinations are possible.");
    }
}