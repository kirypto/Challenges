using System.Linq;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Interfaces;
using kirypto.AdventOfCode.Common.Models;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(13)]
public partial class Day13 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        inputRepository.Fetch()
                .Split("\n\n")
                .Select(Puzzle.Parse)
                .ToList();
        throw new System.NotImplementedException();
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
