using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode._2023.Repos;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day12 : IDailyProgram {
    private const string prefix = @"^([\.\?]*?)";
    private const string interimOperational = @"([\.\?]+?)";
    private const string damageGroupPrefix = @"([\#\?]{";
    private const string damageGroupSuffix = "})";
    private const string suffix = @"([\.\?]*?)$";

    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        List<Part> parts = inputRepository.FetchLines(inputRef)
                .Select(line => line.Split(" "))
                .Select(splitLine => new Part(splitLine[0], ConstructConditionMatcher(splitLine[1])))
                .ToList();
        int sum = parts.Select(p => p.PossibleConditionCount)
                .Sum();
        Console.WriteLine($"Sum: {sum}");
    }

    private static Regex ConstructConditionMatcher(string damageGroupsString) {
        string regexPattern = prefix
                + damageGroupsString.Split(",")
                        .Select(dg => damageGroupPrefix + dg + damageGroupSuffix)
                        .Aggregate((dg1, dg2) => dg1 + interimOperational + dg2)
                + suffix;
        return new Regex(regexPattern, RegexOptions.Compiled);
    }
}

public readonly record struct Part(string PartialCondition, Regex ConditionMatcher) {
    public int PossibleConditionCount => DerivePossibleConditionCount();

    private int DerivePossibleConditionCount() {
        var possibleConditions = new HashSet<string> { PartialCondition };
        var conditionMatcher = ConditionMatcher;
        // Console.Write($"Deriving '{PartialCondition}': ");
        for (var i = 0; i < PartialCondition.Length; i++) {
            possibleConditions = possibleConditions
                    .SelectMany(condition => SubstituteUnknown(condition, i))
                    .Where(condition => conditionMatcher.Match(condition).Success)
                    .ToHashSet();
            // Console.Write($"{possibleConditions.Count}, ");
        }
        // Console.WriteLine($"[final] {possibleConditions.Count}: ");
        // possibleConditions.ForEach(Console.WriteLine);
        return possibleConditions.Count;
    }

    private static IEnumerable<string> SubstituteUnknown(string condition, int index) {
        if (condition[index] != '?') {
            return new List<string> { condition };
        }
        return new List<char> { '.', '#' }
                .Select(replacement => {
                    char[] charArray = condition.ToCharArray();
                    charArray[index] = replacement;
                    return new string(charArray);
                });
    }
}
