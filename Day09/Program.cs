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

    private static long ReorderIds(List<int?> ids)
    {
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

        long sum = CalculateChecksum(ids);

        return sum;
    }

    private static long Compress(List<int?> ids)
    {
        int last = ids.Count - 1;

        while (last > 0)
        {
            while (ids[last] == null && last >= 0)
            {
                last -= 1;
            }

            // the number we are looking at
            int number = ids[last]!.Value;
            // how many times it appears
            int count = 1;

            while (last > 1 && ids[last - 1].HasValue && ids[last - 1] == number)
            {
                count += 1;
                last -= 1;
            }

            int start = 0;

            for (start = 0; start < ids.Count; start++)
            {
                bool free = true;

                if (start + count >= ids.Count)
                {
                    start = last;
                    break;
                }

                for (int j = 0; j < count; j++)
                {
                    if (ids[start + j] == null) continue;
                    free = false;
                    break;
                }

                if (free) break;
            }

            if (start >= last)
            {
                last -= 1;
                continue;
            }

            // set the number at the start and nullify the rest
            for (int i = 0; i < count; i++)
            {
                ids[start + i] = number;
                ids[last + i] = null;
            }

            last -= 1;
        }

        return CalculateChecksum(ids);
    }

    private static long CalculateChecksum(List<int?> ids)
    {
        long sum = 0;

        for (int i = 0; i < ids.Count; i++)
        {
            sum += i * (ids[i] ?? 0);
        }

        return sum;
    }

    static void Main(string[] args)
    {
        string input = "2333133121414131402";
        input = File.ReadAllText("Day09.txt").Replace("\n", "");
        ProcessInput(input);

        List<int?> ids = ProcessInput(input);

        long sum = ReorderIds(ids.ToList());
        Console.WriteLine(sum);

        long compressed = Compress(ids.ToList());
        Console.WriteLine(compressed);
    }
}