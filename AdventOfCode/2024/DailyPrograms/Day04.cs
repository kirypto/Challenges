using System.Collections.Generic;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Collections.Extensions;
using kirypto.AdventOfCode.Common.Models;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;
using static System.ConsoleColor;
using static kirypto.AdventOfCode.Common.Models.CompassDirectionExtensions;

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
                    Coord coord = new() { Y = row, X = col };
                    Logger.LogInformation("Found an X at {coord}", coord);
                    xPositions.Add(coord);
                }
            }
        }

        List<char> letters = ['M', 'A', 'S'];
        int foundCount = 0;
        foreach (Coord wordStart in xPositions) {
            if (Program.IsVerbose) {
                grid.Print((cell, coord) => coord == wordStart
                        ? new CellPrintInstruction(cell.ToString(), Red)
                        : cell.ToString());
            }
            Logger.LogInformation("Checking X at {coord}", wordStart);

            foreach (CompassDirection direction in CardinalDirections) {
                Coord position = wordStart;
                Logger.LogInformation("  --> Checking direction {direction}", direction);
                bool allWorked = true;
                foreach (char letter in letters) {
                    position = position.Move(direction);
                    Logger.LogInformation("    --> Need a {letter} at {position}...", letter, position);
                    if (!grid.TryGetValue(position, out char cellChar) || cellChar != letter) {
                        Logger.LogInformation("      --> Nope!");
                        allWorked = false;
                        break;
                    }
                }
                if (allWorked) {
                    Logger.LogInformation("    --> Found one!");
                    foundCount++;
                }
            }
        }

        return foundCount.ToString();
    }
}
