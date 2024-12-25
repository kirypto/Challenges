global using static kirypto.AdventOfCode.Common.Services.IO.DailyProgramLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Extensions;
using kirypto.AdventOfCode.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

[DailyProgram(1)]
public class Day01 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        var nums = inputRepository.FetchRegexParsedLines<int, int>(@"(\d+)\s+(\d+)");
        Logger.LogInformation($"Nums ({nums.Count}): [[{nums[0].Item1},{nums[0].Item2}],...]");
        SortedList<int, int> listA = [];
        SortedList<int, int> listB = [];
        Logger.LogInformation("Populating sorted lists...");
        foreach ((int item1, int item2) in nums) {
            if (!listA.TryAdd(item1, 1)) {
                listA[item1] += 1;
            }
            if (!listB.TryAdd(item2, 1)) {
                listB[item2] += 1;
            }
        }
        Logger.LogInformation($"List A: {listA.CommaDelimited()}");
        Logger.LogInformation($"List B: {listB.CommaDelimited()}");

        return part == 1 ? Part1(listA, listB) : Part2(listA, listB);
    }

    private static string Part1(SortedList<int, int> listA, SortedList<int, int> listB) {
        Logger.LogInformation("Summing differences...");
        int result = 0;
        while (listA.Count > 0 && listB.Count > 0) {
            Logger.LogInformation($"List A: {listA.CommaDelimited()}");
            Logger.LogInformation($"List B: {listB.CommaDelimited()}");
            int itemA = listA.Keys[0];
            int itemB = listB.Keys[0];
            int countA = listA[itemA];
            int countB = listB[itemB];
            int diff = Math.Abs(itemA - itemB);
            if (countA == countB) {
                Logger.LogInformation("Case 1");
                result += diff * countA;
                listA.RemoveAt(0);
                listB.RemoveAt(0);
            } else if (countA > countB) {
                Logger.LogInformation("Case 2");
                result += diff * countB;
                listB.RemoveAt(0);
                listA[itemA] -= countB;
            } else {
                Logger.LogInformation("Case 3");
                result += diff * countA;
                listA.RemoveAt(0);
                listB[itemB] -= countA;
            }
            Logger.LogInformation($"Result is now {result}");
        }
        Logger.LogInformation("Done!");
        return result.ToString();
    }

    private static string Part2(SortedList<int, int> listA, SortedList<int, int> listB) {
        return listA.Keys
                .Select(itemA => listB.TryGetValue(itemA, out int count) ? itemA * count : 0)
                .Sum()
                .ToString();
    }
}
