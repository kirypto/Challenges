namespace kirypto.AdventOfCode.Common.Services.IO;

public readonly record struct AocProgramArguments(
        int Day,
        int Part,
        string InputFile,
        string FetchCode,
        bool Verbose,
        bool Stats);
