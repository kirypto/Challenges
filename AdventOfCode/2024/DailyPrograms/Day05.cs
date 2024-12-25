using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Extensions;
using kirypto.AdventOfCode.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(5)]
public class Day05 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        string[] parts = inputRepository.Fetch().Split("\n\n");

        Logger.LogInformation("---\n{part1}\n---\n{part2}---", parts[0], parts[1]);

        Dictionary<int, ISet<int>> paths = [];
        parts[0].Split("\n")
            .Select(line => line
                .Split("|")
                .Select(int.Parse)
                .ToList())
            .ForEach(pages => {
            // if (pages.Contains())
            // Need to split the pages and put it in the dictionary page paths
        });
        throw new NotImplementedException();
    }
}
