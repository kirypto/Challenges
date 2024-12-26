using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(17)]
public class Day17: IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        string[] registersAndProgram = inputRepository.Fetch()
                .Replace(" ", "")
                .Replace("Register", "")
                .Replace("Program", "")
                .Split("\n\n");
        Dictionary<string,int> registers = registersAndProgram[0]
                .Split("\n")
                .Select(line => line.Split(":"))
                .ToDictionary(pair => pair[0], nameAndValue => int.Parse(nameAndValue[1]));


        IEnumerator<int> program = registersAndProgram[1][1..] // Drop initial ':'
                .Split(",")
                .Select(int.Parse)
                .GetEnumerator();
        while (program.MoveNext()) {
            int opcode = program.Current;
            if (!program.MoveNext()) {
                throw new InvalidOperationException("Unexpected end of program");
            }
            int operand = program.Current;
            Logger.LogInformation("Performing {opcode} with {operand}", opcode, operand);
        }

        throw new NotImplementedException();
    }
}
