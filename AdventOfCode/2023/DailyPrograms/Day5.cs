using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day5 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        IList<string> inputLines = inputRepository
                .FetchLines(inputRef)
                .Where(s => s.Any())
                .ToList();
        ISet<long> seeds;
        if (part == 1) {
            seeds = Regex.Matches(inputLines[0], @"(\d+)")
                    .Select(match => long.Parse(match.Value))
                    .ToHashSet();
        } else {
            seeds = Regex.Matches(inputLines[0], @"((\d+)\s+(\d+))")
                    .SelectMany(pairMatch => LongRange(
                            long.Parse(pairMatch.Groups[2].Value),
                            long.Parse(pairMatch.Groups[3].Value)))
                    .ToHashSet();
        }



        Console.WriteLine(seeds.Count);
        var currentEntryList = new List<AlmanacMapEntry>();
        IList<AlmanacMap> almanacMaps = new List<AlmanacMap>();
        var currentMap = "";
        foreach (string line in inputLines.Skip(1)) {
            var mapNameMatch = Regex.Match(line, @"^([\w-]+)(?=( map:$))");
            MatchCollection matches = Regex.Matches(line, @"(\d+)");
            if (matches.Any()) {
                currentEntryList.Add(new AlmanacMapEntry(
                        long.Parse(matches[0].Value),
                        long.Parse(matches[1].Value),
                        long.Parse(matches[2].Value)
                ));
            } else {
                if (currentEntryList.Any()) {
                    almanacMaps.Add(new AlmanacMap(mapNameMatch.Value, currentEntryList));
                }
                currentEntryList = new List<AlmanacMapEntry>();
            }
        }
        almanacMaps.ForEach(am => am.PrintToConsole());
        throw new NotImplementedException();
    }

    private static IEnumerable<long> LongRange(long start, long count)
    {
        for (long i = 0; i < count; i++)
        {
            yield return start + i;
        }
    }
}