using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace kirypto.AdventOfCode._2023.Extensions;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
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

    public static IEnumerator<T> InfinitelyRepeat<T>(this IEnumerable<T> enumerable) {
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

    public static bool ReflectsAt<T>(this IEnumerable<T> enumerable, int index) {
        using IEnumerator<T> mirrorEnumerator = enumerable.GetEnumerator();
        var observed = new List<T>();
        int i = -1;
        while (++i < index && mirrorEnumerator.GetNext(out T current)) {
            // Console.WriteLine($"Index {i}: {current}");
            observed.Add(current);
        }
        for (int j = observed.Count - 1; j >= 0; j--) {
            if (mirrorEnumerator.GetNext(out T current)) {
                if (current != null && !current.Equals(observed[j])) {
                    return false;
                }
            } else {
                break;
            }
        }
        return observed.Any();
    }

    public static bool GetNext<T>(this IEnumerator<T> enumerator, out T t) {
        try {
            t = enumerator.Current;
        }
        catch (InvalidOperationException) {
            // Hasn't yet started enumerating;
            bool moveSuccessful = enumerator.MoveNext();
            t = enumerator.Current;
            return moveSuccessful;
        }
        if (enumerator.MoveNext()) {
            t = enumerator.Current;
            return true;
        }
        return false;
    }
}