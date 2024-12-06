using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode.Common.Interfaces;
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

        if (part == 1) {
            int steps = graphNodes.FindAlongPath("AAA", "ZZZ", path);
            Console.WriteLine($"Started at AAA, followed {path}, found ZZZ after {steps}");
        } else {
            HashSet<string> startingNodes = graphNodes.Keys
                    .Where(node => node.EndsWith("A"))
                    .ToHashSet();
            int steps = graphNodes.FindAlongPathInParallel(startingNodes, path, node => node.EndsWith("Z"));
            Console.WriteLine($"Found that all nodes end with Z after {steps} steps.");
        }
    }

    [GeneratedRegex("([0-9A-Z])+")]
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

    private static string Move(this Graph graph, string fromNode, char moveDirection) {
        return moveDirection switch {
                'L' => graph.MoveLeft(fromNode),
                'R' => graph.MoveRight(fromNode),
                _ => throw new NotImplementedException($"Got invalid movement {moveDirection}"),
        };
    }

    public static string FollowPath(this Graph graph, string fromNode, string path) {
        return path.Aggregate(fromNode, graph.Move);
    }

    public static int FindAlongPath(this Graph graph, string fromNode, string targetNode, string path) {
        var steps = 0;
        string currentNode = fromNode;
        using IEnumerator<char> pathMovements = path.InfinitelyRepeat();
        while (currentNode != targetNode) {
            pathMovements.MoveNext();
            currentNode = graph.Move(currentNode, pathMovements.Current);
            steps++;
        }
        return steps;
    }

    public static int FindAlongPathInParallel(
            this Graph graph, ISet<string> fromNodes, string path, Func<string, bool> isGoal
    ) {
        var steps = 0;
        ISet<string> currentNodes = fromNodes;
        using IEnumerator<char> pathMovements = path.InfinitelyRepeat();
        while (!currentNodes.All(isGoal)) {
            // if (steps % 100 == 0) {
                // Console.Write(".");
            // }
            pathMovements.MoveNext();
            currentNodes = currentNodes
                    .Select(node => graph.Move(node, pathMovements.Current))
                    .ToHashSet();
            steps++;
        }
        return steps;
    }
}
