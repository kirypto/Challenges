using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Interfaces;
using kirypto.AdventOfCode.Common.Models;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(14)]
public class Day14 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        inputRepository.FetchLines();
        throw new NotImplementedException();
    }

    private static Func<string, object> ParseRobotInput => input => {
        List<Coord> positionAndVelocity = input
                .Replace("p=", "")
                .Replace("v=", "")
                .Split(" ")
                .Select(Coord.Parse)
                .ToList();
        return "";
    };


    private readonly record struct Robot(Coord Position, Coord Velocity);
}
