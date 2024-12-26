using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Collections.Extensions;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;
using QuickGraph;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(23)]
public class Day23 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        AdjacencyGraph<string, Edge<string>> gameNetwork = new();
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
        ICollection<Tuple<string,string,string>> triangles = gameNetwork.FindTriangles();

        throw new NotImplementedException();
    }
}

public static class GraphExtensions {
    public static ICollection<Tuple<T, T, T>> FindTriangles<T>(this AdjacencyGraph<T, Edge<T>> graph) {
        Logger.LogInformation("Is directed graph: {directed}", graph.IsDirected);
        foreach (Edge<T> edge in graph.Edges) {
            Logger.LogInformation("Examining edge '{edge}'", edge);
            T source = edge.Source;
            T target = edge.Target;
            if (graph.TryGetOutEdges(source, out IEnumerable<Edge<T>> outEdges)) {
                Logger.LogInformation("Found out-edges: {outEdges}", outEdges.CommaDelimited());
            }
        }
        return [];
    }
}
