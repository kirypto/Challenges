using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace kirypto.AdventOfCode.Common.Interfaces;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")] // Library
public static class InputRepositoryExtensions {
    public static IList<string> FetchLines(this IInputRepository repository, string inputRef) {
        return repository.Fetch(inputRef)
                .ReplaceLineEndings()
                .TrimEnd(Environment.NewLine.ToCharArray())
                .Split(Environment.NewLine);
    }

    public static IList<Tuple<T1, T2>> FetchRegexParsedLines<T1, T2>(
            this IInputRepository repository,
            string inputRef,
            string pattern
    ) where T1 : IConvertible where T2 : IConvertible {
        Regex regex = new Regex(pattern);
        var results = new List<Tuple<T1, T2>>();
        foreach (string line in repository.FetchLines(inputRef)) {
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
