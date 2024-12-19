using System;
using System.Collections.Generic;
using kirypto.AdventOfCode.Common.Extensions;
using kirypto.AdventOfCode.Common.Models;

namespace kirypto.AdventOfCode.Common.Algorithms.Search;

public class GridAStar<T>(T[,] grid, Func<T, bool> isWalkable) {
    public (IList<(Coord coord, T item)> path, int cost) FindPath(Coord startCoord, Coord endCoord) {
        C5.IntervalHeap<(int cost, Coord coord)> openSet = [];
        Dictionary<Coord, Coord> cameFrom = new();

        Dictionary<Coord, int> gCosts = new() { [startCoord] = 0 };

        openSet.Add((startCoord.ManhattanDistanceTo(endCoord), startCoord));

        while (openSet.Count > 0) {
            (int _, Coord currentCoord) = openSet.DeleteMin();

            if (currentCoord == endCoord) {
                return (ReconstructPath(cameFrom, currentCoord), gCosts[currentCoord]);
            }

            foreach (CardinalDirection direction in Enum.GetValues<CardinalDirection>()) {
                Coord neighbor = currentCoord.Move(direction);

                if (!grid.TryGetValue(neighbor.Y, neighbor.X, out T cell) || !isWalkable(cell)) {
                    continue;
                }

                int tentativeGCost = gCosts[currentCoord] + 1;

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
