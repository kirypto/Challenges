using System;
using System.Linq;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Collections.Extensions;
using kirypto.AdventOfCode.Common.Models;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;
using static kirypto.AdventOfCode.Common.Models.CompassDirection;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(15)]
public class Day15 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        Coord robotPos = Coord.None;
        char[,] map = inputRepository
                .WithFormatter(raw => raw.Split("\n\n")[0])
                .FetchAs2DCharArray((cellChar, coord) => {
                    if (cellChar == '@') {
                        robotPos = coord;
                    }
                    return cellChar;
                });

        string allMovementInstructions = inputRepository
                .WithFormatter(raw => raw.Split("\n\n")[1])
                .WithFormatter(raw => raw.Replace("&lt;", "<").Replace("&gt;", ">"))
                .FetchLines()
                .Aggregate((line1, line2) => line1 + line2);
        Logger.LogInformation("Movement instructions: {instructions}", allMovementInstructions);

        Logger.LogInformation("Robot position {coord}", robotPos);
        map.Print();

        foreach (CompassDirection direction in allMovementInstructions.Select(ToCompassDirection)) {
            robotPos = robotPos.Move(direction);
            Logger.LogInformation("Robot moved {direction} to {coord}", direction, robotPos);
        }
        throw new NotImplementedException();
    }

    private static CompassDirection ToCompassDirection(char c) => c switch {
            '^' => North,
            '>' => East,
            'v' => South,
            '<' => West,
            _ => throw new InvalidOperationException($"Char {c} is not a valid direction."),
    };
}
