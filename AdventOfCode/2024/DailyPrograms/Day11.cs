using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;
using static System.StringSplitOptions;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

[DailyProgram(11)]
public partial class Day11 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        string[] inputLines = inputRepository.Fetch().Split("\n", RemoveEmptyEntries);
        string input = inputLines.Length > 1
                ? inputLines[1] // Example fetch returns extra lines
                : inputLines[0];

        Stack<EngravedStone> numbers = new();
        foreach (int num in NumberPattern().Matches(input).Select(m => int.Parse(m.Value)).Reverse()) {
            numbers.Push(new EngravedStone { Value = num });
        }

        int stepTarget = part == 1 ? 25 : 75;
        int stoneCount = 0;
        while (numbers.Count > 0) {
            EngravedStone currentStone = numbers.Pop();
            if (currentStone.Step >= stepTarget) {
                Logger.LogInformation($"Found {currentStone.Value} stone");
                stoneCount++;
            } else if (currentStone.Value == 0) {
                Logger.LogInformation($"Turned {currentStone.Value} into {1} stone");
                numbers.Push(new EngravedStone { Value = 1, Step = currentStone.Step + 1 });
            } else if (currentStone.Value.ToString().Length % 2 == 0) {
                string digits = currentStone.Value.ToString();
                string digitsA = digits[..(digits.Length / 2)];
                string digitsB = digits[(digits.Length / 2)..];
                Logger.LogInformation($"Turned {currentStone.Value} into {digitsA} and {digitsB} stone");
                numbers.Push(new EngravedStone { Value = long.Parse(digitsB), Step = currentStone.Step + 1 });
                numbers.Push(new EngravedStone { Value = long.Parse(digitsA), Step = currentStone.Step + 1 });
            } else {
                Logger.LogInformation($"Turned {currentStone.Value} into {currentStone.Value * 2024} stone");
                numbers.Push(new EngravedStone { Value = currentStone.Value * 2024, Step = currentStone.Step + 1 });
            }
        }
        return stoneCount.ToString();
    }

    [GeneratedRegex(@"(\d+)")]
    private static partial Regex NumberPattern();
}

public readonly record struct EngravedStone(long Value, int Step);
