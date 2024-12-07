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

        var usageOption = new Option<bool>(
                ["-u", "--usage"],
                "Displays this usage message.") {
                IsRequired = false,
        };

        var rootCommand = new RootCommand {
                dayOption,
                partOption,
                inputFileOption,
                usageOption,
        };

        rootCommand.Description = "Advent Of Code Runner";

        if (args.Any(arg => arg is "-u" or "--usage")) {
            rootCommand.Invoke("--help");
            return default; // Early exit, no need to parse further
        }

        var parsedArguments = new AocProgramArguments();

        rootCommand.SetHandler(
                (day, part, input, _) => {
                    parsedArguments = new AocProgramArguments(day, part, input);
                },
                dayOption, partOption, inputFileOption, usageOption
        );

        rootCommand.Invoke(args);

        return parsedArguments;
    }
}
