using System.Text.RegularExpressions;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day2 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef) {
        Console.Write("Input: ");
        // string inputRef = "day2-" + Console.ReadLine()!;
        inputRepository.FetchLines(inputRef)
                .Select(line => line.Replace(";", ", ;") + ";")
                .Select(line => {
                    Console.WriteLine(line);
                    return line;
                })
                .Select(game => Regex.Match(game, @"^Game (\d+): ((\d+ \w+, )+;)+"))
                .ForEach(match => { Console.WriteLine(match.Groups.Count); });
    }
}