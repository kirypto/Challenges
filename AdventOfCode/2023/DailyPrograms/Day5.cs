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

        var currentEntryList = new List<AlmanacMapEntry>();
        IList<AlmanacMap> almanacMaps = new List<AlmanacMap>();
        var mapName = "";
        foreach (string line in inputLines.Skip(1)) {
            if (TryParseAlmanacMapEntry(line, out AlmanacMapEntry almanacMapEntry)) {
                currentEntryList.Add(almanacMapEntry);
            } else {
                if (currentEntryList.Any()) {
                    almanacMaps.Add(new AlmanacMap(mapName, currentEntryList));
                }
                mapName = Regex.Match(line, @"^([\w-]+)(?=( map:$))").Value;
                currentEntryList = new List<AlmanacMapEntry>();
            }
        }
        almanacMaps.Add(new AlmanacMap(mapName, currentEntryList));

        AlmanacMap seedToLocationMap = almanacMaps
                .Aggregate((am1, am2) => am1.MergeWith(am2));

        if (part == 1) {
            long minLocation = Regex.Matches(inputLines[0], @"(\d+)")
                    .Select(seedMatch => long.Parse(seedMatch.Value))
                    .Select(seed => seedToLocationMap.Map(seed))
                    .Min();
            Console.WriteLine($"Lowest location number: {minLocation}");
        } else {
            throw new NotImplementedException();
        }
    }
}

public readonly record struct AlmanacMapEntry(long destinationRangeStart, long sourceRangeStart, long rangeLength) {
    public static bool TryParseAlmanacMapEntry(string inputLine, out AlmanacMapEntry result) {
        MatchCollection matches = Regex.Matches(inputLine, @"(\d+)");
        if (matches.Any()) {
            result = new AlmanacMapEntry(
                    long.Parse(matches[0].Value),
                    long.Parse(matches[1].Value),
                    long.Parse(matches[2].Value));
            return true;
        }
        result = new AlmanacMapEntry();
        return false;
    }
}

public readonly record struct AlmanacMap {
    private readonly AlmanacRanges _ranges;
    private string Name { get; }

    public AlmanacMap(string name, ICollection<AlmanacMapEntry> entries)
            : this(name, DeriveAlmanacRangesFromMapEntries(entries)) { }

    private AlmanacMap(string name, AlmanacRanges ranges) {
        Name = name;
        _ranges = ReduceIdenticalRanges(ranges);
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
            (other._ranges.Contains(rangeBStart)
                            ? new List<AlmanacRangeEntry>()
                            : new List<AlmanacRangeEntry> { other._ranges.Predecessor(rangeBStart) })
                    .Concat(other._ranges.RangeFromTo(rangeBStart, rangeBEnd + 1))
                    .ForEach(entry => {
                        long newKey = entry.Key != long.MinValue && rangeAStart < entry.Key - currTransform
                                ? entry.Key - currTransform
                                : rangeAStart;
                        newRanges[newKey] = currTransform + entry.Value;
                    });
        }
        return new AlmanacMap(
                name ?? $"{Name} -> {other.Name}",
                newRanges);
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

    private static AlmanacRanges ReduceIdenticalRanges(AlmanacRanges range) {
        var rangeReduced = new AlmanacRanges();
        AlmanacRangeEntry? last = null;
        foreach (AlmanacRangeEntry entry in range) {
            if (last != null && last.Value.Value == entry.Value) {
                continue;
            }
            rangeReduced[entry.Key] = entry.Value;
            last = entry;
        }
        return rangeReduced;
    }
}
