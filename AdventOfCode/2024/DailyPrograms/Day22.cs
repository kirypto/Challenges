using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(22)]
public class Day22 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        inputRepository.Fetch();
        Logger.LogInformation("Mix 15 into 42 (expecting 37): {result}", Mix(42, 15));
        Logger.LogInformation("Prune 100000000 (expecting 16113920): {result}", Prune(100000000));
        throw new System.NotImplementedException();
    }

    private static long Mix(long secretNumber, long newNumber) => secretNumber ^ newNumber;
    private static long Prune(long secretNumber) => secretNumber % 16777216L;
}
