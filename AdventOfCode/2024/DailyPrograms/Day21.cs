using System;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Repositories;
using KeyPadMap = System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, string>>;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(21)]
public class Day21 : IDailyProgram {
    private static KeyPadMap KeyPadMap { get; } = InitializeKeyPadMap();

    public string Run(IInputRepository inputRepository, int part) {
        inputRepository.Fetch();

        throw new NotImplementedException();
    }

    private static KeyPadMap InitializeKeyPadMap() {
        char[,] keyPad = new char[4, 3] {
                { '7', '8', '9' },
                { '4', '5', '6' },
                { '1', '2', '3' },
                { '.', '0', 'A' },
        };

        throw new NotImplementedException();
    }
}
