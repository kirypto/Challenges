using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Extensions;
using kirypto.AdventOfCode.Common.Interfaces;
using Microsoft.Extensions.Logging;
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

        foreach (Coord currPos in Range(0, rowCount)
                         .SelectMany(row => Range(0, colCount)
                                 .Select(col => new Coord(col, row)))
                ) {
            if (!visited.Add(currPos)) {
                continue;
            }
            char plotType = garden[currPos.Y, currPos.X];
            int plotArea = 0;
            int plotPerimeter = 0;

            HashSet<Coord> currentBatch = [currPos];
            while (currentBatch.Count > 0) {
                Coord focus = currentBatch.First();
                currentBatch.Remove(focus);
                Logger.LogInformation($"Focus: {focus}");
                if (!garden.TryGetValue(focus.Y, focus.X, out char focusType)) {
                    plotPerimeter++;
                    Logger.LogInformation($"Out of bounds, '{plotType}' is now {plotArea} x {plotPerimeter}");
                } else if (focusType == plotType) {
                    visited.Add(focus);
                    plotArea++;
                    Logger.LogInformation($"Growing plot, '{plotType}' is now {plotArea} x {plotPerimeter}");
                    foreach (Coord adjacentCoord in Enum.GetValues<CardinalDirection>()
                                     .Select(direction => focus.Move(direction))) {
                        if (!visited.Contains(adjacentCoord)) {
                            Logger.LogInformation($"  --> Adding new coord to batch {adjacentCoord}");
                            currentBatch.Add(adjacentCoord);
                        } else if (garden.TryGetValue(adjacentCoord.Y, adjacentCoord.X, out char adjPlotType)
                                   && adjPlotType != plotType) {
                            plotPerimeter++;
                            Logger.LogInformation($"  --> Adjacent plot {adjacentCoord} is different type, perim now {plotPerimeter}");
                        }
                    }
                } else {
                    plotPerimeter++;
                    Logger.LogInformation($"Found different plot type, '{plotType}' is now {plotArea} x {plotPerimeter}.");
                }
            }
            fenceCost += plotArea * plotPerimeter;
            Logger.LogInformation($"!! Plot {plotType}: {plotArea}, {plotPerimeter}, {plotArea * plotPerimeter}");
        }
        return fenceCost.ToString();
    }
}
