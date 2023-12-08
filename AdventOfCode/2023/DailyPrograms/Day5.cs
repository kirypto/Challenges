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

        for (var count = 2; count <= almanacMaps.Count; count++) {
            AlmanacMap mergedMap = Enumerable.Range(0, count)
                    .Select(index => almanacMaps[index])
                    .Aggregate((am1, am2) => am1.MergeWith(am2));
            Console.WriteLine($"With Almanac Map {mergedMap.Name}");
            seeds.ForEach(seed => Console.WriteLine($" --> Seed {seed} maps to {mergedMap.Map(seed)}"));
        }
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
    public string Name { get; }

    public AlmanacMap(string name, ICollection<AlmanacMapEntry> entries)
            : this(name, DeriveAlmanacRangesFromMapEntries(entries)) { }

    private AlmanacMap(string name, AlmanacRanges ranges) {
        Name = name;
        _ranges = ReduceIdenticalRanges(name, ranges);
    }

    public long Map(long input) {
        return input + _ranges.WeakPredecessor(input).Value;
    }

    public AlmanacMap MergeWith(AlmanacMap other, string? name = null) {
        var newRanges = new AlmanacRanges();
        foreach (long rangeAStart in _ranges.Keys) {
            long currTransform = _ranges[rangeAStart];
            long rangeAEnd = _ranges.TrySuccessor(rangeAStart, out AlmanacRangeEntry successor)
                    ? successor.Key - 1
                    : long.MaxValue;

            long rangeBStart = rangeAStart + currTransform;
            long rangeBEnd = rangeAEnd + currTransform;
            List<AlmanacRangeEntry> foo;
            if (other._ranges.Contains(rangeBStart)) {
                foo = other._ranges.RangeFromTo(rangeBStart, rangeBEnd + 1).ToList();
            } else {
                foo = new List<AlmanacRangeEntry> { other._ranges.Predecessor(rangeBStart) }
                        .Concat(other._ranges.RangeFromTo(rangeBStart, rangeBEnd + 1)).ToList();
            }
            foreach (AlmanacRangeEntry entry in foo) {
                long newKey = entry.Key != long.MinValue && rangeAStart < entry.Key - currTransform
                        ? entry.Key - currTransform
                        : rangeAStart;
                newRanges[newKey] = currTransform + entry.Value;
            }
        }
        return new AlmanacMap(
                name ?? $"{Name} -> {other.Name}",
                newRanges);
    }

    public void PrintToConsole() {
        Console.WriteLine($"Almanac Map '{Name}'");
        foreach (AlmanacRangeEntry keyValuePair in _ranges) {
            Console.WriteLine($"{keyValuePair.Key} -> {keyValuePair.Value}");
        }
    }

    private static AlmanacRanges DeriveAlmanacRangesFromMapEntries(ICollection<AlmanacMapEntry> entries) {
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
        return ranges;
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
