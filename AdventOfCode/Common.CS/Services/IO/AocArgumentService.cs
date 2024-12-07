using System;
using System.CommandLine;
using System.Linq;

namespace kirypto.AdventOfCode.Common.Services.IO;

public static class AocArgumentService {
    public static AocProgramArguments Parse(string[] args) {
        var dayOption = new Option<int>(
                ["-d", "--day"],
                "Specifies the day to run (1-25).") {
                IsRequired = true,
        };

        var partOption = new Option<int>(
                ["-p", "--part"],
                "Specifies the part to run (1 or 2).") {
                IsRequired = true,
        };

        var inputFileOption = new Option<string>(
                ["-i", "--input"],
                "The path to the file to use as input.") {
                IsRequired = true,
        };

        var verboseOption = new Option<bool>(
                ["-v", "--verbose"],
                "Whether or not to print dev logs.") {
                IsRequired = false,
        };

        var usageOption = new Option<bool>(
                ["-u", "--usage"],
                "Displays this usage message.") {
                IsRequired = false,
        };

        var rootCommand = new RootCommand {
                dayOption,
                partOption,
                inputFileOption,
                verboseOption,
                usageOption,
        };

        rootCommand.Description = "Advent Of Code Runner";

        if (args.Any(arg => arg is "-u" or "--usage")) {
            rootCommand.Invoke("--help");
            Environment.Exit(-1);
        }

        var parsedArguments = new AocProgramArguments();

        rootCommand.SetHandler(
                (day, part, input, verbose, _) => {
                    DailyProgramLogger.Initialize(verbose);
                    parsedArguments = new AocProgramArguments(day, part, input);
                },
                dayOption, partOption, inputFileOption, verboseOption, usageOption
        );

        if (0 != rootCommand.Invoke(args)) {
            Environment.Exit(-1);
        }

        return parsedArguments;
    }
}
