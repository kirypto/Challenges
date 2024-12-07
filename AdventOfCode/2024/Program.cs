using System;
using System.Linq;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Interfaces;
using kirypto.AdventOfCode.Common.Repositories;
using kirypto.AdventOfCode.Common.Services.IO;
using Microsoft.Extensions.Logging;
using static kirypto.AdventOfCode.Common.Services.IO.DailyProgramLogger;

namespace kirypto.AdventOfCode._2024;

public static class Program {
    public static void Main(string[] rawArgs) {
        AocProgramArguments args = AocArgumentService.Parse(rawArgs);
        IDailyProgram program = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetCustomAttributes(typeof(DailyProgramAttribute), false)
                        .OfType<DailyProgramAttribute>()
                        .Any(attr => attr.Day == args.Day))
                .Select(Activator.CreateInstance)
                .OfType<IDailyProgram>()
                .FirstOrDefault(DefaultDailyProgram);

        string result = program.Run(
                new FileInputRepository(),
                args.InputFile,
                args.Part
        );
        Console.WriteLine(result);
    }

    private static IDailyProgram DefaultDailyProgram => new DefaultProgram();

    private class DefaultProgram : IDailyProgram {
        public string Run(IInputRepository inputRepository, string inputRef, int part) {
            Logger.LogInformation("No matching daily program found, returning default answer.");
            return "42";
        }
    }
}
