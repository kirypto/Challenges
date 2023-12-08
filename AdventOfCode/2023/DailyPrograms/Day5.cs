using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;
using static kirypto.AdventOfCode._2023.DailyPrograms.AlmanacMapEntry;
using AlmanacRanges = C5.TreeDictionary<long, long>;
using AlmanacRangeEntry = C5.KeyValuePair<long, long>;

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
        almanacMaps.ForEach(am => am.PrintToConsole());

        almanacMaps[0].MergeWith(almanacMaps[1]);
        throw new NotImplementedException();

        long minLocation = seeds
                .Select(seed => almanacMaps.Aggregate(seed, (curr, map) => map.Map(curr)))
                .Min();
        Console.WriteLine($"Min seed location: {minLocation}");

        Console.WriteLine("\n----\n");
        var mergedMap = almanacMaps.Take(2).Aggregate((am1, am2) => am1.MergeWith(am2));
        mergedMap.PrintToConsole();

        seeds.ForEach(seed => Console.WriteLine($"Seed {seed} -> {mergedMap.Map(seed)}"));

        Console.WriteLine("\n----\n");
        almanacMaps[2].PrintToConsole();

        Console.WriteLine("\n----\n");
        mergedMap.MergeWith(almanacMaps[2]).PrintToConsole();


        throw new NotImplementedException();
    }

    private static IEnumerable<long> LongRange(long start, long count) {
        for (long i = 0; i < count; i++) {
            yield return start + i;
        }
    }
}

public readonly record struct AlmanacMapEntry(long destinationRangeStart, long sourceRangeStart, long rangeLength) {
    public static AlmanacMapEntry AlmanacMapEntryFrom(long start, long rangeLength, long mappingValue) {
        return new AlmanacMapEntry(
                destinationRangeStart: start + mappingValue,
                sourceRangeStart: start,
                rangeLength: rangeLength);
    }
}

public readonly record struct AlmanacMap {
    private readonly AlmanacRanges _ranges;
    private string Name { get; }

    public AlmanacMap(string name, ICollection<AlmanacMapEntry> entries) {
        Name = name;
        var ranges = new AlmanacRanges { [long.MinValue] = 0 };
        foreach ((long destinationRangeStart, long sourceRangeStart, long rangeLength) in entries
                         .OrderBy(e => e.sourceRangeStart)) {
            AlmanacRangeEntry newAfter = ranges.WeakPredecessor(sourceRangeStart + rangeLength);
            AlmanacRangeEntry existingBefore = ranges.WeakPredecessor(sourceRangeStart);
            if (newAfter != existingBefore) {
                throw new NotImplementedException("Clean slice insert doesn't work...");
            }
            ranges[sourceRangeStart + rangeLength] = newAfter.Value;
            ranges[sourceRangeStart] = destinationRangeStart - sourceRangeStart;
        }

        _ranges = ReduceIdenticalRanges(name, ranges);
    }

    public long Map(long input) {
        return input + _ranges.WeakPredecessor(input).Value;
    }

    public AlmanacMap MergeWith(AlmanacMap other, string? name = null) {
        var newMapEntries = new List<AlmanacMapEntry>();
        var currKey = long.MinValue;
        while (true) {
            AlmanacRangeEntry nextA = _ranges.TrySuccessor(currKey, out AlmanacRangeEntry nA)
                    ? nA
                    : new AlmanacRangeEntry(long.MaxValue, 0);
            AlmanacRangeEntry nextB = other._ranges.TrySuccessor(currKey, out AlmanacRangeEntry nB)
                    ? nB
                    : new AlmanacRangeEntry(long.MaxValue, 0);

            long nextKey = Math.Min(nextA.Key, nextB.Key);

            if (nextKey == long.MaxValue) {
                break;
            }

            newMapEntries.Add(AlmanacMapEntryFrom(
                    start: currKey,
                    nextKey - currKey,
                    _ranges.WeakPredecessor(currKey).Value + other._ranges.WeakPredecessor(currKey).Value));
            currKey = nextKey;
        }
        return new AlmanacMap(
                name ?? $"{Name} -> {other.Name}",
                newMapEntries);
    }

    public void PrintToConsole() {
        Console.WriteLine($"Almanac Map '{Name}'");
        foreach (AlmanacRangeEntry keyValuePair in _ranges) {
            Console.WriteLine($"{keyValuePair.Key} -> {keyValuePair.Value}");
        }
    }

    private static AlmanacRanges ReduceIdenticalRanges(string name, AlmanacRanges range) {
        var rangeReduced = new AlmanacRanges();
        AlmanacRangeEntry? last = null;
        foreach (AlmanacRangeEntry entry in range) {
            if (last != null && last.Value.Value == entry.Value) {
                Console.WriteLine(
                        $"{name} has same value ({entry.Value}) at both {last.Value.Key} and {entry.Key}, combining.");
                continue;
            }
            rangeReduced[entry.Key] = entry.Value;
            last = entry;
        }
        return rangeReduced;
    }
}
