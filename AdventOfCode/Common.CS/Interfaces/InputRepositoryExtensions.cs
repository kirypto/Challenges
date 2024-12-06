using System;
using System.Collections.Generic;

namespace kirypto.AdventOfCode.Common.Interfaces;

public static class InputRepositoryExtensions {
    public static IList<string> FetchLines(this IInputRepository repository, string inputRef) {
        return repository.Fetch(inputRef)
                .ReplaceLineEndings()
                .TrimEnd(Environment.NewLine.ToCharArray())
                .Split(Environment.NewLine);
    }
}

