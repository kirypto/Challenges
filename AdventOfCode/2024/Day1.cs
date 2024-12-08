using System;
using System.Collections.Generic;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Interfaces;
using Microsoft.Extensions.Logging;
using static kirypto.AdventOfCode.Common.Services.IO.DailyProgramLogger;

namespace kirypto.AdventOfCode._2024;

[DailyProgram(1)]
public class Day1 : IDailyProgram {
    public string Run(IInputRepository inputRepository, string inputRef, int part) {
        var nums = inputRepository.FetchRegexParsedLines<int, int>(inputRef, @"(\d+)\s+(\d+)");
        Logger.LogInformation($"Nums ({nums.Count}): [[{nums[0].Item1},{nums[0].Item2}],...]");
        throw new NotImplementedException();
    }
}
