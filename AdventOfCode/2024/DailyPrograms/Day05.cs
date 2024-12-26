using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Collections.Extensions;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;
using QuickGraph;
using QuickGraph.Algorithms;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(5)]
public class Day05 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        string[] parts = inputRepository.Fetch().Split("\n\n");

        Logger.LogInformation("---\n{part1}\n---\n{part2}---", parts[0], parts[1]);

        AdjacencyGraph<int, SEdge<int>> pageConstraints = new();
        parts[0].Split("\n")
            .Select(line => line
                .Split("|")
                .Select(int.Parse)
                .ToList())
            .ForEach(pages => {
                pageConstraints.AddVertex(pages[0]);
                pageConstraints.AddVertex(pages[1]);
                pageConstraints.AddEdge(new SEdge<int>(pages[0], pages[1]));
            });

        Logger.LogInformation("Pages");
        foreach (int page in pageConstraints.TopologicalSort()) {
            Logger.LogInformation("Page {page}", page);
        }
        throw new NotImplementedException();
    }
}
