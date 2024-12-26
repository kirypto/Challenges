using System;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Repositories;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(24)]
public class Day24 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        throw new NotImplementedException();
    }

    private enum Operation {
        And,
        Or,
        Xor,
    }

    private record Equation(string InIdA, string InIdB, Operation Op) {
        private int valueA;
        private int valueB;
        // public bool ProvideInput(string inputId, int value) {
        // }
    };
}
