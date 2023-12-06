using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode._2023.Repos;
using static kirypto.AdventOfCode._2023.DailyPrograms.AlmanacMapper;

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
        IDictionary<string, IList<AlmanacMapper>> mappers = new Dictionary<string, IList<AlmanacMapper>>();
        var currentMapper = "";
        var currentEntryList = new List<AlmanacMapEntry>();
        foreach (string line in inputLines.Skip(1)) {
            var mapNameMatch = Regex.Match(line, @"^([\w-]+)(?=( map:$))");
            MatchCollection matches = Regex.Matches(line, @"(\d+)");
            if (mapNameMatch.Success) {
                currentMapper = mapNameMatch.Value;
                mappers[currentMapper] = new List<AlmanacMapper>();
                if (currentEntryList.Any()) {
                    new AlmanacMap(currentEntryList);
                }
                currentEntryList = new List<AlmanacMapEntry>();
            } else {
                mappers[currentMapper].Add(AlmanacMapperFrom(
                        matches.Select(match => long.Parse(match.Value)).ToList()
                ));
                currentEntryList.Add(new AlmanacMapEntry(
                        long.Parse(matches[0].Value),
                        long.Parse(matches[1].Value),
                        long.Parse(matches[2].Value)
                        ));
            }
        }
        var almanac = new Almamac(mappers);
        long minLocation = seeds.Select(seed => almanac.MapSeedToLocation(seed))
                .Min();
        Console.WriteLine($"Min location: {minLocation}");
    }

    private static IEnumerable<long> LongRange(long start, long count)
    {
        for (long i = 0; i < count; i++)
        {
            yield return start + i;
        }
    }
}

public record Almamac(IDictionary<string, IList<AlmanacMapper>> Mappers) {
    public long MapSeedToLocation(long seed) => new List<string> {
                    "seed-to-soil", "soil-to-fertilizer", "fertilizer-to-water", "water-to-light",
                    "light-to-temperature", "temperature-to-humidity", "humidity-to-location",
            }
            .Select(mapName => Mappers[mapName])
            .Aggregate(seed, (current, categoryMappers) => categoryMappers.Map(current));
}

public readonly record struct AlmanacMapper(MapperFunc Map, IsApplicableFunction IsApplicable) {
    public delegate long MapperFunc(long input);

    public delegate bool IsApplicableFunction(long input);

    public static AlmanacMapper Identity() {
        return new AlmanacMapper(input => input, _ => true);
    }

    public static AlmanacMapper AlmanacMapperFrom(IList<long> args) {
        long destinationRangeStart = args[0];
        long sourceRangeStart = args[1];
        long rangeLength = args[2];
        return new AlmanacMapper(
                input => input + (destinationRangeStart - sourceRangeStart),
                input => sourceRangeStart <= input && input < sourceRangeStart + rangeLength);
    }
}

public static class AlmanacMapperExtensions {
    public static long Map(this IList<AlmanacMapper> mappers, long input) {
        return mappers
                .Where(mapper => mapper.IsApplicable(input))
                .FirstOrDefault(Identity())
                .Map(input);
    }
}