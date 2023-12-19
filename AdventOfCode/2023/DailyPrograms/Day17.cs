using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Models;
using kirypto.AdventOfCode._2023.Repos;
using ExplorationQueue = C5.IntervalHeap<kirypto.AdventOfCode._2023.DailyPrograms.State>;
using Path = System.Collections.Generic.IList<kirypto.AdventOfCode._2023.Models.Position>;
using CityMap = int[,];

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day17 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        CityMap cityMap = inputRepository.Fetch(inputRef)
                .Trim()
                .To2DCharArray("\n")
                .ApplyMask((chars, row, col) => chars[row, col] - '0');
        Path bestLavaFlow = FindBestLavaFlow(
                cityMap,
                new Position(0, 0),
                new Position(cityMap.GetLength(0), cityMap.GetLength(1))
        );
        Console.WriteLine(bestLavaFlow.Count);
    }

    private static Path FindBestLavaFlow(CityMap cityMap, Position start, Position goal) {
        var queue = new ExplorationQueue {
                new(new List<Position> {start}, 0, start.ManhattenDistanceTo(goal)),
        };

        while (queue.Any()) {
            State current = queue.DeleteMin();
            Console.WriteLine($"Exploring {current.Path[^1]}: {current.CostSoFar} : {current.EstimateToGoal}");
            Thread.Sleep(1000);
            if (current.Path[^1] == goal) {
                return current.Path;
            }

            queue.AddAll(GetSuccessors(current, cityMap, goal));
        }
        throw new InvalidOperationException("Should not be here");
    }

    private static IEnumerable<State> GetSuccessors(State current, CityMap cityMap, Position goal) {
        Position currentPosition = current.Path[^1];
        Position? dissallowed = null;
        if (current.Path.Count >= 4) {
            var back3 = current.Path[^4];
            var back2 = current.Path[^3];
            var back1 = current.Path[^2];
            int lastDiffRow = currentPosition.Row - back1.Row;
            int lastDiffCol = currentPosition.Col - back1.Col;
            if (lastDiffRow == back1.Row - back2.Row && lastDiffRow == back2.Row - back3.Row
                && lastDiffCol == back1.Col - back2.Col && lastDiffCol == back2.Col - back3.Col) {
                dissallowed = new Position(currentPosition.Row + lastDiffRow, currentPosition.Col + lastDiffCol);
            }
        }
        return currentPosition.GetAdjacentPositions()
                .Where(pos => pos.Row >= 0)
                .Where(pos => pos.Col >= 0)
                .Where(pos => pos.Row < cityMap.GetLength(0))
                .Where(pos => pos.Col < cityMap.GetLength(1))
                .Where(pos => pos != dissallowed)
                .Select(pos => new State(
                        current.Path.Append(pos).ToList(),
                        current.CostSoFar + cityMap[pos.Row, pos.Col],
                        pos.ManhattenDistanceTo(goal)));
    }
}

public readonly record struct State(Path Path, float CostSoFar, float EstimateToGoal) : IComparable<State> {
    public int CompareTo(State other) {
        return (CostSoFar + EstimateToGoal).CompareTo(other.CostSoFar + other.EstimateToGoal);
    }
}
