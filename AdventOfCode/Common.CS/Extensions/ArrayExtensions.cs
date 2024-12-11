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
}
