using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Extensions;
using kirypto.AdventOfCode.Common.Interfaces;
using Microsoft.Extensions.Logging;
using QuickGraph;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(23)]
public class Day23 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        BidirectionalGraph<string, IEdge<string>> gameNetwork = new();
        HashSet<string> uniqueComputers = [];
        inputRepository
                .FetchLines()
                .Select(link => link.Split('-'))
                .Tap(computers => {
                    gameNetwork.AddVertex(computers[0]);
                    gameNetwork.AddVertex(computers[1]);
                    uniqueComputers.Add(computers[0]);
                    uniqueComputers.Add(computers[1]);
                })
                .Select(computers => new Edge<string>(computers[0], computers[1]))
                .ForEach(edge => gameNetwork.AddEdge(edge));
        foreach (string computer in uniqueComputers.Where(computer => computer.Contains('t'))) {
            Logger.LogInformation("Checking computer '{computer}'", computer);
        }

        throw new NotImplementedException();
    }
}
