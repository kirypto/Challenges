using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Collections.Extensions;
using kirypto.AdventOfCode.Common.Models;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;
using QuickGraph;
using QuickGraph.Algorithms.Search;
using static kirypto.AdventOfCode.Common.Models.CompassDirectionExtensions;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

[DailyProgram(10)]
public class Day10 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        char[,] inputMap = inputRepository.FetchAs2DCharArray();
        int rowCount = inputMap.GetLength(0);
        int colCount = inputMap.GetLength(1);
        Logger.LogInformation($"Rows = {rowCount}; Cols = {colCount}");
        AdjacencyGraph<Coord, Edge<Coord>> graph = new();
        graph.AddVertexRange(Enumerable
                .Range(0, rowCount)
                .SelectMany(row => Enumerable.Range(0, colCount).Select(col => new Coord(col, row))));

        HashSet<Coord> trailHeads = [];

        for (int row = 0; row < rowCount; row++) {
            for (int col = 0; col < colCount; col++) {
                int curr = inputMap[row, col];
                Coord currPos = new(col, row);
                if (curr == '0') {
                    trailHeads.Add(currPos);
                }
                foreach (CompassDirection direction in CardinalDirections) {
                    Coord nextPos = currPos.Move(direction);
                    if (inputMap.TryGetValue(nextPos.Y, nextPos.X, out char adjacent) && adjacent - curr == 1) {
                        graph.AddEdge(new Edge<Coord>(currPos, nextPos));
                        Logger.LogInformation($"Added edge from {currPos} to {nextPos}");
                    }
                }
            }
        }


        int sum = trailHeads.Select(trailHead => {
            int endCount = 0;
            DepthFirstSearchAlgorithm<Coord, Edge<Coord>> dfs = new(graph);
            dfs.DiscoverVertex += vertex => {
                if (inputMap[vertex.Y, vertex.X] == '9') {
                    endCount++;
                }
            };
            dfs.Compute(trailHead);
            return endCount;
        }).Sum();
        return sum.ToString();
    }
}
