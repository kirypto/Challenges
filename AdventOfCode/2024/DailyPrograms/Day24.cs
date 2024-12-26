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
    public string Run(IInputRepository inputRepository, int part) {
        string[] registersAndEquations = inputRepository
                .WithFormatter(raw => raw
                        .Replace("&gt;", ">")
                        .Replace("AND", "And")
                        .Replace("XOR", "Xor")
                        .Replace("OR", "Or"))
                .Fetch()
                .Split("\n\n");

        Dictionary<string, int> registers = [];
        Dictionary<string, HashSet<Equation>> subscriptions = [];
        Queue<Equation> readyEquations = new();
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

        registersAndEquations[0]
                .Replace(" ", "")
                .Split("\n", RemoveEmptyEntries | TrimEntries)
                .Select(registerRaw => registerRaw.Split(":"))
                .ForEach(nameAndValue => ProvideInput(nameAndValue[0], int.Parse(nameAndValue[1])));

        while (readyEquations.Count > 0) {
            Equation equation = readyEquations.Dequeue();
            int newValue = equation.Evaluate();
            string outId = equation.OutId;
            Logger.LogInformation("Equation {equation} finished with value {val}", equation, newValue);
            registers[outId] = newValue;
            if (subscriptions.TryGetValue(outId, out HashSet<Equation> newResultSubscriptions)) {
                newResultSubscriptions.ForEach(subscribed => subscribed.ProvideInput(outId, newValue));
            }
        }

        int final = registers.Keys
                .Where(register => register.StartsWith('z'))
                .OrderDescending()
                .Select(register => registers[register])
                .Aggregate(0, (soFar, newVal) => (soFar << 1) | newVal);
        return final.ToString();

        void ProvideInput(string registerId, int value) {
            Logger.LogInformation("Setting value for register {register} to {value}", registerId, value);
            subscriptions[registerId].ForEach(equation => {
                if (equation.ProvideInput(registerId, value)) {
                    readyEquations.Enqueue(equation);
                    Logger.LogInformation("--> Equation {equation} is now ready to run", equation);
                }
            });
        }
    }

    private enum Operation {
        And,
        Or,
        Xor,
    }

    private partial record Equation(string InIdA, string InIdB, Operation Op, string OutId) {
        private int valueA = -1;
        private int valueB = -1;

        public bool ProvideInput(string inputId, int value) {
            if (inputId == InIdA) {
                valueA = value;
            } else if (inputId == InIdB) {
                valueB = value;
            }
            return IsReadyToRun;
        }

        private bool IsReadyToRun => valueA != -1 && valueB != -1;

        public int Evaluate() {
            if (!IsReadyToRun) {
                throw new InvalidOperationException($"Equation {this} is not yet ready to run.");
            }

            return Op switch {
                    Operation.And => valueA & valueB,
                    Operation.Or => valueA | valueB,
                    Operation.Xor => valueA ^ valueB,
                    _ => throw new NotImplementedException($"Operation {Op} not supported"),
            };
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
    }
}
