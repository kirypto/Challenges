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

        int limit = part switch {
                1 => inputRepository is RestInputRepository && colCount == 7 && rowCount == 7 ? 12 : 1024,
                _ => 99999,
        };

        coords.Take(limit).ForEach(coord => map[coord.Y, coord.X] = true);

        bool isVerbose = Logger.IsEnabled(Information);
        if (isVerbose) {
            Logger.LogInformation($"Map ({rowCount}, {colCount}):");
            map.Print((cellValue, _) => cellValue ? "#" : ".");
        }

        GridAStar<bool> search = new(map, isWalkable: b => !b);
        Coord startCoord = new(0, 0);
        Coord endCoord = new Coord(colCount - 1, rowCount - 1);
        (IList<(Coord coord, bool item)> path, int cost) = search.FindPath(
                startCoord: startCoord,
                endCoord: endCoord);

        ISet<Coord> visitedCoords = part == 2 || isVerbose
            ? path.Select(p => p.coord).ToHashSet()
            : []; // If part 1 or not verbose, this is not needed, skip for efficiency

        if (isVerbose) {
            if (cost != -1) {
                Logger.LogInformation($"Found path of length {cost}:");
                map.Print((cellValue, coord) => {
                    bool wasVisited = visitedCoords.Contains(coord);
                    return new CellPrintInstruction {
                            CellString = wasVisited ? "O" : cellValue ? "#" : ".",
                            Foreground = wasVisited ? Green : White,
                    };
                });
            } else {
                Logger.LogInformation("No path found");
            }
        }

        if (part == 1) {
            return cost.ToString();
        }

        throw new NotImplementedException();
    }
}
