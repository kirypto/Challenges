using System;
using System.Linq;
using C5;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public readonly record struct AlmanacMapEntry(long destinationRangeStart, long sourceRangeStart, long rangeLength);

public readonly record struct AlmanacMap {
    private readonly TreeDictionary<long, long> _ranges;
    public string Name { get; }

    public AlmanacMap(string name, System.Collections.Generic.ICollection<AlmanacMapEntry> entries) {
        Name = name;
        var ranges = new TreeDictionary<long, long> {
                [0] = 0,
        };
        foreach ((long destinationRangeStart, long sourceRangeStart, long rangeLength) in entries
                         .OrderBy(e => e.sourceRangeStart)) {
            KeyValuePair<long,long> newAfter = ranges.WeakPredecessor(sourceRangeStart + rangeLength);
            KeyValuePair<long,long> existingBefore = ranges.WeakPredecessor(sourceRangeStart);
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
        foreach (KeyValuePair<long, long> keyValuePair in _ranges) {
            Console.WriteLine($"{keyValuePair.Key} -> {keyValuePair.Value}");
        }
    }
}