using System;

namespace kirypto.AdventOfCode._2023.Extensions;

public static class StringExtensions {
    public static string Reversed(this string s) {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public static char[,] To2DCharArray(this string s, string rowSplit) {
        string[] rows = s.Split(rowSplit);
        int rowCount = rows.Length;
        int colCount = rows[0].Length;
        var array = new char[rowCount, colCount];
        for (var row = 0; row < rowCount; row++) {
            if (rows[row].Length != colCount) {
                throw new InvalidOperationException("All split rows must be of the same length");
            }
            for (var col = 0; col < colCount; col++) {
                array[row, col] = rows[row][col];
            }
        }
        return array;
    }
}