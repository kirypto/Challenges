namespace kirypto.AdventOfCode._2023.Extensions;

public static class Array2dExtensions {
    public delegate R Mask<in T, out R>(T[,] t, int row, int col);

    public static R[,] ApplyMask<T, R>(this T[,] array, Mask<T, R> mask) {
        var result = new R[array.GetLength(0), array.GetLength(1)];
        for (var row = 0; row < array.GetLength(0); row++) {
            for (var col = 0; col < array.GetLength(1); col++) {
                result[row, col] = mask(array, row, col);
            }
        }
        return result;
    }

    public static void PrintToConsole<T>(this T[,] array) {
        for (var i = 0; i < array.GetLength(0); i++) {
            for (var j = 0; j < array.GetLength(1); j++) {
                Console.Write($"{array[i, j]} ");
            }
            Console.WriteLine();
        }
    }

    public static T[] GetRow<T>(this T[,] array, int row) {
        return Enumerable.Range(0, array.GetLength(1))
                .Select(col => array[row, col])
                .ToArray();
    }
}