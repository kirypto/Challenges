using System;
using System.Collections.Generic;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Extensions;
using kirypto.AdventOfCode.Common.Interfaces;
using kirypto.AdventOfCode.Common.Models;
using Microsoft.Extensions.Logging;
using static kirypto.AdventOfCode.Common.Services.IO.DailyProgramLogger;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(4)]
public class Day04 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        char[,] grid = inputRepository.FetchAs2DCharArray();

        HashSet<Coord> xPositions = [];
        for (int row = 0; row < grid.GetLength(0); row++) {
            for (int col = 0; col < grid.GetLength(1); col++) {
                if (grid[row, col] == 'X') {
                    Coord coord = new(row, col);
                    Logger.LogInformation("Found an X at {coord}", coord);
                    xPositions.Add(coord);
                }
            }
        }

        List<char> letters = ['M', 'A', 'S'];
        foreach (Coord wordStart in xPositions) {
            Coord position = wordStart;
            foreach (CardinalDirection direction in Enum.GetValues<CardinalDirection>()) {
                foreach (char letter in letters) {
                    position = position.Move(direction);
                    if (!grid.TryGetValue(position, out char cellChar) || cellChar != letter) {
                        // Will not work
                        break;
                    }
                    
                }
            }
        }

        throw new System.NotImplementedException();
    }
}
