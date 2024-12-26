using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Models;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;
using AntennaMap = System.Collections.Generic.Dictionary<char, System.Collections.Generic.HashSet<kirypto.AdventOfCode.Common.Models.Coord>>;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(8)]
public class Day08 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        char[,] map = inputRepository.FetchAs2DCharArray();
        AntennaMap antennaMap = new();

        for (int row = 0; row < map.GetLength(0); row++) {
            for (int col = 0; col < map.GetLength(1); col++) {
                Coord coord = new(row, col);
                char cell = map[row, col];
                if (cell != '.') {
                    if (antennaMap.TryGetValue(cell, out HashSet<Coord> existingCoords)) {
                        existingCoords.Add(coord);
                    } else {
                        antennaMap[cell] = [coord];
                    }
                }
            }
        }
        foreach (char antennaId in antennaMap.Keys) {
            Logger.LogInformation("Checking antenna {id}...", antennaId);
            HashSet<Coord> antennaCoords = antennaMap[antennaId];
            antennaCoords
                    .SelectMany(_ => antennaCoords, (a, b) => (a, b))
                    .Where(pair => pair.a < pair.b);
        }

        throw new NotImplementedException();
    }
}
