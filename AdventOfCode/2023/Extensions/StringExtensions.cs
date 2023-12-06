using System;

namespace kirypto.AdventOfCode._2023.Extensions;

public static class StringExtensions {
    public static string Reversed(this string s) {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}