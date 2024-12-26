using System;
using System.Linq;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(22)]
public class Day22 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        inputRepository.Fetch();
        RunTests();
        throw new NotImplementedException();
    }

    private static void RunTests() {
        if (!Program.IsVerbose) {
            return;
        }
        Logger.LogInformation("Mix 15 into 42 (expecting 37): {result}", Mix(42, 15));
        Logger.LogInformation("Prune 100000000 (expecting 16113920): {result}", Prune(100000000));
        long secretNumber = 123;
        Logger.LogInformation("Sample, starting at 123:");
        for (int iteration = 0; iteration < 10; iteration++) {
            secretNumber = IterateSecretNumber(iteration, secretNumber);
            Logger.LogInformation("At {iteration} number is {secretNum}", iteration, secretNumber);
        }
    }

    private static long IterateSecretNumber(int iteration, long secretNumber) => (iteration % 3) switch {
            0 => Prune(Mix(secretNumber, 64 * secretNumber)),
            1 => Prune(Mix(secretNumber, secretNumber / 32)),
            2 => Prune(Mix(secretNumber, secretNumber * 2048)),
            _ => throw new InvalidOperationException($"Somehow it didn't work... iteration was {iteration}"),
    };

    private static long Mix(long secretNumber, long newNumber) => secretNumber ^ newNumber;
    private static long Prune(long secretNumber) => secretNumber % 16777216L;
}
