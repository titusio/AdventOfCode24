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

    public static int CountAntinodes(List<Antenna> antennae, int width, int height, bool extend = false)
    {
        HashSet<ValueTuple<int, int>> antinodes = [];

        foreach (Antenna antennaA in antennae)
        {
            foreach (Antenna antennaB in antennae)
            {
                if (antennaA == antennaB || antennaA.Type != antennaB.Type) continue;

                int dx = antennaB.X - antennaA.X;
                int dy = antennaB.Y - antennaA.Y;

                if (extend)
                {
                    antinodes.Add((antennaA.X, antennaA.Y));
                    antinodes.Add((antennaB.X, antennaB.Y));
                }

                int aX = antennaA.X - dx;
                int aY = antennaA.Y - dy;
                int bX = antennaB.X + dx;
                int bY = antennaB.Y + dy;

                while (aX >= 0 && aX < width && aY >= 0 && aY < height)
                {
                    antinodes.Add((aX, aY));
                    aX -= dx;
                    aY -= dy;
                    if (!extend) break;
                }

                while (bX >= 0 && bX < width && bY >= 0 && bY < height)
                {
                    antinodes.Add((bX, bY));
                    bX += dx;
                    bY += dy;
                    if (!extend) break;
                }
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
        int extendedAntinodes = CountAntinodes(antennae, width, height, true);
        Console.WriteLine($"Found {extendedAntinodes} extended antinodes");
    }
}