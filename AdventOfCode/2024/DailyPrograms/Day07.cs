using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.AOC;
using kirypto.AdventOfCode.Common.Repositories;
using Microsoft.Extensions.Logging;
using static System.StringSplitOptions;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(7)]
public class Day07 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        List<Equation> equations = inputRepository.FetchLines()
                .Select(Equation.Parse)
                .ToList();
        Logger.LogInformation("Found {count} equations", equations.Count);

        HashSet<Func<int, int, int>> operators = [
            (a, b) => a + b,
            (a, b) => a * b,
        ];

        foreach (Equation equation in equations) {
            Logger.LogInformation("Working on equation {result}: {values}", equation.Result, equation.Values);

        }

        throw new NotImplementedException();
    }

    private static Equation Reduce(Equation equation) {
        if (equation.Values.Count == 1) {
            return equation;
        }
        int a = equation.Values[0];
        int b = equation.Values[1];
        return Reduce(equation with {
                Values = [],
        });
    }


    private readonly record struct Equation(int Result, IList<int> Values) {
        public static Func<string, Equation> Parse => raw => {
            string[] resultAndValues = raw.Split(":");
            return new Equation {
                    Result = int.Parse(resultAndValues[0]),
                    Values = resultAndValues[1].Split(" ", TrimEntries | RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToList(),
            };
        };
    }
}
