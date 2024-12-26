using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Models;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(13)]
public partial class Day13 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        List<Puzzle> puzzles = inputRepository.Fetch()
                .Split("\n\n")
                .Select(Puzzle.Parse)
                .ToList();

        foreach (Puzzle puzzle in puzzles) {
            Coord position = new();
            Logger.LogInformation("Trying puzzle {puzzle}", puzzle);
            position += puzzle.ADiff;
            Logger.LogInformation("After pressing 'A': {coord}", position);
            position += puzzle.BDiff;
            Logger.LogInformation("After pressing 'B': {coord}", position);
        }
        throw new NotImplementedException();
    }

    private readonly partial record struct Puzzle(Coord ADiff, Coord BDiff, Coord Goal) {
        public static Puzzle Parse(string input) {
            string[] inputLines = input.Split("\n");
            Match aMatch = ButtonPattern().Match(inputLines[0]);
            Match bMatch = ButtonPattern().Match(inputLines[1]);
            Match goalMatch = GoalPattern().Match(inputLines[2]);

            Puzzle puzzle = new() {
                    ADiff = new Coord {
                            X = int.Parse(aMatch.Groups[1].Value),
                            Y = int.Parse(aMatch.Groups[2].Value),
                    },
                    BDiff = new Coord {
                            X = int.Parse(bMatch.Groups[1].Value),
                            Y = int.Parse(bMatch.Groups[2].Value),
                    },
                    Goal = new Coord {
                            X = int.Parse(goalMatch.Groups[1].Value),
                            Y = int.Parse(goalMatch.Groups[2].Value),
                    },
            };
            Logger.LogInformation("New Puzzle: {puzzle}", puzzle);
            return puzzle;
        }

        [GeneratedRegex(@"Prize: X=(\d+), Y=(\d+)")]
        private static partial Regex GoalPattern();
    }

    [GeneratedRegex(@"Button A?B?: X\+(\d+), Y\+(\d)")]
    private static partial Regex ButtonPattern();
}
