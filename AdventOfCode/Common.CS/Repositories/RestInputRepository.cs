using System;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode.Common.Interfaces;
using kirypto.AdventOfCode.Common.Services.IO;
using Microsoft.Extensions.Logging;

namespace kirypto.AdventOfCode.Common.Repositories;

public class RestInputRepository(int day, string fetchCode) : IInputRepository {
    public string Fetch() {
        DailyProgramLogger.Logger.LogInformation($"Fetching {fetchCode} input from website for day {day}");
        throw new NotImplementedException();
    }
}
