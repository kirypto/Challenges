using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode.Common.Interfaces;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day6 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        Func<string,string> identityOrWhitespaceStripper = (part == 1)
                ? s => s
                : s => s.Replace(" ", "");
        IList<string> inputLines = inputRepository.FetchLines(inputRef)
                .Select(identityOrWhitespaceStripper)
                .ToList();
        List<long> times = Regex.Matches(inputLines[0], @"(\d+)")
                .Select(match => match.Value)
                .Select(long.Parse)
                .ToList();
        List<long> distances = Regex.Matches(inputLines[1], @"(\d+)")
                .Select(match => match.Value)
                .Select(long.Parse)
                .ToList();


        IList<long> winningPossibilitiesPerRace = new List<long>();
        for (var i = 0; i < times.Count; i++) {
            long time = times[i];
            long distance = distances[i];

            long low = 0;
            long high = time / 2;
            while (low < high - 1) {
                long mid = low + (high - low) / 2;
                long val = CalculateDistanceTraveled(mid, time);
                if (val <= distance) {
                    low = mid;
                } else {
                    high = mid;
                }
            }
            long almostWinningPossibilities = (time / 2 - high + 1) * 2;
            long winningPossibilities = almostWinningPossibilities - (time + 1) % 2;
            winningPossibilitiesPerRace.Add(winningPossibilities);
        }
        long winningPossibilityProduct = winningPossibilitiesPerRace.Aggregate((i1, i2) => i1 * i2);
        Console.WriteLine($"The product of all winning possibilities is: {winningPossibilityProduct}");
    }

    private static long CalculateDistanceTraveled(long buttonHeldSeconds, long raceDurationSeconds) {
        long timeInMotion = raceDurationSeconds - buttonHeldSeconds;
        return buttonHeldSeconds * timeInMotion;
    }
}