using System;
using System.CommandLine;
using System.Linq;

namespace kirypto.AdventOfCode.Common.AOC;

public static class AocArgumentService {
    public static AocProgramArguments Parse(string[] args) {
        Option<int> day = new(
                ["-d", "--day"],
                "Specifies the day to run (1-25).") {
                IsRequired = true,
        };

        Option<int> part = new(
                ["-p", "--part"],
                "Specifies the part to run (1 or 2).") {
                IsRequired = true,
        };

        Option<string> inputFile = new(
                ["-i", "--input"],
                "The path to the file to use as input.") {
                IsRequired = false,
        };

        Option<string> fetchInput = new(
                ["-f", "--fetch"],
                "Instruct the program to fetch from the website (specify 'real' or 'example').") {
                IsRequired = false,
        };

        Option<bool> verbose = new(
                ["-v", "--verbose"],
                "Whether or not to print dev logs.") {
                IsRequired = false,
        };

        Option<bool> stats = new(
                ["-s", "--stats"],
                "Whether or not to print statistics.") {
                IsRequired = false,
        };

        Option<bool> usage = new(
                ["-u", "--usage"],
                "Displays this usage message.") {
                IsRequired = false,
        };

        RootCommand rootCommand = [
                day, part, inputFile, fetchInput, verbose, stats, usage,
        ];

        rootCommand.Description = "Advent Of Code Runner";

        if (args.Any(arg => arg is "-u" or "--usage")) {
            rootCommand.Invoke("--help");
            Environment.Exit(-1);
        }

        rootCommand.AddValidator(result => {
            bool inputWasProvided = result.GetValueForOption(inputFile) != null;
            bool fetchWasProvided = result.GetValueForOption(fetchInput) != null;
            if (inputWasProvided && fetchWasProvided) {
                result.ErrorMessage = "You must specify either --input or --fetch, but not both.";
            }
        });

        AocProgramArguments parsedArguments = new();

        rootCommand.SetHandler(
                (day_, part_, input, fetch, verbose_, stats_, _) => {
                    DailyProgramLogger.Initialize(verbose_);
                    parsedArguments = new AocProgramArguments(day_, part_, input, fetch, verbose_, stats_);
                },
                day, part, inputFile, fetchInput, verbose, stats, usage
        );

        if (0 != rootCommand.Invoke(args)) {
            Environment.Exit(-1);
        }

        return parsedArguments;
    }
}
