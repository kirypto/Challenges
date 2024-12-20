using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.Algorithms.Search;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Extensions;
using kirypto.AdventOfCode.Common.Interfaces;
using kirypto.AdventOfCode.Common.Models;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;
using static System.ConsoleColor;
using static kirypto.AdventOfCode.Common.Services.IO.DailyProgramLogger;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

[DailyProgram(18)]
public class Day18 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        int colCount = 0;
        int rowCount = 0;
        List<Coord> coords = inputRepository.FetchRegexParsedLines<int, int>(@"(\d+),(\d+)")
                .Select(coordPair => new Coord(coordPair.Item1, coordPair.Item2))
                .Tap(coord => {
                    rowCount = Math.Max(rowCount, coord.Y + 1);
                    colCount = Math.Max(colCount, coord.X + 1);
                })
                .ToList();
        bool[,] map = new bool[colCount, rowCount];

        int part1Limit = inputRepository is RestInputRepository && colCount == 7 && rowCount == 7
                ? 12 // This is the example input
                : 1024;

        coords.Take(part1Limit).ForEach(coord => map[coord.Y, coord.X] = true);

        Logger.LogInformation($"Map ({rowCount}, {colCount}):");
        PrintMap(map, []);

        GridAStar<bool> search = new(map, isWalkable: b => !b);
        Coord startCoord = new(0, 0);
        Coord endCoord = new(colCount - 1, rowCount - 1);
        (IList<(Coord coord, bool item)> path, int cost) = search.FindPath(
                startCoord: startCoord,
                endCoord: endCoord);

        Logger.LogInformation(cost == -1 ? "No path found" : "Found path:");
        PrintMap(map, path);

        if (part == 1) {
            return cost.ToString();
        }

        ISet<Coord> pathCoords = path.Select(p => p.coord).ToHashSet();

        foreach (Coord newBlockCoord in coords.Skip(part1Limit)) {
            Logger.LogInformation("New blockage at {coord}", newBlockCoord);
            map[newBlockCoord.Y, newBlockCoord.X] = true;

            if (!pathCoords.Contains(newBlockCoord)) {
                Logger.LogInformation("--> Path unchanged");
                continue;
            }

            Logger.LogInformation("Finding new path...");
            (IList<(Coord coord, bool item)> newPath, int newCost) = search.FindPath(startCoord, endCoord);
            PrintMap(map, newPath);
            if (newCost == -1) {
                Logger.LogInformation("There is no longer a walkable path.");
                return $"{newBlockCoord.X},{newBlockCoord.Y}";
            }
        }
        throw new InvalidOperationException("Code should not have reached here");
    }

    private static void PrintMap(bool[,] map, IList<(Coord coord, bool item)> path) {
        if (!Program.IsVerbose) {
            return;
        }
        HashSet<Coord> visited = path.Select(p => p.coord).ToHashSet();

        map.Print((cellValue, coord) => new CellPrintInstruction {
                CellString = visited.Contains(coord) ? "O" : cellValue ? "#" : ".",
                Foreground = visited.Contains(coord) ? Green : White,
        });
    }
}
