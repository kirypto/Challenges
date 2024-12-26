using System.Text.RegularExpressions;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(3)]
public partial class Day03 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        MatchCollection matchCollection = MulInstructionPattern().Matches(inputRepository.Fetch());
        Logger.LogInformation("Found {count} instructions", matchCollection.Count);
        int total = 0;
        foreach (Match match in matchCollection) {
            Logger.LogInformation(match.ToString());
            int valueA = int.Parse(match.Groups[1].Value);
            int valueB = int.Parse(match.Groups[2].Value);
            int result = valueA * valueB;
            Logger.LogInformation("{a} * {b} = {result}", valueA, valueB, result);
            total += result;
        }
        return total.ToString();
    }

    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex MulInstructionPattern();
}
