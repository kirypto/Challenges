using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode._2023.Repos;
using AlmanacRange = C5.TreeDictionary<long, long>;

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


        var currentEntryList = new List<AlmanacMapEntry>();
        IList<AlmanacMap> almanacMaps = new List<AlmanacMap>();
        var mapName = "";
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
                    almanacMaps.Add(new AlmanacMap(mapName, currentEntryList));
                }
                mapName = mapNameMatch.Value;
                currentEntryList = new List<AlmanacMapEntry>();
            }
        }
        almanacMaps.Add(new AlmanacMap(mapName, currentEntryList));

        long minLocation = seeds
                .Select(seed => almanacMaps.Aggregate(seed, (curr, map) => map.Map(curr)))
                .Min();
        Console.WriteLine($"Min seed location: {minLocation}");
    }

    private static IEnumerable<long> LongRange(long start, long count) {
        for (long i = 0; i < count; i++) {
            yield return start + i;
        }
    }
}

public readonly record struct AlmanacMapEntry(long destinationRangeStart, long sourceRangeStart, long rangeLength);

public readonly record struct AlmanacMap {
    private readonly AlmanacRange _ranges;
    private string Name { get; }

    public AlmanacMap(string name, ICollection<AlmanacMapEntry> entries) {
        Name = name;
        var ranges = new AlmanacRange {
                [0] = 0,
        };
        foreach ((long destinationRangeStart, long sourceRangeStart, long rangeLength) in entries
                         .OrderBy(e => e.sourceRangeStart)) {
            C5.KeyValuePair<long, long> newAfter = ranges.WeakPredecessor(sourceRangeStart + rangeLength);
            C5.KeyValuePair<long, long> existingBefore = ranges.WeakPredecessor(sourceRangeStart);
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