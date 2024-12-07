using System;
using kirypto.AdventOfCode.Common.Services.IO;
using Microsoft.Extensions.Logging;

namespace kirypto.AdventOfCode._2024;

public static class Program {
    public static void Main(string[] rawArgs) {
        AocProgramArguments args = AocArgumentService.Parse(rawArgs);
        DailyProgramLogger.Logger.LogInformation($"{args}");
        Console.WriteLine("Always");
        DailyProgramLogger.Logger.LogInformation("Only if verbose");
    }
}
