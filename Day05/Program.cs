namespace AdventOfCode.Day05;

public class Rule
{
    /// <summary>
    /// The value which should be printed first.
    /// </summary>
    public required int First { get; set; }

    /// <summary>
    /// The value which should be printed after the first value.
    /// </summary>
    public required int Second { get; set; }
}

class Program
{
    /// <summary>
    /// Parses the input and fills the rules and pages list.
    /// </summary>
    /// <param name="input"> The input string. </param>
    /// <param name="rules"> The list of rules. </param>
    /// <param name="pages"> The list of pages. </param>
    private static void ParseInput(string input, out List<Rule> rules, out List<int[]> pages)
    {
        string[] lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        rules = [];
        pages = [];

        rules.AddRange(lines.TakeWhile(t => !t.Contains(','))
            .Select(t => t.Split("|", StringSplitOptions.RemoveEmptyEntries))
            .Select(rule => rule.Select(int.Parse).ToArray())
            .Select(r => new Rule { First = r[0], Second = r[1] })
        );

        for (int i = rules.Count; i < lines.Length; i++)
        {
            string[] ticket = lines[i].Split(",", StringSplitOptions.RemoveEmptyEntries);
            pages.Add(ticket.Select(int.Parse).ToArray());
        }
    }

    /// <summary>
    /// Sorts the page according to the rules and checks if the page is sorted correctly.
    /// </summary>
    /// <param name="rules"> The rules for sorting. </param>
    /// <param name="page"> The page to sort. </param>
    /// <param name="correctOrdering"> The resulting correct ordering. </param>
    /// <returns> True if the page is sorted correctly, false otherwise. </returns>
    private static bool IsSorted(List<Rule> rules, int[] page, out int[] correctOrdering)
    {
        IEnumerable<Rule> relevantRules = rules
            .Where(r => page.Contains(r.First) && page.Contains(r.Second)
                        || page.Contains(r.Second) && page.Contains(r.First)
            ).ToArray();

        correctOrdering = new int[page.Length];

        foreach (int entry in page)
        {
            int count = relevantRules.Count(p => p.First == entry);
            correctOrdering[page.Length - count - 1] = entry;
        }

        return page.SequenceEqual(correctOrdering);
    }

    static void Main(string[] args)
    {
        string input = """
                       47|53
                       97|13
                       97|61
                       97|47
                       75|29
                       61|13
                       75|53
                       29|13
                       97|29
                       53|29
                       61|53
                       97|53
                       61|29
                       47|13
                       75|47
                       97|75
                       47|61
                       75|61
                       47|29
                       75|13
                       53|13

                       75,47,61,53,29
                       97,61,53,29,13
                       75,29,13
                       75,97,47,61,53
                       61,13,29
                       97,13,75,29,47
                       """;

        input = File.ReadAllText("Day05.txt");

        ParseInput(input, out List<Rule> rules, out List<int[]> pages);

        // get the sum of the middle values of the pages which are sorted correctly
        int firstResult = pages
            .Where(p => IsSorted(rules, p, out _))
            .Select(p => p[p.Length / 2])
            .Sum();

        // get the sum of the middle values of the pages which are not sorted correctly
        int secondResult = pages
            .Select(p => !IsSorted(rules, p, out int[] correct) ? correct[correct.Length / 2] : 0).Sum();

        Console.WriteLine($"The first result is: {firstResult}"); // 5747
        Console.WriteLine($"The second result is: {secondResult}"); // 5502
    }
}