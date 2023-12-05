using System.Text.RegularExpressions;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;
using static kirypto.AdventOfCode._2023.DailyPrograms.Almamac;
using static kirypto.AdventOfCode._2023.DailyPrograms.AlmanacMapper;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day5 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        IList<string> inputLines = inputRepository
                .FetchLines(inputRef)
                .Where(s => s.Any())
                .ToList();
        ISet<int> seeds = Regex.Matches(inputLines[0], @"(\d+)")
                .Select(match => int.Parse(match.Value))
                .ToHashSet();
        Console.WriteLine("Seeds:");
        seeds.ForEach(Console.WriteLine);
        IList<IList<AlmanacMapper>> mappers = Enumerable.Range(0, 7)
                .Select(_ => new List<AlmanacMapper>())
                .Cast<IList<AlmanacMapper>>()
                .ToList();
        var mapperIndex = 0;
        foreach (string line in inputLines.Skip(2)) {
            MatchCollection matches = Regex.Matches(line, @"(\d+)");
            if (matches.Count == 0) {
                mappers[mapperIndex].Add(Identity());
                mapperIndex++;
            } else {
                mappers[mapperIndex].Add(AlmanacMapperFrom(
                        matches.Select(match => int.Parse(match.Value)).ToList()
                ));
            }
        }
        var almanac = new Almamac(seeds, mappers);
        throw new NotImplementedException();
    }
}

public readonly record struct Almamac(ISet<int> Seeds, IList<IList<AlmanacMapper>> Mappers);

public readonly record struct AlmanacMapper(MapperFunc Mapper, IsApplicableFunction IsApplicable) {
    public delegate int MapperFunc(int input);

    public delegate bool IsApplicableFunction(int input);

    public static AlmanacMapper Identity() {
        return new AlmanacMapper(input => input, input => true);
    }

    public static AlmanacMapper AlmanacMapperFrom(IList<int> args) {
        int destinationRangeStart = args[0];
        int sourceRangeStart = args[1];
        int rangeLength = args[2];
        return new AlmanacMapper(
                input => input + (destinationRangeStart - sourceRangeStart),
                input => sourceRangeStart <= input && input < sourceRangeStart + rangeLength);
    }
}