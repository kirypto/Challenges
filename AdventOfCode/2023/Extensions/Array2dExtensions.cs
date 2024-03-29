﻿using System;
using System.Collections.Generic;
using System.Linq;
using static System.Linq.Enumerable;

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

    public static void PrintToConsole<T>(this T[,] array, int cellWidth = 2) {
        for (var i = 0; i < array.GetLength(0); i++) {
            for (var j = 0; j < array.GetLength(1); j++) {
                Console.Write($"{array[i, j]}".PadRight(cellWidth));
            }
            Console.WriteLine();
        }
    }
    public static IEnumerable<T> EnumerateRow<T>(this T[,] array, int row) {
        return Range(0, array.GetLength(1))
                .Select(col => array[row, col]);
    }

    public static IEnumerable<T> EnumerateCol<T>(this T[,] array, int col) {
        return Range(0, array.GetLength(0))
                .Select(row => array[row, col]);
    }

    public static T[] GetRow<T>(this T[,] array, int row) {
        return array.EnumerateRow(row).ToArray();
    }
}
