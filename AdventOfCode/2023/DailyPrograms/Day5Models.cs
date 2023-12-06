
using C5;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public readonly record struct AlmanacMapEntry(long destinationRangeStart, long sourceRangeStart, long rangeLength);

public readonly record struct AlmanacMap {
    private readonly TreeDictionary<long, long> _ranges;

    public AlmanacMap(ICollection<AlmanacMapEntry> entries) {
        var ranges = new TreeDictionary<long, long> {
                [0] = 0,
        };
        foreach ((long destinationRangeStart, long sourceRangeStart, long rangeLength) in entries) {
            C5.KeyValuePair<long,long> newAfter = ranges.WeakPredecessor(sourceRangeStart + rangeLength);
        }

        _ranges = ranges;
    }
}