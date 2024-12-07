using System;
using kirypto.AdventOfCode.Common.Services.IO;

namespace kirypto.AdventOfCode._2024;

public static class Program {
    public static void Main(string[] rawArgs) {
        AocProgramArguments args = AocArgumentService.Parse(rawArgs);
        Console.WriteLine(args);
    }
}
