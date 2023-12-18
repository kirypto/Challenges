using System;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;
using static System.Linq.Enumerable;
using MirrorMap = char[,];

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day13 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        int summary = inputRepository.Fetch(inputRef)
                .Split("\n\n")
                .Select(s => s.Trim())
                .Select(mirrorMapString => mirrorMapString.To2DCharArray("\n"))
                .Select(mirrorMap => mirrorMap.FindReflection())
                .Select(obj => (obj.isVertical ? 1 : 100) * obj.reflectionPoint)
                .Sum();
        Console.WriteLine($"Summary: {summary}");
    }
}

public static class MirrorMapExtensions {
    public static (bool isVertical, int reflectionPoint) FindReflection(this MirrorMap mirrorMap) {
        int verticalReflection = mirrorMap.FindVerticalReflection();
        return verticalReflection != -1
                ? (true, verticalReflection)
                : (false, mirrorMap.FindHorizontalReflection());
    }

    private static int FindVerticalReflection(this MirrorMap mirrorMap) {
        return Range(0, mirrorMap.GetLength(1))
                .Where(reflectionPoint => Range(0, mirrorMap.GetLength(0))
                        .All(row => mirrorMap.EnumerateRow(row).ReflectsAt(reflectionPoint))
                )
                .FirstOrDefault(-1);
    }

    private static int FindHorizontalReflection(this MirrorMap mirrorMap) {
        return Range(0, mirrorMap.GetLength(0))
                .Where(reflectionPoint => Range(0, mirrorMap.GetLength(1))
                        .All(col => mirrorMap.EnumerateCol(col).ReflectsAt(reflectionPoint))
                )
                .FirstOrDefault(-1);
    }
}
