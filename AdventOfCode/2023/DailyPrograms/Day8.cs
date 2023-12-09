using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode._2023.Repos;
using Graph = System.Collections.Generic.Dictionary<string, kirypto.AdventOfCode._2023.DailyPrograms.GraphNode>;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public partial class Day8 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        List<string> inputLines = inputRepository.FetchLines(inputRef)
                .Where(inputLine => inputLine.Any())
                .ToList();
        string path = inputLines[0];
        Graph graphNodes = inputLines.Skip(1)
                .Select(inputLine => NodeMatcher().Matches(inputLine))
                .Select(nodeMatches => new GraphNode(nodeMatches[0].Value, nodeMatches[1].Value, nodeMatches[2].Value))
                .ToDictionary(node => node.Name);

        string resultNode = graphNodes.FollowPath("AAA", path);
        Console.WriteLine($"Started at AAA, followed {path}, ended at {resultNode}");

        throw new NotImplementedException();
    }

    [GeneratedRegex("([A-Z])+")]
    private static partial Regex NodeMatcher();
}

public readonly record struct GraphNode(string Name, string Left, string Right);

public static class GraphExtensions {
    private static string MoveLeft(this Graph graph, string fromNode) {
        return graph[fromNode].Left;
    }

    private static string MoveRight(this Graph graph, string fromNode) {
        return graph[fromNode].Right;
    }

    public static string FollowPath(this Graph graph, string fromNode, string path) {
        return path.Aggregate(fromNode, (currNode, moveDirection) => moveDirection switch {
                'L' => graph.MoveLeft(currNode),
                'R' => graph.MoveRight(currNode),
                _ => throw new NotImplementedException(),
        });
    }
}
