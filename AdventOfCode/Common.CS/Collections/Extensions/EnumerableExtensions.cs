using System;
using System.Collections.Generic;

namespace kirypto.AdventOfCode.Common.Collections.Extensions;

public static class EnumerableExtensions {
    public static string CommaDelimited<T>(this IEnumerable<T> source) {
        return source == null ? "null" : $"[{string.Join(", ", source)}]";
    }

    public static IEnumerable<T> Tap<T>(this IEnumerable<T> source, Action<T> action) {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(action);

        foreach (T item in source) {
            action(item);
            yield return item;
        }
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
        foreach (T item in source) {
            action(item);
        }
    }
}
