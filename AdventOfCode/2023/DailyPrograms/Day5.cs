using System.Text.RegularExpressions;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;
using static kirypto.AdventOfCode._2023.DailyPrograms.AlmanacMapper;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day5 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        IList<string> inputLines = inputRepository
                .FetchLines(inputRef)
                .Where(s => s.Any())
                .ToList();
        ISet<long> seeds = Regex.Matches(inputLines[0], @"(\d+)")
                .Select(match => long.Parse(match.Value))
                .ToHashSet();
        IDictionary<string, IList<AlmanacMapper>> mappers = new Dictionary<string, IList<AlmanacMapper>>();
        var currentMapper = "";
        foreach (string line in inputLines.Skip(1)) {
            var mapNameMatch = Regex.Match(line, @"^([\w-]+)(?=( map:$))");
            MatchCollection matches = Regex.Matches(line, @"(\d+)");
            if (mapNameMatch.Success) {
                currentMapper = mapNameMatch.Value;
                mappers[currentMapper] = new List<AlmanacMapper>();
            } else {
                mappers[currentMapper].Add(AlmanacMapperFrom(
                        matches.Select(match => long.Parse(match.Value)).ToList()
                ));
            }
        }
        var almanac = new Almamac(mappers);
        long minLocation = seeds.Select(seed => almanac.MapSeedToLocation(seed))
                .Min();
        Console.WriteLine($"Min location: {minLocation}");
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