using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using C5;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day5 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        System.Collections.Generic.IList<string> inputLines = inputRepository
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
        System.Collections.Generic.IList<AlmanacMap> almanacMaps = new List<AlmanacMap>();
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

    private static IEnumerable<long> LongRange(long start, long count) {
        for (long i = 0; i < count; i++) {
            yield return start + i;
        }
    }
}

public readonly record struct AlmanacMapEntry(long destinationRangeStart, long sourceRangeStart, long rangeLength);

public readonly record struct AlmanacMap {
    private readonly TreeDictionary<long, long> _ranges;
    private string Name { get; }

    public AlmanacMap(string name, System.Collections.Generic.ICollection<AlmanacMapEntry> entries) {
        Name = name;
        var ranges = new TreeDictionary<long, long> {
                [0] = 0,
        };
        foreach ((long destinationRangeStart, long sourceRangeStart, long rangeLength) in entries
                         .OrderBy(e => e.sourceRangeStart)) {
            C5.KeyValuePair<long,long> newAfter = ranges.WeakPredecessor(sourceRangeStart + rangeLength);
            C5.KeyValuePair<long,long> existingBefore = ranges.WeakPredecessor(sourceRangeStart);
            if (newAfter != existingBefore) {
                throw new NotImplementedException("Clean slice insert doesn't work...");
            }
            ranges[sourceRangeStart + rangeLength] = newAfter.Value;
            ranges[sourceRangeStart] = destinationRangeStart - sourceRangeStart;
        }
        _ranges = ranges;
    }

    public long Map(long input) {
        return input + _ranges.WeakPredecessor(input).Value;
    }

    public void PrintToConsole() {
        Console.WriteLine($"Almanac Map '{Name}'");
        foreach (C5.KeyValuePair<long, long> keyValuePair in _ranges) {
            Console.WriteLine($"{keyValuePair.Key} -> {keyValuePair.Value}");
        }
    }
}