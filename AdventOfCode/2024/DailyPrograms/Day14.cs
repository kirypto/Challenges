using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Models;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(14)]
public class Day14 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        bool isExample = inputRepository is RestInputRepository { FetchCode: "example" };

        int rowCount = isExample ? 11 : 101;
        int colCount = isExample ? 7 : 103;

        List<Robot> robots = inputRepository.FetchLines()
                .Select(Robot.Parse)
                .ToList();


        Logger.LogInformation("Map size: Rows={rows}, Cols={cols}", rowCount, colCount);
        foreach (Robot robot in robots) {
            Logger.LogInformation("Predicting robot {robot}", robot);
        }

        throw new NotImplementedException();
    }

    private readonly record struct Robot(Coord Position, Coord Velocity) {
        // Expecting string of format `p=0,4 v=3,-3`
        public static Robot Parse(string input) => input
                .Replace("p=", "")
                .Replace("v=", "")
                .Split(" ")
                .Select(Coord.Parse)
                .Take(2)
                .Select((coord, index) => new { coord, index })
                .Aggregate(new Robot(), (robot1, obj) => obj.index switch {
                        0 => robot1 with { Position = obj.coord },
                        1 => robot1 with { Velocity = obj.coord },
                        _ => throw new InvalidOperationException("... This should not happen..."),
                });
    }
}
