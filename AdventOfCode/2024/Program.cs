using System;
using System.Linq;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Interfaces;
using kirypto.AdventOfCode.Common.Services.IO;
using Microsoft.Extensions.Logging;
using static kirypto.AdventOfCode.Common.Repositories.InputRepositoryFactory;
using static kirypto.AdventOfCode.Common.Services.IO.DailyProgramLogger;

namespace kirypto.AdventOfCode._2024;

public static class Program {
    public static bool IsVerbose { get; private set; }

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

        IInputRepository inputRepository = CreateInputRepository(args);
        IsVerbose = Logger.IsEnabled(LogLevel.Information); // TODO: Get this from AocProgramArguments

        string result = program.Run(
                inputRepository,
                args.Part
        );
        Console.WriteLine(result);
    }

    private static IDailyProgram DefaultDailyProgram => new DefaultProgram();

    private class DefaultProgram : IDailyProgram {
        public string Run(IInputRepository inputRepository, int part) {
            Logger.LogInformation("No matching daily program found, returning default answer.");
            return "42";
        }
    }
}
