using System;
using System.Linq;
using kirypto.AdventOfCode.Common.Models;
using static System.Linq.Enumerable;

namespace kirypto.AdventOfCode.Common.Extensions;

public static class ArrayExtensions {
    public static bool TryGetValue<T>(this T[,] map, int row, int col, out T value) {
        if (row < 0 || col < 0 || row >= map.GetLength(0) || col >= map.GetLength(1)) {
            value = default;
            return false;
        }
        value = map[row, col];
        return true;
    }

    public static string ToString<T>(this T[,] map, Func<T, Coord, string> cellToString) {
        return string.Join("\n", Range(0, map.GetLength(0))
                .Select(row => Range(0, map.GetLength(1))
                        .Select(col => cellToString(map[row, col], new Coord {Y = row, X = col}))
                        .Aggregate((s1, s2) => $"{s1}{s2}")));
    }
}
