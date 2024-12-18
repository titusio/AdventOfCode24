namespace AdventOfCode.Utils;

public struct Direction
{
    public static readonly Direction North = new(0, -1);
    public static readonly Direction South = new(0, 1);
    public static readonly Direction East = new(1, 0);
    public static readonly Direction West = new(-1, 0);

    public int X { get; set; }
    public int Y { get; set; }

    public Direction()
    {
        X = 0;
        Y = 0;
    }

    public Direction(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static bool operator ==(Direction a, Direction b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Direction a, Direction b)
    {
        return a.X != b.X || a.Y != b.Y;
    }

    public static Direction operator +(Direction a, Direction b)
    {
        return new Direction(a.X + b.X, a.Y + b.Y);
    }

    public static Direction operator -(Direction a, Direction b)
    {
        return new Direction(a.X - b.X, a.Y - b.Y);
    }

    public override string ToString()
    {
        return $"[{X}, {Y}]";
    }
}