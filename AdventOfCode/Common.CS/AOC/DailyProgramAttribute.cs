using System;

namespace kirypto.AdventOfCode.Common.AOC;

[AttributeUsage(AttributeTargets.Class)]
public class DailyProgramAttribute(int day) : Attribute {
    public int Day => day;
}
