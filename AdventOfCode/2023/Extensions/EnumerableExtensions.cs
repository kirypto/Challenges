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

    public static IEnumerable<T> InfinitelyRepeat<T>(this IEnumerable<T> enumerable) {
        IList<T> viewed = new List<T>();
        foreach (T t in enumerable) {
            viewed.Add(t);
            yield return t;
        }
        while (true) {
            foreach (T t in viewed) {
                yield return t;
            }
        }
        // ReSharper disable once IteratorNeverReturns
    }
}