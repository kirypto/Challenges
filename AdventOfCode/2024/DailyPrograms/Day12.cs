using System;
using System.Collections.Generic;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Extensions;
using kirypto.AdventOfCode.Common.Interfaces;
using Microsoft.Extensions.Logging;
using static System.Enum;
using static System.Linq.Enumerable;
using static kirypto.AdventOfCode.Common.Services.IO.DailyProgramLogger;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

[DailyProgram(12)]
public class Day12 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        char[,] garden = inputRepository.FetchAs2DCharArray();
        int rowCount = garden.GetLength(0);
        int colCount = garden.GetLength(1);
        HashSet<Coord> visited = [];

        int fenceCost = 0;
        IEnumerable<Coord> coordEnumerable = Range(0, rowCount)
                .SelectMany(row => Range(0, colCount).Select(col => new Coord(col, row)));

        foreach (Coord currPos in coordEnumerable) {
            if (!visited.Add(currPos)) {
                continue;
            }
            char plotType = garden[currPos.Y, currPos.X];
            int plotArea = 0;
            int plotPerimeter = 0;
            int plotSides = 0;
            bool[,] batchFences = new bool[rowCount + 1, colCount + 1];

            HashSet<Coord> currentBatch = [currPos];
            while (currentBatch.Count > 0) {
                Coord focus = currentBatch.First();
                currentBatch.Remove(focus);
                visited.Add(focus);
                plotArea++;
                Logger.LogInformation($"Focus: {focus}");

                foreach (Coord adjacentCoord in GetValues<CardinalDirection>().Select(d => focus.Move(d))) {
                    if (!garden.TryGetValue(adjacentCoord.Y, adjacentCoord.X, out char adjPlotType)
                        || adjPlotType != plotType) {
                        plotPerimeter++;
                        Logger.LogInformation(
                                $" --> Adjacent {adjacentCoord} is other, increment perim to {plotPerimeter}");

                        (FenceType fenceType, Coord fenceCoord) = GetFence(focus, adjacentCoord);
                        batchFences[fenceCoord.Y, fenceCoord.X] = true;
                        Logger.LogInformation($"    --> {fenceType} @ {fenceCoord}");
                        (Coord prevFence, Coord nextFence) = GetAdjacentFences(fenceCoord, fenceType);
                        if ((batchFences.TryGetValue(prevFence.Y, prevFence.X, out bool hasPreviousAdjacentFence)
                                    && hasPreviousAdjacentFence)
                            || (batchFences.TryGetValue(nextFence.Y, nextFence.X, out bool hasNextAdjacentFence)
                                    && hasNextAdjacentFence)) {
                            Logger.LogInformation("    --> Already counted this fence side, skipping.");
                        } else {
                            plotSides++;
                            Logger.LogInformation($"    --> New fence side seen, incremented count to {plotSides}.");
                        }
                    } else if (!visited.Contains(adjacentCoord)) {
                        currentBatch.Add(adjacentCoord);
                    }
                }
            }
            int currentFenceCost = part == 1 ? plotArea * plotPerimeter : plotArea * plotSides;
            fenceCost += currentFenceCost;
            Logger.LogInformation($"!! Plot {plotType}: {plotArea}, {plotPerimeter}, {plotSides}, {currentFenceCost}");
        }
        return fenceCost.ToString();
    }

    private static (FenceType fenceType, Coord fenceCoord) GetFence(Coord plotCoord1, Coord plotCoord2) =>
            plotCoord1.Y == plotCoord2.Y
                    ? (FenceType.NorthSouth, plotCoord1 with { X = Math.Max(plotCoord1.X, plotCoord2.X) })
                    : (FenceType.EastWest, plotCoord1 with { Y = Math.Max(plotCoord1.Y, plotCoord2.Y) });

    private static (Coord prev, Coord next) GetAdjacentFences(Coord fenceCoord, FenceType fenceType) =>
            fenceType switch {
                    FenceType.NorthSouth => (
                            fenceCoord with { Y = fenceCoord.Y - 1 },
                            fenceCoord with { Y = fenceCoord.Y + 1 }),
                    FenceType.EastWest => (
                            fenceCoord with { X = fenceCoord.X - 1 },
                            fenceCoord with { X = fenceCoord.X + 1 }),
                    _ => throw new ArgumentOutOfRangeException(nameof(fenceType), fenceType, null)
            };

    private enum FenceType {
        NorthSouth,
        EastWest,
    }
}
