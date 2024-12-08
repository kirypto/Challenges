using System.Collections.Generic;

namespace kirypto.AdventOfCode.Common.Extensions;

public static class EnumerableExtensions {
    public static string CommaDelimited<T>(this IEnumerable<T> source) {
        return source == null ? "null" : $"[{string.Join(", ", source)}]";
    }
}
