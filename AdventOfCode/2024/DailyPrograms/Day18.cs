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
using static Microsoft.Extensions.Logging.LogLevel;

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
        PrintMap(map, new HashSet<Coord>());

        GridAStar<bool> search = new(map, isWalkable: b => !b);
        Coord startCoord = new(0, 0);
        Coord endCoord = new(colCount - 1, rowCount - 1);
        (IList<(Coord coord, bool item)> path, int cost) = search.FindPath(
                startCoord: startCoord,
                endCoord: endCoord);

        // Only need this for part 2 or verbose
        ISet<Coord> visitedCoords = part == 2 || Program.IsVerbose
                ? path.Select(p => p.coord).ToHashSet()
                : [];

        Logger.LogInformation(cost == -1 ? "No path found" : "Found path:");
        PrintMap(map, visitedCoords);

        if (part == 1) {
            return cost.ToString();
        }

        throw new NotImplementedException();
    }

    private static void PrintMap(bool[,] map, ISet<Coord> visited) {
        if (!Program.IsVerbose) {
            return;
        }

        map.Print((cellValue, coord) => new CellPrintInstruction {
                CellString = visited.Contains(coord) ? "O" : cellValue ? "#" : ".",
                Foreground = visited.Contains(coord) ? Green : White,
        });
    }
}
