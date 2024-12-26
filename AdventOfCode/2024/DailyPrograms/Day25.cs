using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Collections.Extensions;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(25)]
public class Day25 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        List<long> keysAndLocks = inputRepository.Fetch()
                .Replace("#", "1")
                .Replace(".", "0")
                .Replace("\n\n", ",")
                .Replace("\n", "")
                .Split(",")
                .Tap(numericString => Logger.LogInformation(numericString))
                .Select(ConvertStringToBinary)
                .ToList();
        if (Program.IsVerbose) {
            foreach (long keyOrLock in keysAndLocks) {
                Logger.LogInformation(Convert.ToString(keyOrLock, 2));
            }
        }
        throw new NotImplementedException();
    }

    private static long ConvertStringToBinary(string input) {
        long result = 0;
        foreach (char c in input) {
            result <<= 1;
            if (c == '#') {
                result |= 1;
            }
        }
        return result;
    }
}
