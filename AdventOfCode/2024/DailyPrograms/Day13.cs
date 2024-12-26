using System;
using System.Linq;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Models;
using kirypto.AdventOfCode.Common.Repositories;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(13)]
public partial class Day13 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        inputRepository.Fetch()
                .Split("\n\n")
                .Select(Puzzle.Parse)
                .ToList();
        throw new NotImplementedException();
    }

    private readonly partial record struct Puzzle(Coord ADiff, Coord BDiff, Coord Goal) {
        public static Puzzle Parse(string input) {
            string[] inputLines = input.Split("\n");
            Match aMatch = ButtonPattern().Match(inputLines[0]);
            Match bMatch = ButtonPattern().Match(inputLines[1]);
            Match goalMatch = Regex.Match(inputLines[2], @"Prize: X=(\d+), ");

            return new Puzzle { };
        }
    }

        [GeneratedRegex(@"Button A?B?: X\+(\d+), Y\+(\d)")]
        private static partial Regex ButtonPattern();
    }
