namespace Day09;

class Program
{
    private static List<int?> ProcessInput(string input)
    {
        List<int?> ids = [];

        int id = 0;

        for (int i = 0; i < input.Length; i++)
        {
            int number = int.Parse(input[i].ToString());

            for (int j = 0; j < number; j++)
            {
                ids.Add(i % 2 == 0 ? id : null);
            }

            if (i % 2 == 0) id++;
        }

        return ids;
    }

    static void Main(string[] args)
    {
        string input = "2333133121414131402";
        input = File.ReadAllText("Day09.txt").Replace("\n", "");
        ProcessInput(input);

        List<int?> ids = ProcessInput(input);

        int index = ids.Count - 1;

        while (ids.Count(id => !id.HasValue) != 0)
        {
            while (ids[index] == null)
            {
                index -= 1;
            }

            int start = 0;

            while (start <= index)
            {
                if (!ids[start].HasValue) break;
                start += 1;
            }

            if (start > index) break;

            ids[start] = ids[index];
            ids[index] = null;
        }

        ids = ids.Where(id => id.HasValue).ToList();

        long sum = 0;

        for (int i = 0; i < ids.Count; i++)
        {
            sum += ids[i]!.Value * i;
        }

        Console.WriteLine(sum);
    }
}