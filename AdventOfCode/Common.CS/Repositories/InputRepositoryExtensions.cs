using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode.Common.Models;

namespace kirypto.AdventOfCode.Common.Repositories;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")] // Library
public static class InputRepositoryExtensions {
    public static IInputRepository WithFormatter(this IInputRepository repository, Func<string, string> formatter) {
        return new InMemoryInputRepository(formatter(repository.Fetch()));
    }

    public static IList<string> FetchLines(this IInputRepository repository) {
        return repository.Fetch()
                .ReplaceLineEndings()
                .TrimEnd(Environment.NewLine.ToCharArray())
                .Split(Environment.NewLine);
    }

    public static char[,] FetchAs2DCharArray(
            this IInputRepository repository,
            Func<char, Coord, char> cellMutator = null
    ) {
        cellMutator ??= (cellValue, _) => cellValue;

        IList<string> lines = repository.FetchLines();
        int rows = lines.Count;
        int cols = lines[0].Length;
        char[,] result = new char[rows, cols];
        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < cols; col++) {
                result[row, col] = cellMutator(lines[row][col], new Coord { X = col, Y = row });
            }
        }
        return result;
    }

    public static IList<Tuple<T1, T2>> FetchRegexParsedLines<T1, T2>(
            this IInputRepository repository,
            string pattern
    ) where T1 : IConvertible where T2 : IConvertible {
        Regex regex = new Regex(pattern);
        var results = new List<Tuple<T1, T2>>();
        foreach (string line in repository.FetchLines()) {
            var match = regex.Match(line);
            if (!match.Success) {
                throw new FormatException($"Line `{line}` does not match regex `{regex}`");
            }
            var value1 = (T1)Convert.ChangeType(match.Groups[1].Value, typeof(T1));
            var value2 = (T2)Convert.ChangeType(match.Groups[2].Value, typeof(T2));
            results.Add(Tuple.Create(value1, value2));
        }

        return results;
    }
}
