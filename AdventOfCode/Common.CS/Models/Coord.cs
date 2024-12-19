using System;
using static kirypto.AdventOfCode.Common.Models.CardinalDirection;

namespace kirypto.AdventOfCode.Common.Models;

public readonly record struct Coord(int X, int Y) : IComparable<Coord> {
    public int CompareTo(Coord other) {
        int yComparison = Y.CompareTo(other.Y);
        return yComparison != 0 ? yComparison : X.CompareTo(other.X);
    }
}
public static class CoordExtensions {
    public static Coord Move(this Coord coord, CardinalDirection direction) {
        return direction switch {
                North => coord with { Y = coord.Y - 1 },
                East => coord with { X = coord.X + 1 },
                South => coord with { Y = coord.Y + 1 },
                West => coord with { X = coord.X - 1 },
                _ => throw new ArgumentException($"Unsupported direction {direction}", nameof(direction)),
        };
    }

    public static int ManhattanDistanceTo(this Coord a, Coord b) {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
}
