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
        int possibleDesignCount = 0;
        string[] requests = availableAndRequests[1].Split("\n", RemoveEmptyEntries | TrimEntries);
        Logger.LogInformation("Request count: {count}", requests.Length);
        for (int index = 0; index < requests.Length; index++) {
            Logger.LogInformation("Checked {current} of {total}, found {count} so far", index, requests.Length, possibleDesignCount);
            string request = requests[index];
            Match designMatch = regex.Match(request);
            // Logger.LogInformation("Request: '{request}': {success}", request, designMatch.Success);
            if (designMatch.Success) {
                possibleDesignCount++;
            }
        }
        return possibleDesignCount.ToString();
    }
}
