using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.Collections;
using kirypto.AdventOfCode.Common.Collections.Extensions;
using kirypto.AdventOfCode.Common.Models;
using static kirypto.AdventOfCode.Common.Models.CompassDirectionExtensions;

namespace kirypto.AdventOfCode.Common.Algorithms.Grids;

public class GridSearch<T>(T[,] grid, Func<T, bool> isWalkableFunc) {
    public (IList<(Coord coord, T item)> path, int cost) FindShortestPath(Coord startCoord, Coord endCoord)
        => AStarPath(startCoord, endCoord);

    public (IList<(Coord coord, T item)> path, int cost) AStarPath(
            Coord startCoord,
            Coord endCoord,
            Func<CostFunctionData, int> costFunc = null
    ) {
        costFunc ??= _ => 1;

        C5.IntervalHeap<(int cost, Coord coord)> openSet = [];
        Dictionary<Coord, Coord> cameFrom = new();

        Dictionary<Coord, int> gCosts = new() { [startCoord] = 0 };

        openSet.Add((startCoord.ManhattanDistanceTo(endCoord), startCoord));

        while (openSet.Count > 0) {
            (int _, Coord currentCoord) = openSet.DeleteMin();

            if (currentCoord == endCoord) {
                return (ReconstructPath(cameFrom, currentCoord), gCosts[currentCoord]);
            }

            foreach (Coord neighbor in NeighbouringCoords(currentCoord)) {
                if (!grid.TryGetValue(neighbor.Y, neighbor.X, out T cell) || !isWalkableFunc(cell)) {
                    continue;
                }

                int tentativeGCost = gCosts[currentCoord] + costFunc(new CostFunctionData {
                        Prev = cameFrom.TryGetValue(currentCoord, out Coord prevCoord) ? prevCoord : null,
                        Curr = currentCoord,
                        Next = neighbor,
                });

                if (!gCosts.TryGetValue(neighbor, out int value) || tentativeGCost < value) {
                    cameFrom[neighbor] = currentCoord;
                    value = tentativeGCost;
                    gCosts[neighbor] = value;

                    int fCost = tentativeGCost + neighbor.ManhattanDistanceTo(endCoord);
                    openSet.Add((fCost, neighbor));
                }
            }
        }

        return (new List<(Coord, T)>(), -1);
    }

    public ISet<Coord> FloodFill(Coord startCoord) {
        HashSet<Coord> filled = [];
        QueueSet<Coord> toVisit = [];

        if (IsWalkable(startCoord)) {
            toVisit.Add(startCoord);
        }

        while (toVisit.RemoveFirst(out Coord current)) {
            filled.Add(current);
            NeighbouringCoords(current)
                    .Where(IsWalkable)
                    .Where(neighbour => !filled.Contains(neighbour))
                    .ForEach(neighbour => toVisit.Add(neighbour));
        }

        return filled;
    }

    private bool IsWalkable(Coord coord) => grid.TryGetValue(coord.Y, coord.X, out T cellValue)
            && isWalkableFunc(cellValue);

    private static IEnumerable<Coord> NeighbouringCoords(Coord currentCoord) => CardinalDirections
            .Select(direction => currentCoord.Move(direction));

    private List<(Coord coord, T item)> ReconstructPath(Dictionary<Coord, Coord> cameFrom, Coord current) {
        List<(Coord coord, T item)> path = [];

        while (cameFrom.ContainsKey(current)) {
            path.Add((current, grid[current.Y, current.X]));
            current = cameFrom[current];
        }

        path.Reverse();
        return path;
    }
}

public readonly record struct CostFunctionData(Coord? Prev, Coord Curr, Coord Next);
