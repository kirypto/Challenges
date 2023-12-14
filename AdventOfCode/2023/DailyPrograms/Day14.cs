using System;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;
using static System.Linq.Enumerable;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day14 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        char[,] map = inputRepository.Fetch(inputRef)
                .Trim()
                .To2DCharArray("\n");
        int rowCount = map.GetLength(0);
        int colCount = map.GetLength(1);
        int totalLoad = Range(0, colCount)
                .Select(col => {
                    int lastRockIndex = -1;
                    // Console.Write($"[Col {col}] ");
                    int sum = map.EnumerateCol(col)
                            .Select((c, i) => c switch {
                                    'O' => rowCount - ++lastRockIndex,
                                    '#' => (lastRockIndex = i) - lastRockIndex,
                                    '.' => 0,
                                    _ => throw new NotImplementedException($"Unhandled char: {c}"),
                            })
                            // .Tap(load => Console.Write($"{load} "))
                            .Sum();
                    // Console.WriteLine($"--> {sum}");
                    return sum;
                })
                .Sum();

        Console.WriteLine($"Total load: {totalLoad}");
    }
}
