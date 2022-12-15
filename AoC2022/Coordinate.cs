using System.Text.RegularExpressions;

namespace AoC2022
{
    // Many of the quality of life features from https://markheath.net/post/coord-performance-versus-readability
    public class Coordinate
    {
        public int X { get; }
        public int Y { get; }

        public Coordinate(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public Coordinate(string stringWithCoordinates)
        {
            const string intRegexPattern = @"-?\d+";
            var matches = Regex.Matches(stringWithCoordinates, intRegexPattern);

            var integers = matches
                .Take(2)
                .Select(match => int.Parse(match.Value))
                .ToArray();

            X = integers[0];
            Y = integers[1];
        }

        public static Coordinate Create(int x = 0, int y = 0)
        {
            return new Coordinate(x, y);
        }

        public static Coordinate Create(string stringWithCoordinates)
        {
            return new Coordinate(stringWithCoordinates);
        }

        public void Deconstruct(out int x, out int y)
        {
            x = X;
            y = Y;
        }

        // TODO: Ideally would override Equal(object? other) but would need to override the GetHashCode() also
        public bool SameCoordinate(Coordinate other)
        {
            return X == other.X && Y == other.Y;
        }

        public static implicit operator (int, int)(Coordinate c) =>
            (c.X, c.Y);
        public static implicit operator Coordinate((int X, int Y) c) =>
            new (c.X, c.Y);
        
        public override string ToString() => $"({X},{Y})";


        public static Coordinate operator +(Coordinate a, Coordinate b)
        {
            return new Coordinate(a.X + b.X, a.Y + b.Y);
        }

        public static int ManhattanDistance(Coordinate a, Coordinate b)
            => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

        public int ManhattanDistance(Coordinate other)
            => ManhattanDistance(this, other);
    }
}
