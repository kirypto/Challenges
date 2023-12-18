using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Models;
using kirypto.AdventOfCode._2023.Repos;
using static System.Math;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day11 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        IList<string> inputLines = inputRepository.FetchLines(inputRef);
        ISet<Position> stars = new HashSet<Position>();
        int rowCount = inputLines.Count;
        int colCount = inputLines[0].Length;
        var rowsWithStars = new bool[rowCount];
        var colsWithStars = new bool[colCount];
        for (var row = 0; row < rowCount; row++) {
            for (var col = 0; col < colCount; col++) {
                if (inputLines[row][col] == '#') {
                    rowsWithStars[row] = true;
                    colsWithStars[col] = true;
                    stars.Add(new Position(row, col));
                }
            }
        }
        // Not sure why exactly the the request in "making it 1000000 times longer" required 1000000 - 1. For the
        // samples, 9 (10 - 1) matched the "if 10 times longer" value, and same for 99 (100 - 1) for "if 100 times
        // longer".
        int shiftPerBlankRow = part == 1 ? 1 : 1000000 - 1;
        IList<int> rowShift = Enumerable.Range(0, rowCount)
                .Select(index => rowsWithStars.Where((b, i) => !b && i < index).Count() * shiftPerBlankRow)
                .ToList();
        IList<int> colShift = Enumerable.Range(0, colCount)
                .Select(index => colsWithStars.Where((b, i) => !b && i < index).Count() * shiftPerBlankRow)
                .ToList();
        // Console.WriteLine(string.Join(",", rowShift));
        // Console.WriteLine(string.Join(",", colShift));
        ISet<Position> previousStars = new HashSet<Position>();
        long sumOfAllPairDistances = stars
                .Order()
                // .Tap(star => Console.Write($"{star} --> "))
                .Select(star => new Position(star.Row + rowShift[star.Row], star.Col + colShift[star.Col]))
                // .Tap(star => Console.Write($"{star}"))
                .Select(star => new {
                        Star = star,
                        SumOfDistancesSoFar = previousStars
                                .Aggregate(0L, (curr, other) => curr + star.ManhattenDistanceTo(other)),
                })
                .Tap(obj => previousStars.Add(obj.Star))
                .Select(obj => obj.SumOfDistancesSoFar)
                .Sum();
        Console.WriteLine($"Sum of all pair star distances: {sumOfAllPairDistances}");
    }
}

public static class PositionExtensions {
    public static int ManhattenDistanceTo(this Position source, Position destination) {
        return Abs(destination.Row - source.Row) + Abs(destination.Col - source.Col);
    }
}
