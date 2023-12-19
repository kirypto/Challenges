using System;
using System.Collections.Generic;
using static System.Math;

namespace kirypto.AdventOfCode._2023.Models;

public readonly record struct Position(int Row, int Col) : IComparable<Position> {
    public int CompareTo(Position other) {
        int rowComparison = Row.CompareTo(other.Row);
        return rowComparison != 0 ? rowComparison : Col.CompareTo(other.Col);
    }
}

public static class PositionExtensions {
    public static int ManhattenDistanceTo(this Position source, Position destination) {
        return Abs(destination.Row - source.Row) + Abs(destination.Col - source.Col);
    }

    public static IEnumerable<Position> GetAdjacentPositions(this Position position) {
        yield return position with { Row = position.Row - 1 };
        yield return position with { Row = position.Row + 1 };
        yield return position with { Col = position.Col - 1 };
        yield return position with { Col = position.Col + 1 };
    }
}
