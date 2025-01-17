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
        List<long> locks = [];
        List<long> keys = [];
                inputRepository.Fetch()
                .Replace("#", "1")
                .Replace(".", "0")
                .Replace("\n\n", ",")
                .Replace("\n", "")
                .Split(",")
                .Tap(numericString => Logger.LogInformation("Grid as flat string: {numericString}", numericString))
                .Select(ConvertStringToBinary)
                .Tap(binary => Logger.LogInformation("Binary: {binary}", binary.ToString()))
                .ForEach(binary => {
                    if ((binary & 1) == 0) {
                        Logger.LogInformation("Found Key, binary: {binary}", binary.ToString());
                        keys.Add(binary);
                    } else {
                        Logger.LogInformation("Found Lock, binary: {binary}", binary.ToString());
                        locks.Add(binary);
                    }
                });

        int count = 0;
        foreach (long key in keys) {
            foreach (long lock_ in locks) {
                Logger.LogInformation("Comparing the following 2:");
                Logger.LogInformation("  --> Lock: {lock}", ToBinaryString(lock_));
                Logger.LogInformation("  -->  Key: {key}", ToBinaryString(key));
                bool overlap = (key & lock_) > 0;
                if (!overlap) {
                    Logger.LogInformation("  --> Found one!");
                    count++;
                }
            }
        }
        return count.ToString();
    }

    private static string ToBinaryString(long keyOrLock) => Convert
            .ToString(keyOrLock, 2)
            .PadLeft(35, '0');

    private static long ConvertStringToBinary(string input) {
        long result = 0;
        foreach (char c in input) {
            result <<= 1;
            result |= (uint)(c - '0');
        }
        return result;
    }
}
