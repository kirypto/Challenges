using System;
using Microsoft.Extensions.Logging;

namespace kirypto.AdventOfCode.Common.AOC;

public class DailyProgramLogger(bool verboseFlag) : ILogger {
    private static DailyProgramLogger _logger;
    public static void Initialize(bool verboseFlag) => _logger = new DailyProgramLogger(verboseFlag);
    public static DailyProgramLogger Logger => _logger
            ?? throw new InvalidOperationException($"{nameof(DailyProgramLogger)} is not initialized");

    public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter
    ) {
        if (verboseFlag) {
            Console.WriteLine($"[{logLevel}] {formatter(state, exception)}");
        }
    }

    public bool IsEnabled(LogLevel logLevel) {
        return verboseFlag;
    }

    public IDisposable BeginScope<TState>(TState state) where TState : notnull => null;
}
