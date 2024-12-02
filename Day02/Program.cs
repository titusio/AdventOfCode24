namespace AdventOfCode.Day02;

public class Program
{
    /// <summary>
    /// Parses the input into a sequence of reports.
    /// A report is an array of integers.
    /// </summary>
    /// <param name="lines">The input lines.</param>
    /// <returns>The sequence of reports.</returns>
    private static IEnumerable<int[]> ParseInput(string[] lines)
    {
        return lines.Select(l => l.Split(" ", StringSplitOptions.TrimEntries).Select(int.Parse).ToArray());
    }
    
    /// <summary>
    /// Checks if the report is safe.
    /// A report is safe if all the differences between the numbers are either positive or negative,
    /// if the differences are not greater than 3 and if all differences are not equal to 0.
    /// </summary>
    /// <param name="report">The report.</param>
    /// <returns>True if the report is safe, false otherwise.</returns>
    private static bool IsSave(int[] report)
    {
        List<int> differences = [];
        
        for (int i = 1; i < report.Length; i++)
        {
            int diff = report[i] - report[i - 1];
            differences.Add(diff);
        }
        
        bool positive = differences.Any(d => d > 0);
        bool negative = differences.Any(d => d < 0);
        
        bool invalidDifferences = positive && negative;
        
        if (invalidDifferences)
        {
            return false;
        }

        return !differences.Distinct().Any(d => d == 0 || Math.Abs(d) > 3);
    }
    
    public static void Main()
    {
        // string input = File.ReadAllText("Day02.txt");
        string input = """
                       7 6 4 2 1
                       1 2 7 8 9
                       9 7 6 2 1
                       1 3 2 4 5
                       8 6 4 4 1
                       1 3 6 7 9
                       """;
        
        string[] lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        lines = File.ReadAllLines("Day02.txt");
        IEnumerable<int[]> reports = ParseInput(lines);

        int correctReports = 0;
        int toleratedReports = 0;
        
        foreach (int[] report in reports)
        {
            bool isSafe = IsSave(report);
            
            if (isSafe)
            {
                // add to first puzzle
                correctReports++;
                // add to second puzzle
                toleratedReports++;
                continue;
            }
            
            for (int i = 0; i < report.Length; i++)
            {
                // array without the i-th element
                int[] newReport = report.Take(i).Concat(report.Skip(i + 1)).ToArray();
                // check if the new report is safe
                bool isSafeWithout = IsSave(newReport);
                
                if (!isSafeWithout) continue;
                
                toleratedReports++;
                // break the inner loop
                break;
            }
        }

        Console.WriteLine($"The number of correct reports is: {correctReports}");
        Console.WriteLine($"The number of tolerated reports is: {toleratedReports}");
    }
}