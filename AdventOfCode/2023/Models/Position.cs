using System;

namespace kirypto.AdventOfCode._2023.Models;

public readonly record struct Position(int Row, int Col) : IComparable<Position> {
    public int CompareTo(Position other) {
        int rowComparison = Row.CompareTo(other.Row);
        return rowComparison != 0 ? rowComparison : Col.CompareTo(other.Col);
    }
}
