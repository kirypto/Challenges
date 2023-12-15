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
            Console.Write("Running Tilt Cycles... 0%");
            const int totalCycles = 1000000000;
            DateTime start = DateTime.Now;
            for (var i = 0; i < totalCycles; i++) {
                rockPlatform.TiltCycle();
                if (i % 1000 == 0) {
                    float percentDone = 100f * i / totalCycles;
                    double tiltsPerSecond = i / (DateTime.Now - start).TotalSeconds;
                    Console.Write($"                   \r" +
                            $"Running, {percentDone:F5}% done @ {tiltsPerSecond:F2} tilts cycles per second...");
                    if (i % 10_000_000 == 0) {
                        Console.WriteLine($"\nBackup of cycle {i}:");
                        rockPlatform.PrintToConsole();
                    }
                }
            }
            Console.WriteLine("\nFinal state:");
            rockPlatform.PrintToConsole();
            Console.WriteLine($"Total northern load: {rockPlatform.DetermineNorthernLoad()}");

            // throw new NotImplementedException();
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

    private static void TiltEast(this char[,] rockPlatform) {
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

    private static void TiltSouth(this char[,] rockPlatform) {
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

    private static void TiltWest(this char[,] rockPlatform) {
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
