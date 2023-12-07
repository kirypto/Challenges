using System;
using System.Collections.Generic;
using System.Linq;

namespace kirypto.AdventOfCode._2023.Extensions;

public static class EnumerableExtensions {
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) {
        foreach (T t in enumerable) {
            action(t);
        }
    }

    public static IEnumerable<T> Tap<T>(this IEnumerable<T> enumerable, Action<T> action) {
        return enumerable.Select(t => {
            action(t);
            return t;
        });
    }
}