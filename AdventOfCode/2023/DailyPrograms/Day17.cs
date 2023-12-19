using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Models;
using kirypto.AdventOfCode._2023.Repos;
using static kirypto.AdventOfCode._2023.DailyPrograms.ExplorationVisit;
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
        int rowCount = cityMap.GetLength(0);
        int colCount = cityMap.GetLength(1);
        Path bestLavaFlow = FindBestLavaFlow(
                cityMap,
                new Position(0, 0),
                new Position(rowCount - 1, colCount - 1)
        );
        new char[rowCount, colCount]
                .ApplyMask((_, row, col) => bestLavaFlow.Contains(new Position(row, col)) ? '#' : '.')
                .PrintToConsole();

        int totalHeatLoss = bestLavaFlow
                .Skip(1) // Don't include starting position
                .Select(position => cityMap[position.Row, position.Col])
                .Sum();
        Console.WriteLine($"Minimal lava heat loss is {totalHeatLoss}");
    }

    private static Path FindBestLavaFlow(CityMap cityMap, Position start, Position goal) {
        double averageCost = Enumerable.Range(0, cityMap.GetLength(0))
                .SelectMany(cityMap.EnumerateRow)
                .Average();
        var queue = new ExplorationQueue {
                new(start, 0, start.EuclideanDistanceTo(goal)),
        };

        ISet<ExplorationVisit> visited = new HashSet<ExplorationVisit>();
        Path currentBestPath = new List<Position>();
        var currentBestCost = double.MaxValue;
        while (queue.Any()) {
            State current = queue.DeleteMin();
            if (current.CostSoFar >= currentBestCost) {
                Console.Write(".");
                continue;
            }
            ExplorationVisit visit = ExplorationVisitFrom(current);
            if (visited.Contains(visit)) {
                // Problem might be here, possibly throwing away a different path to a current place. Not sure why
                continue;
            }
            if (current.Position == goal) {
                currentBestPath = current.Path;
                currentBestCost = current.CostSoFar;
                Console.WriteLine($"Found a new minimum: {currentBestCost}");
            }

            visited.Add(visit);
            foreach (var successor in GetSuccessors(current, cityMap, goal, averageCost)) {
                if (successor.CostSoFar < currentBestCost) {
                    queue.Add(successor);
                } else {
                    Console.Write(".");
                }
            }
        }
        throw new InvalidOperationException("Should not be here");
    }

    private static IEnumerable<State> GetSuccessors(State current, CityMap cityMap, Position goal, double averageCost) {
        Position currentPosition = current.Position;
        Position? dissallowed = null;
        if (current.Parent != null && current.Parent.Parent != null && current.Parent.Parent.Parent != null) {
            var back1 = current.Parent.Position;
            var back2 = current.Parent.Parent.Position;
            var back3 = current.Parent.Parent.Parent.Position;
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
                .Where(pos => pos != current.Parent?.Position)
                .Where(pos => pos != dissallowed)
                .Select(pos => new State(
                        pos,
                        current.CostSoFar + cityMap[pos.Row, pos.Col],
                        pos.ManhattenDistanceTo(goal) * averageCost,
                        current));
    }
}

public record State(
        Position Position,
        float CostSoFar,
        double EstimateToGoal,
        State? Parent = null
) : IComparable<State> {
    public int CompareTo(State? other) => other == null ? -1 : Value.CompareTo(other.Value);

    public double Value => CostSoFar + EstimateToGoal;

    public Path Path => Parent == null
            ? new List<Position> { Position }
            : Parent.Path.Append(Position).ToList();
}

// Used only in hash set
[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
public readonly record struct ExplorationVisit(Position Current, Position? Back1, Position? Back2, Position? Back3) {
    public static ExplorationVisit ExplorationVisitFrom(State state) {
        return new ExplorationVisit(
                state.Position,
                state.Parent?.Position,
                state.Parent?.Parent?.Position,
                state.Parent?.Parent?.Parent?.Position);
    }
}
