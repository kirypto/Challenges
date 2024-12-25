using System;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Interfaces;
using KeyPadMap = System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, string>>;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(21)]
public class Day21 : IDailyProgram {
    private static KeyPadMap KeyPadMap { get; } = InitializeKeyPadMap();

    private static KeyPadMap InitializeKeyPadMap() {

        throw new NotImplementedException();
    }

    public string Run(IInputRepository inputRepository, int part) {
        inputRepository.Fetch();

        throw new NotImplementedException();
    }
}
