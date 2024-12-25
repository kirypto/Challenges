using System.Linq;
using kirypto.AdventOfCode.Common.Algorithms.Search;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Extensions;
using kirypto.AdventOfCode.Common.Interfaces;
using kirypto.AdventOfCode.Common.Models;
using Microsoft.Extensions.Logging;
using static System.ConsoleColor;
using static kirypto.AdventOfCode.Common.Services.IO.DailyProgramLogger;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(16)]
public class Day16 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        Coord start = Coord.None;
        Coord end = Coord.None;
        char[,] map = inputRepository
                .FetchAs2DCharArray((cell, coord) => {
                    switch (cell) {
                        case 'S':
                            start = coord;
                            return '.';
                        case 'E':
                            end = coord;
                            return '.';
                        default:
                            return cell;
                    }
                });

        GridSearch<char> search = new(map, c => c == '.');
        (System.Collections.Generic.IList<(Coord coord, char item)> path, int cost) = search.AStarPath(start, end);
        Logger.LogInformation("Shortest path cost (without turn): {cost}", cost);
        PrintMap(map, path);

        throw new System.NotImplementedException();
    }


    private static void PrintMap(char[,] map, System.Collections.Generic.IList<(Coord coord, char item)> path) {
        if (!Program.IsVerbose) {
            return;
        }
        System.Collections.Generic.HashSet<Coord> visited = path.Select(p => p.coord).ToHashSet();

        map.Print((cellValue, coord) => new CellPrintInstruction {
                CellString = visited.Contains(coord) ? "*" : cellValue.ToString(),
                Foreground = cellValue switch {
                        'S' => Blue,
                        'E' => Cyan,
                        'C' => Magenta,
                        _ when visited.Contains(coord) => Green,
                        _ => null,
                },
        });
    }
}
