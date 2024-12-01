
// process the input string and return an array of integers
static int[] GetNumbers(string input)
{
    IEnumerable<string> elements = input.Split(" ", StringSplitOptions.TrimEntries);
    elements = elements.SelectMany(e => e.Split("\n", StringSplitOptions.TrimEntries));
    elements = elements.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

    try
    {
        int[] numbers = elements.Select(int.Parse).ToArray();
        return numbers;
    }
    catch (Exception e)
    {
        Console.WriteLine($"Failed to parse: input was not a number: {e}");
        throw;
    }
}

// Solution for Part one of Day 1.
// The task is to calculate the sum of the absolute differences between the smallest number in each pair of numbers.
static int GetDistances(int[] leftNumbersInput, int[] rightNumbersInput)
{
    int countedNumbers = 0;
    int result = 0;
    
    // copy the input arrays to avoid modifying the original arrays
    int[] leftNumbers = leftNumbersInput.ToArray();
    int[] rightNumbers = rightNumbersInput.ToArray();

    while (countedNumbers < leftNumbers.Length)
    {
        int smallestLeft = leftNumbers.Min();
        int leftIndex = Array.IndexOf(leftNumbers, smallestLeft);

        int smallestRight = rightNumbers.Min();
        int rightIndex = Array.IndexOf(rightNumbers, smallestRight);

        int distance = Math.Abs(smallestLeft - smallestRight);
        result += distance;

        leftNumbers[leftIndex] = int.MaxValue;
        rightNumbers[rightIndex] = int.MaxValue;

        countedNumbers++;
    }

    return result;
}

// solution for Part two of Day 1.
// The task is to calculate the sum of the products of the numbers that are the same in each pair of numbers.
static int GetSimilarity(int[] leftNumbers, int[] rightNumbers)
{
    int result = 0;
    
    foreach (int leftNumber in leftNumbers)
    {
        int matches = Enumerable.Range(0, leftNumbers.Length).Count(j => leftNumber == rightNumbers[j]);
        result += matches * leftNumber;
    }

    return result;
}

string input = """
               3   4
               4   3
               2   5
               1   3
               3   9
               3   3
               """;

input = File.ReadAllText("Day01.txt");
int[] numbers = GetNumbers(input);

if (numbers.Length % 2 != 0)
{
    throw new Exception("Invalid input: odd number of elements");
}

int[] leftNumbers = new int[numbers.Length / 2];
int[] rightNumbers = new int[numbers.Length / 2];

for (int i = 0; i < numbers.Length; i++)
{
    if (i % 2 == 0)
        leftNumbers[i / 2] = numbers[i];
    else
        rightNumbers[i / 2] = numbers[i];
}

int distances = GetDistances(leftNumbers, rightNumbers); // 2756096
Console.WriteLine("The sum of the distances is: " + distances); 

int similarity = GetSimilarity(leftNumbers, rightNumbers); // 23117829
Console.WriteLine("The similarity is: " + similarity);

