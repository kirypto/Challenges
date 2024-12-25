using System.Linq;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Extensions;
using kirypto.AdventOfCode.Common.Interfaces;
using kirypto.AdventOfCode.Common.Models;
using Microsoft.Extensions.Logging;
using static kirypto.AdventOfCode.Common.Services.IO.DailyProgramLogger;

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
        throw new System.NotImplementedException();
    }
}
