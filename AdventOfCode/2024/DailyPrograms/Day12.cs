using System;
using System.Collections.Generic;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Collections;
using kirypto.AdventOfCode.Common.Extensions;
using kirypto.AdventOfCode.Common.Interfaces;
using kirypto.AdventOfCode.Common.Models;
using Microsoft.Extensions.Logging;
using static System.ConsoleColor;
using static System.Enum;
using static System.Linq.Enumerable;

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
            Dictionary<FenceType, bool[,]> fences = new() {
                    { FenceType.NorthSouth, new bool[rowCount, colCount + 1] },
                    { FenceType.EastWest, new bool[rowCount + 1, colCount] },
            };

            QueueSet<Coord> currentBatch = [currPos];
            while (currentBatch.Count > 0) {
                currentBatch.RemoveFirst(out Coord focus);
                visited.Add(focus);
                plotArea++;
                Logger.LogInformation("Focus: {focus}", focus);

                foreach (Coord adjacentCoord in GetValues<CardinalDirection>().Select(d => focus.Move(d))) {
                    if (!garden.TryGetValue(adjacentCoord.Y, adjacentCoord.X, out char adjPlotType)
                        || adjPlotType != plotType) {
                        plotPerimeter++;
                        Logger.LogInformation(" --> Adjacent {adjacentCoord} is other, increment perim to {plotPerimeter}",
                                adjacentCoord, plotPerimeter);

                        (FenceType fenceType, Coord fenceCoord) = GetFence(focus, adjacentCoord);
                        fences[fenceType][fenceCoord.Y, fenceCoord.X] = true;
                        Logger.LogInformation("    --> {fenceType} @ {fenceCoord}", fenceType, fenceCoord);
                        (Coord prevFence, Coord nextFence) = GetAdjacent(fenceCoord, fenceType);
                        (Coord prevPlot, Coord nextPlot) = GetAdjacent(focus, fenceType);
                        bool isPrevFenceSameSide = fences[fenceType].TryGetValue(prevFence.Y, prevFence.X, out bool hasPrevFence)
                                && hasPrevFence
                                && garden.TryGetValue(prevPlot.Y, prevPlot.X, out char prevPlotType)
                                && prevPlotType == plotType;
                        bool isNextFenceSameSide = fences[fenceType].TryGetValue(nextFence.Y, nextFence.X, out bool hasNextFence)
                                && hasNextFence
                                && garden.TryGetValue(nextPlot.Y, nextPlot.X, out char nextPlotType)
                                && nextPlotType == plotType;
                        if (isPrevFenceSameSide && isNextFenceSameSide) {
                            plotSides--;
                            Logger.LogInformation("    --> Counted previous and next, merging (decrementing count to {plotSides}.", plotSides);
                        } else if (isPrevFenceSameSide || isNextFenceSameSide) {
                            Logger.LogInformation("    --> Already counted this fence side, skipping.");
                        } else {
                            plotSides++;
                            Logger.LogInformation("    --> New fence side seen, incremented count to {plotSides}.", plotSides);
                        }
                    } else if (!visited.Contains(adjacentCoord)) {
                        currentBatch.Add(adjacentCoord);
                    }
                }
                Print(garden, fences, focus, currentBatch);
            }
            int currentFenceCost = part == 1 ? plotArea * plotPerimeter : plotArea * plotSides;
            fenceCost += currentFenceCost;
            Logger.LogInformation("!! Plot {plotType}: {plotArea}, {plotPerimeter}, {plotSides}, {currentFenceCost}",
                    plotType, plotArea, plotPerimeter, plotSides, currentFenceCost);
            if (Logger.IsEnabled(LogLevel.Debug)) {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
            }
        }
        return fenceCost.ToString();
    }

    private static void Print(char[,] plots, Dictionary<FenceType, bool[,]> fences, Coord focus, ISet<Coord> batch) {
        if (!Logger.IsEnabled(LogLevel.Debug)) {
            return;
        }
        int plotRows = plots.GetLength(0);
        int plotCols = plots.GetLength(1);
        try {
            for (int row = 0; row < plotRows; row++) {
                for (int col = 0; col < fences[FenceType.EastWest].GetLength(1); col++) {
                    Console.Write(" " + (fences[FenceType.EastWest][row, col] ? "-" : " "));
                }
                Console.Write("\n");
                for (int col = 0; col < plotCols; col++) {
                    Console.Write(fences[FenceType.NorthSouth][row, col] ? "|" : " ");

                    if (row == focus.Y && col == focus.X) {
                        Console.ForegroundColor = Blue;
                    } else if (batch.Contains(new Coord(col, row))) {
                        Console.ForegroundColor = Green;
                    }
                    Console.Write($"{plots[row, col]}");
                    Console.ResetColor();
                }
                Console.Write(fences[FenceType.NorthSouth][row, fences[FenceType.NorthSouth].GetLength(1) - 1] ? "|" : " ");
                Console.Write("\n");
            }
            for (int col = 0; col < fences[FenceType.EastWest].GetLength(1); col++) {
                Console.Write(" " + (fences[FenceType.EastWest][fences[FenceType.EastWest].GetLength(0) - 1, col] ? "-" : " "));
            }
        } catch (Exception) {
            Console.Out.Flush();
            throw;
        }
        Console.WriteLine();
        Console.Out.Flush();
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);
    }

    private static (FenceType fenceType, Coord fenceCoord) GetFence(Coord plotCoord1, Coord plotCoord2) =>
            plotCoord1.Y == plotCoord2.Y
                    ? (FenceType.NorthSouth, plotCoord1 with { X = Math.Max(plotCoord1.X, plotCoord2.X) })
                    : (FenceType.EastWest, plotCoord1 with { Y = Math.Max(plotCoord1.Y, plotCoord2.Y) });

    private static (Coord prev, Coord next) GetAdjacent(Coord coord, FenceType direction) =>
            direction switch {
                    FenceType.NorthSouth => (
                            coord with { Y = coord.Y - 1 },
                            coord with { Y = coord.Y + 1 }),
                    FenceType.EastWest => (
                            coord with { X = coord.X - 1 },
                            coord with { X = coord.X + 1 }),
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };

    private enum FenceType {
        NorthSouth,
        EastWest,
    }
}
