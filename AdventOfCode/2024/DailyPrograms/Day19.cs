using System.Text.RegularExpressions;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;
using static System.StringSplitOptions;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(19)]
public class Day19 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        string[] availableAndRequests = inputRepository.Fetch()
                .Split("\n\n");
        string[] availableTowels = availableAndRequests[0]
                .Split(",", TrimEntries);
        Logger.LogInformation("Available Towels: {towels}", string.Join(",", availableTowels));
        string pattern = $"({string.Join('|', availableTowels)})+";
        Logger.LogInformation("Pattern: {pattern}", pattern);
        Regex regex = new("^" + pattern + "$", RegexOptions.Compiled);
        foreach (string request in availableAndRequests[1].Split("\n")) {
            Logger.LogInformation("Request: '{request}': {success}", request, regex.Match(request).Success);
        }


        throw new System.NotImplementedException();
    }
}
