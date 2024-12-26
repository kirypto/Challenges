using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Repositories;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(17)]
public class Day17: IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        string[] registersAndProgram = inputRepository.Fetch()
                .Replace(" ", "")
                .Replace("Register", "")
                .Replace("Program ", "")
                .Split("\n\n");
        Dictionary<string,int> registers = registersAndProgram[0]
                .Split("\n")
                .Select(line => line.Split(":"))
                .ToDictionary(pair => pair[0], nameAndValue => int.Parse(nameAndValue[1]));

        throw new NotImplementedException();
    }
}
