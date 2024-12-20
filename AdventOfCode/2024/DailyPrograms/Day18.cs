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

        if (Logger.IsEnabled(Information)) {
            string mapAsString = map.ToString((cellValue, _) => cellValue ? "#" : ".");
            Logger.LogInformation($"Map ({rowCount}, {colCount}):\n{mapAsString}");
        }

        GridAStar<bool> search = new(map, isWalkable: b => !b);
        (IList<(Coord coord, bool item)> _, int cost) = search.FindPath(
                startCoord: new Coord(0, 0),
                endCoord: new Coord(colCount - 1, rowCount - 1));
        return cost.ToString();
    }
}
