using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Collections.Extensions;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;
using static System.StringSplitOptions;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(24)]
public partial class Day24 : IDailyProgram {
    private delegate void EquationSolved(string outId, long value);

    private delegate void InputProvided(long input);

    public string Run(IInputRepository inputRepository, int part) {
        string[] registersAndEquations = inputRepository
                .WithFormatter(raw => raw
                        .Replace("&gt;", ">")
                        .Replace("AND", "And")
                        .Replace("XOR", "Xor")
                        .Replace("OR", "Or"))
                .Fetch()
                .Split("\n\n");

        Dictionary<string, long> registers = [];
        Dictionary<string, HashSet<Equation>> subscriptions = [];
        registersAndEquations[1]
                .Split("\n", RemoveEmptyEntries | TrimEntries)
                .Select(Equation.Parse)
                .ForEach(equation => {
                    Logger.LogInformation("Registering new equation {equation}", equation);
                    registers[equation.InIdA] = 0;
                    registers[equation.InIdB] = 0;
                    registers[equation.OutId] = 0;
                    if (subscriptions.TryGetValue(equation.InIdA, out HashSet<Equation> subsA)) {
                        subsA.Add(equation);
                    } else {
                        subscriptions[equation.InIdA] = [equation];
                    }
                    if (subscriptions.TryGetValue(equation.InIdB, out HashSet<Equation> subsB)) {
                        subsB.Add(equation);
                    } else {
                        subscriptions[equation.InIdB] = [equation];
                    }
                });

        throw new NotImplementedException();
    }

    private enum Operation {
        And,
        Or,
        Xor,
    }

    private partial record Equation(string InIdA, string InIdB, Operation Op, string OutId) {
        private long valueA = -1;
        private long valueB = -1;
        public bool ProvideInput(string inputId, int value) {
            if (inputId == InIdA) {
                valueA = value;
            } else if (inputId == InIdB) {
                valueB = value;
            }
            return valueA != -1 && valueB != -1;
        }

        public static Equation Parse(string raw) {
            // Expected format: `x00 AND y00 -> z00`
            Match match = RawEquationPattern().Match(raw);
            return new Equation(
                    match.Groups[1].Value,
                    match.Groups[3].Value,
                    Enum.Parse<Operation>(match.Groups[2].Value),
                    match.Groups[4].Value);
        }

        [GeneratedRegex(@"(\w+) (\w+) (\w+) -> (\w+)")]
        private static partial Regex RawEquationPattern();
    };
}
