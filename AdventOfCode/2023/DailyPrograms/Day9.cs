using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;
using static kirypto.AdventOfCode._2023.DailyPrograms.OasisHistoryRecord;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day9 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        long sumOfPredictions = inputRepository.FetchLines(inputRef)
                .Select(ParseOasisHistoryRecord)
                .Select(PredictNext)
                // .Tap(prediction => Console.WriteLine($"Predicted {prediction}"))
                .Sum();

        Console.WriteLine($"Sum of predictions: {sumOfPredictions}");
    }

    private static long PredictNext(OasisHistoryRecord historyRecord) {
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
        return predictMatrix[0, observationsCount];
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