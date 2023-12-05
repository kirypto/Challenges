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
        ISet<int> seeds = Regex.Matches(inputLines[0], @"(\d+)")
                .Select(match => int.Parse(match.Value))
                .ToHashSet();
        Console.WriteLine("Seeds:");
        seeds.ForEach(Console.WriteLine);
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
                        matches.Select(match => int.Parse(match.Value)).ToList()
                ));
            }
        }
        var almanac = new Almamac(mappers);
        throw new NotImplementedException();
    }
}

public readonly record struct Almamac(IDictionary<string, IList<AlmanacMapper>> Mappers) {
}

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