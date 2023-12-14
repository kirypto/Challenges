using System;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;
using static System.Linq.Enumerable;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day14 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        char[,] rockPlatform = inputRepository.Fetch(inputRef)
                .Trim()
                .To2DCharArray("\n");

        if (part == 1) {
            rockPlatform.TiltNorth();
            // rockPlatform.PrintToConsole();
            int totalLoad = rockPlatform.DetermineNorthernLoad();
            Console.WriteLine($"Total load: {totalLoad}");
        } else {
            rockPlatform.TiltCycle();
            rockPlatform.PrintToConsole();
            throw new NotImplementedException();
        }
    }
}

public static class RockPlatformExtensions {
    public static void TiltCycle(this char[,] rocPlatform) {
        rocPlatform.TiltNorth();
        rocPlatform.TiltWest();
        rocPlatform.TiltSouth();
        rocPlatform.TiltEast();
    }

    public static void TiltNorth(this char[,] rockPlatform) {
        int rowCount = rockPlatform.GetLength(0);
        int colCount = rockPlatform.GetLength(1);
        for (var col = 0; col < colCount; col++) {
            int lastRockIndex = -1;
            for (var row = 0; row < rowCount; row++) {
                char c = rockPlatform[row, col];
                switch (c) {
                    case 'O':
                        lastRockIndex++;
                        rockPlatform[row, col] = '.';
                        rockPlatform[lastRockIndex, col] = 'O';
                        break;
                    case '#':
                        lastRockIndex = row;
                        break;
                }
            }
        }
    }

    public static void TiltEast(this char[,] rockPlatform) {
        int rowCount = rockPlatform.GetLength(0);
        int colCount = rockPlatform.GetLength(1);
        for (var row = 0; row < rowCount; row++) {
            int lastRockIndex = colCount;
            for (int col = colCount - 1; col >= 0; col--) {
                char c = rockPlatform[row, col];
                switch (c) {
                    case 'O':
                        lastRockIndex--;
                        rockPlatform[row, col] = '.';
                        rockPlatform[row, lastRockIndex] = 'O';
                        break;
                    case '#':
                        lastRockIndex = col;
                        break;
                }
            }
        }
    }

    public static void TiltSouth(this char[,] rockPlatform) {
        int rowCount = rockPlatform.GetLength(0);
        int colCount = rockPlatform.GetLength(1);
        for (var col = 0; col < colCount; col++) {
            int lastRockIndex = rowCount;
            for (int row = rowCount - 1; row >= 0; row--) {
                char c = rockPlatform[row, col];
                switch (c) {
                    case 'O':
                        lastRockIndex--;
                        rockPlatform[row, col] = '.';
                        rockPlatform[lastRockIndex, col] = 'O';
                        break;
                    case '#':
                        lastRockIndex = row;
                        break;
                }
            }
        }
    }

    public static void TiltWest(this char[,] rockPlatform) {
        int rowCount = rockPlatform.GetLength(0);
        int colCount = rockPlatform.GetLength(1);
        for (var row = 0; row < rowCount; row++) {
            int lastRockIndex = -1;
            for (var col = 0; col < colCount; col++) {
                char c = rockPlatform[row, col];
                switch (c) {
                    case 'O':
                        lastRockIndex++;
                        rockPlatform[row, col] = '.';
                        rockPlatform[row, lastRockIndex] = 'O';
                        break;
                    case '#':
                        lastRockIndex = col;
                        break;
                }
            }
        }
    }

    public static int DetermineNorthernLoad(this char[,] rockPlatform) {
        int rowCount = rockPlatform.GetLength(0);
        int colCount = rockPlatform.GetLength(1);
        return Range(0, colCount)
                .Select(col => rockPlatform.EnumerateCol(col)
                        .Select((c, i) => c switch {
                                'O' => rowCount - i,
                                '#' => 0,
                                '.' => 0,
                                _ => throw new NotImplementedException($"Unhandled char: {c}"),
                        })
                        .Sum())
                .Sum();
    }
}
