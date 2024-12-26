using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(2)]
public class Day02 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        List<List<int>> lines = inputRepository.FetchLines()
                .Select(line => line.Split(" ")
                        .Select(numericChar => int.Parse(numericChar.ToString()))
                        .ToList())
                .ToList();

        foreach (List<int> line in lines) {
            bool increasing = line[0] < line[1];
            Logger.LogInformation(increasing ? "increasing" : "decreasing");
            bool isSafe = true;
            for (int i = 0; i < line.Count - 1; i++) {
                int curr = line[i];
                int next = line[i + 1];
                int diff = next - curr;
                if (increasing && diff is < 1 or > 2) {
                    isSafe = false;
                    break;
                }
                if (!increasing && diff is > -1 or < -2) {
                    isSafe = false;
                    break;
                }
            }
            Logger.LogInformation("Is safe: {isSafe}", isSafe);
        }


        throw new NotImplementedException();
    }
}
