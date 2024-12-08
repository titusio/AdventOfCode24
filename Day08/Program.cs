namespace Day08;

public class Antenna
{
    public required char Type { get; init; }
    public required int X { get; init; }
    public required int Y { get; init; }
}

class Program
{
    private static List<Antenna> ParseInput(string input, out int width, out int height)
    {
        string[] lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        List<Antenna> antennae = [];

        height = lines.Length;
        width = lines[0].Length;

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                char c = lines[y][x];
                if (c == '.') continue;

                antennae.Add(new Antenna()
                {
                    Type = c,
                    X = x,
                    Y = y
                });
            }
        }

        return antennae;
    }

    public static int CountAntinodes(List<Antenna> antennae, int width, int height)
    {
        HashSet<ValueTuple<int, int>> antinodes = [];

        foreach (Antenna antennaA in antennae)
        {
            foreach (Antenna antennaB in antennae)
            {
                if (antennaA == antennaB || antennaA.Type != antennaB.Type) continue;

                int dx = antennaB.X - antennaA.X;
                int dy = antennaB.Y - antennaA.Y;

                int aX = antennaA.X - dx;
                int aY = antennaA.Y - dy;
                int bX = antennaB.X + dx;
                int bY = antennaB.Y + dy;

                antinodes.Add((aX, aY));

                antinodes.Add((bX, bY));
            }
        }

        return antinodes.Count;
    }

    static void Main(string[] args)
    {
        string input = """
                       ............
                       ........0...
                       .....0......
                       .......0....
                       ....0.......
                       ......A.....
                       ............
                       ............
                       ........A...
                       .........A..
                       ............
                       ............
                       """;

        input = File.ReadAllText("Day08.txt");
        List<Antenna> antennae = ParseInput(input, out int width, out int height);
        int antinodes = CountAntinodes(antennae, width, height);
        Console.WriteLine($"Found {antinodes} antinodes");
    }
}