using System;
using System.Linq;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Collections.Extensions;
using kirypto.AdventOfCode.Common.Models;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;

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
                .FetchLines()
                .Aggregate((line1, line2) => line1 + line2);

        Logger.LogInformation("Robot position {coord}", robotPos);
        map.Print();
        throw new NotImplementedException();
    }
}
