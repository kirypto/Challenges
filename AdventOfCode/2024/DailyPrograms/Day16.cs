using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.Algorithms.Grids;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Collections.Extensions;
using kirypto.AdventOfCode.Common.Models;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;
using static System.ConsoleColor;
using static kirypto.AdventOfCode.Common.Models.CompassDirectionExtensions;

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
        (IList<(Coord coord, char item)> path, int cost) = search.AStarPath(start, end, DetermineStepCost(start));
        Logger.LogInformation("Shortest path cost (without turn): {cost}", cost);
        PrintMap(map, path);

        return cost.ToString();
    }


    private static void PrintMap(char[,] map, IList<(Coord coord, char item)> path) {
        if (!Program.IsVerbose) {
            return;
        }
        HashSet<Coord> visited = path.Select(p => p.coord).ToHashSet();

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

    private static Func<CostFunctionData, int> DetermineStepCost(Coord startCoord) => state => {
        Coord prev = state.Prev
                ?? startCoord.Move(CompassDirection.West); // Initial direction is east
        (_, Coord curr, Coord next) = state;

        CompassDirection prevDirection = GetDirectionToAdjacentCoord(prev, curr);
        CompassDirection nextDirection = GetDirectionToAdjacentCoord(curr, next);
        if (prevDirection == nextDirection) {
            return 1;
        }
        bool is90DegreeDiff = prevDirection.Rotate90Clockwise() == nextDirection
                || prevDirection.Rotate90Anticlockwise() == nextDirection;
        if (is90DegreeDiff) {
            // 1000 for rotation + 1 move
            return 1001;
        }
        // Must be 180 degrees diff, meaning 2 rotations + 1 move
        return 2001;
    };

    private static CompassDirection GetDirectionToAdjacentCoord(Coord coordA, Coord coordB) => CardinalDirections
            .First(direction => coordA.Move(direction) == coordB);
}
