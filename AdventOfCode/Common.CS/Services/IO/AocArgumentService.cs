using System;
using System.CommandLine;
using System.Linq;

namespace kirypto.AdventOfCode.Common.Services.IO;

public static class AocArgumentService {
    public static AocProgramArguments Parse(string[] args) {
        Option<int> dayOption = new(
                ["-d", "--day"],
                "Specifies the day to run (1-25).") {
                IsRequired = true,
        };

        Option<int> partOption = new(
                ["-p", "--part"],
                "Specifies the part to run (1 or 2).") {
                IsRequired = true,
        };

        Option<string> inputFileOption = new(
                ["-i", "--input"],
                "The path to the file to use as input.") {
                IsRequired = false,
        };

        Option<string> fetchInputOption = new(
                ["-f", "--fetch"],
                "Instruct the program to fetch from the website (specify 'real' or 'example').") {
                IsRequired = false,
        };

        Option<bool> verboseOption = new(
                ["-v", "--verbose"],
                "Whether or not to print dev logs.") {
                IsRequired = false,
        };

        Option<bool> usageOption = new(
                ["-u", "--usage"],
                "Displays this usage message.") {
                IsRequired = false,
        };

        RootCommand rootCommand = [
                dayOption, partOption, inputFileOption, fetchInputOption, verboseOption, usageOption
        ];

        rootCommand.Description = "Advent Of Code Runner";

        if (args.Any(arg => arg is "-u" or "--usage")) {
            rootCommand.Invoke("--help");
            Environment.Exit(-1);
        }

        rootCommand.AddValidator(result => {
            bool inputWasProvided = result.GetValueForOption(inputFileOption) != null;
            bool fetchWasProvided = result.GetValueForOption(fetchInputOption) != null;
            if (inputWasProvided && fetchWasProvided) {
                result.ErrorMessage = "You must specify either --input or --fetch, but not both.";
            }
        });

        AocProgramArguments parsedArguments = new();

        rootCommand.SetHandler(
                (day, part, input, fetch, verbose, _) => {
                    DailyProgramLogger.Initialize(verbose);
                    parsedArguments = new AocProgramArguments(day, part, input, fetch);
                },
                dayOption, partOption, inputFileOption, fetchInputOption, verboseOption, usageOption
        );

        if (0 != rootCommand.Invoke(args)) {
            Environment.Exit(-1);
        }

        return parsedArguments;
    }
}
