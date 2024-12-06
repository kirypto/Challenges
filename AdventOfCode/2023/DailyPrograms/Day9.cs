using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode.Common.Interfaces;
using static kirypto.AdventOfCode._2023.DailyPrograms.OasisHistoryRecord;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day9 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        long sumOfPredictions = inputRepository.FetchLines(inputRef)
                .Select(ParseOasisHistoryRecord)
                .Select(PredictNext)
                .Select(obj => part == 1 ? obj.next : obj.previous)
                .Sum();

        Console.WriteLine($"Sum of predictions: {sumOfPredictions}");
    }

    private static (long next, long previous) PredictNext(OasisHistoryRecord historyRecord) {
        int observationsCount = historyRecord.Observations.Count;
        var predictMatrix = new long[observationsCount + 1, observationsCount + 1];
        for (var col = 0; col < observationsCount; col++) {
            predictMatrix[0, col] = historyRecord.Observations[col];
            for (int rCol = col - 1; rCol >= 0; rCol--) {
                int rRow = col - rCol;
                predictMatrix[rRow, rCol] = predictMatrix[rRow - 1, rCol + 1] - predictMatrix[rRow - 1, rCol];
            }
        }
        long lastVal = 0;
        for (int row = observationsCount - 1; row >= 0; row--) {
            int col = observationsCount - row;
            lastVal += predictMatrix[row, col - 1];
            predictMatrix[row, col] = lastVal;
        }
        long next = predictMatrix[0, observationsCount];
        long previous = 0;
        for (int row = observationsCount - 1; row >= 0; row--) {
            previous = predictMatrix[row, 0] - previous;
        }
        return (next, previous);
    }
}

public readonly partial record struct OasisHistoryRecord(IList<long> Observations) {
    public static OasisHistoryRecord ParseOasisHistoryRecord(string recordString) {
        return new OasisHistoryRecord(
                OasisHistoryRecordEntryPattern().Matches(recordString)
                        .Select(match => long.Parse(match.Value))
                        .ToList());
    }

    [GeneratedRegex("(-?\\d+)")]
    private static partial Regex OasisHistoryRecordEntryPattern();
}

// #1 982489301 -> Too Low