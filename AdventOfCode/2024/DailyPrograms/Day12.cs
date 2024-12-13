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
                visited.Add(focus);
                plotArea++;
                Logger.LogInformation($"Focus: {focus}");

                foreach (Coord adjacentCoord in GetValues<CardinalDirection>().Select(d => focus.Move(d))) {
                    if (!garden.TryGetValue(adjacentCoord.Y, adjacentCoord.X, out char adjPlotType)
                        || adjPlotType != plotType) {
                        plotPerimeter++;
                        Logger.LogInformation($" --> Adjacent is other, increment perim to {plotPerimeter}");
                    } else if (!visited.Contains(adjacentCoord)) {
                        currentBatch.Add(adjacentCoord);
                    }
                }
            }
            fenceCost += plotArea * plotPerimeter;
            Logger.LogInformation($"!! Plot {plotType}: {plotArea}, {plotPerimeter}, {plotArea * plotPerimeter}");
        }
        return fenceCost.ToString();
    }
}
