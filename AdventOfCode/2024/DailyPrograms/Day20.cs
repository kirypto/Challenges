using System;
using System.Collections.Generic;
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
[DailyProgram(20)]
public class Day20 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        Coord start = Coord.None;
        Coord end = Coord.None;
        char[,] racetrack = inputRepository
                .WithFormatter(RemoveEmTagsInExampleInputs)
                .FetchAs2DCharArray((cell, coord) => {
                    if (cell == 'S') {
                        start = coord;
                    } else if (cell == 'E') {
                        end = coord;
                    }
                    return cell;
                });
        if (Program.IsVerbose) {
            racetrack.Print((cell, _) => cell.ToString());
        }
        Logger.LogInformation("Start: {start}, end: {end}", start, end);
        GridSearch<char> racetrackSearch = new(racetrack, cell => cell != '#');
        (IList<(Coord coord, char item)> path, int cost) = racetrackSearch.AStarPath(start, end);
        PrintMap(racetrack, path);
        Logger.LogInformation("Best non-cheating path is {cost}.", cost);

        ISet<Coord> possibleCheats = path
                .Select(pair => pair.coord)
                .SelectMany(coord => Enum.GetValues<CardinalDirection>()
                        .Select(direction => new { Direction = direction, Coord = coord.Move(direction) })
                        .Where(obj => racetrack.TryGetValue(obj.Coord, out char neighbourCell)
                                && neighbourCell == '#')
                        .Select(obj => obj.Coord))
                .ToHashSet();
        Logger.LogInformation("Possible cheat count {cheatCount}.", possibleCheats.Count);

        bool isExample = possibleCheats.Count == 107;
        int savingsMinimum = isExample ? 1 : 100;

        Dictionary<int, int> cheatSavingsCounts = new();
        foreach (Coord possibleCheat in possibleCheats) {
            racetrack[possibleCheat.Y, possibleCheat.X] = '.';
            (IList<(Coord coord, char item)> _, int costWithCheat) = racetrackSearch.AStarPath(start, end);
            int cheatSavings = cost - costWithCheat;
            if (cheatSavings >= savingsMinimum) {
                Logger.LogInformation("Cheat count {cheatCount}.", costWithCheat);
                cheatSavingsCounts.TryAdd(cheatSavings, 0);
                cheatSavingsCounts[cheatSavings]++;
            }
            racetrack[possibleCheat.Y, possibleCheat.X] = '#';
        }
        if (Program.IsVerbose) {
            cheatSavingsCounts.Keys
                    .ForEach(savings =>
                            Logger.LogInformation("{savings}: {count}", savings, cheatSavingsCounts[savings]));
        }
        return cheatSavingsCounts.Values
                .Sum()
                .ToString();
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

    private static string RemoveEmTagsInExampleInputs(string rawInput) => rawInput
            .Replace("<em>", "")
            .Replace("</em>", "");
}
