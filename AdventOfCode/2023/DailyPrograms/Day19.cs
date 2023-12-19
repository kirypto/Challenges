using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;
using static kirypto.AdventOfCode._2023.DailyPrograms.XMASValue;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day19 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        string[] workflowAndParts = inputRepository.Fetch(inputRef)
                .Trim()
                .Split("\n\n");

        IEnumerable<XMASValue> parts = workflowAndParts[1]
                .Split("\n")
                .Select(ParseXMASValue);
        parts.ForEach(xmasVal => Console.WriteLine(xmasVal));
        throw new NotImplementedException();
    }
}

public readonly record struct XMASValue(int X, int M, int A, int S) {
    private static readonly Regex XMASMatcher = new(@"^\{x=(\d+),m=(\d+),a=(\d+),s=(\d+)\}$", RegexOptions.Compiled);
    public static XMASValue ParseXMASValue(string raw) {
        var match = XMASMatcher.Match(raw);
        return new XMASValue(
                int.Parse(match.Groups[1].Value),
                int.Parse(match.Groups[2].Value),
                int.Parse(match.Groups[3].Value),
                int.Parse(match.Groups[4].Value)
        );
    }

}
