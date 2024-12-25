using System;
using System.Collections.Generic;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Interfaces;
using kirypto.AdventOfCode.Common.Models;
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

        throw new NotImplementedException();
    }
}
