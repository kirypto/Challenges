using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode._2023.Repos;
using static kirypto.AdventOfCode._2023.DailyPrograms.OasisHistoryRecord;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day9 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        List<OasisHistoryRecord> historyRecords = inputRepository.FetchLines(inputRef)
                .Select(ParseOasisHistoryRecord)
                .ToList();

        historyRecords.ForEach(history => Console.WriteLine(string.Join(",", history.Observations)));
    }
}

public readonly partial record struct OasisHistoryRecord(IList<long> Observations) {
    public static OasisHistoryRecord ParseOasisHistoryRecord(string recordString) {
        return new OasisHistoryRecord(
                OasisHistoryRecordEntryPattern().Matches(recordString)
                        .Select(match => long.Parse(match.Value))
                        .ToList());
    }

    [GeneratedRegex("(\\d+)")]
    private static partial Regex OasisHistoryRecordEntryPattern();
}
