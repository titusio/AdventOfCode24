namespace AdventOfCode.Day04;

class Program
{
    /// <summary>
    /// Parse a string into a 2D char array
    /// </summary>
    /// <param name="input"> The string to parse </param>
    /// <returns> The 2D char array </returns>
    private static char[][] ParseString(string input)
    {
        string[] lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        char[][] result = new char[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            result[i] = lines[i].ToCharArray();
        }

        return result;
    }

    /// <summary>
    /// Count the number of occurrences of the word "XMAS" or "SAMX" in a char array
    /// </summary>
    /// <param name="line"> The char array to search in </param>
    /// <returns> The number of occurrences of the word "XMAS" or "SAMX" </returns>
    private static int CountInLine(char[] line)
    {
        int i = 0;
        int result = 0;
        while (i < line.Length)
        {
            if (line.Length - i < 4)
                break;

            if (!(line[i] == 'X' || line[i] == 'S'))
            {
                i++;
                continue;
            }

            // check if the next chars result in "XMAS" or "SAMX"
            bool ltr = line[i] == 'X' && line[i + 1] == 'M' && line[i + 2] == 'A' && line[i + 3] == 'S';
            bool rtl = line[i] == 'S' && line[i + 1] == 'A' && line[i + 2] == 'M' && line[i + 3] == 'X';

            if (ltr || rtl)
            {
                result += 1;
            }

            i++;
        }

        return result;
    }

    /// <summary>
    /// Check the diagonals of a 2D char array for the pattern "XMAS" or "SAMX"
    /// </summary>
    /// <param name="map"> The 2D char array to search in </param>
    /// <returns> The number of occurrences of the pattern </returns>
    /// <exception cref="ArgumentException"> Thrown when the lines have different lengths </exception>
    private static int CheckDiagonals(char[][] map)
    {
        // the first array are the lines
        // the second array are the columns

        int height = map.Length;
        int width = map[0].Length;

        if (map.Any(c => c.Length != width))
            throw new ArgumentException("All lines must have the same length");

        int result = 0;


        for (int w = 0; w < width; w++)
        {
            result += FindLtR(w, 0);
            result += FindRtL(w, 0);
        }

        for (int h = 1; h < height; h++)
        {
            result += FindLtR(0, h);
            result += FindRtL(0, h);
        }

        return result;

        char Access(int x, int y)
        {
            // check if the coordinates are valid
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                Console.WriteLine($"Invalid coordinates {x}, {y}");
                return '\0';
            }

            return map[y][x];
        }

        int FindLtR(int offW, int offH)
        {
            int findResult = 0;
            int i = 0;
            while (i < width)
            {
                int x = i + offW;
                int y = i + offH;

                // reached the end of the diagonal line
                if (width - x < 4 || height - y < 4)
                    break;

                char c0 = Access(x, y);
                if (c0 != 'X' && c0 != 'S')
                {
                    i++;
                    continue;
                }

                // check if the next chars result in "XMAS" or "SAMX"
                char c1 = Access(x + 1, y + 1);
                char c2 = Access(x + 2, y + 2);
                char c3 = Access(x + 3, y + 3);
                bool ltr = c0 == 'X' && c1 == 'M' && c2 == 'A' && c3 == 'S';
                bool rtl = c0 == 'S' && c1 == 'A' && c2 == 'M' && c3 == 'X';

                if (ltr || rtl)
                {
                    findResult += 1;
                }

                i += 1;
            }

            return findResult;
        }

        int FindRtL(int offW, int offH)
        {
            int findResult = 0;
            int i = 0;
            while (i < width)
            {
                int x = width - i - 1 - offW;
                int y = i + offH;

                // reached the end of the diagonal line
                if (x - 3 < 0 || height - 1 - y < 3)
                {
                    i++;
                    // not break because this will happen at the start of the line
                    continue;
                }

                char c0 = Access(x, y);
                if (c0 != 'X' && c0 != 'S')
                {
                    i++;
                    continue;
                }

                // check if the next chars result in "XMAS" or "SAMX"
                char c1 = Access(x - 1, y + 1);
                char c2 = Access(x - 2, y + 2);
                char c3 = Access(x - 3, y + 3);
                bool ltr = c0 == 'X' && c1 == 'M' && c2 == 'A' && c3 == 'S';
                bool rtl = c0 == 'S' && c1 == 'A' && c2 == 'M' && c3 == 'X';

                if (ltr || rtl)
                {
                    findResult += 1;
                }

                i += 1;
            }

            return findResult;
        }
    }

    /// <summary>
    /// searches for the pattern "SAMX" or "XMAS" in a 3x3 square
    /// </summary>
    /// <param name="map"> The map to search in </param>
    /// <returns> The number of occurrences of the pattern </returns>
    private static int Second(char[][] map)
    {
        int result = 0;

        for (int y = 0; y < map.Length - 2; y++)
        {
            for (int x = 0; x < map[0].Length - 2; x++)
            {
                char center = map[y + 1][x + 1];
                if (center != 'A')
                    continue;

                char tl = map[y][x];
                char tr = map[y][x + 2];
                char bl = map[y + 2][x];
                char br = map[y + 2][x + 2];

                bool rtl = tl == 'M' && br == 'S' || tl == 'S' && br == 'M';
                bool ltr = tr == 'M' && bl == 'S' || tr == 'S' && bl == 'M';

                if (rtl && ltr)
                {
                    result += 1;
                }
            }
        }

        return result;
    }

    private static void Main()
    {
        string input = File.ReadAllText("Day04.txt");

        char[][] map = ParseString(input);

        int result = map.Sum(CountInLine);

        for (int i = 0; i < map[0].Length; i++)
        {
            char[] column = new char[map.Length];
            for (int j = 0; j < map.Length; j++)
            {
                column[j] = map[j][i];
            }

            result += CountInLine(column);
        }

        result += CheckDiagonals(map);

        Console.WriteLine($"The result is {result}"); // 2496 

        int result2 = Second(map);
        Console.WriteLine($"The second result is {result2}"); // 1967
    }
}