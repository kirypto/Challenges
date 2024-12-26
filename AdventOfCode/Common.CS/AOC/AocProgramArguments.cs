namespace kirypto.AdventOfCode.Common.AOC;

public readonly record struct AocProgramArguments(
        int Day,
        int Part,
        string InputFile,
        string FetchCode,
        bool Verbose,
        bool Stats);
