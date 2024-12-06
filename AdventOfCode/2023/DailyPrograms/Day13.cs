using System;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode.Common.Interfaces;
using static System.Linq.Enumerable;
using MirrorMap = char[,];

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day13 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        int summary = inputRepository.Fetch(inputRef)
                .Split("\n\n")
                .Select(s => s.Trim())
                .Select(mirrorMapString => mirrorMapString.To2DCharArray("\n"))
                .Select(mirrorMap => new {
                        MirrorMap = mirrorMap,
                        Reflection = mirrorMap.FindReflection(),
                })
                .Select(obj => part == 1
                        ? obj.Reflection
                        : obj.MirrorMap.FindDifferentSmudgeReflection(obj.Reflection))
                .Select(reflectionData => (reflectionData.IsVertical ? 1 : 100) * reflectionData.ReflectionPoint)
                .Sum();
        Console.WriteLine($"Summary: {summary}");
    }
}

public static class MirrorMapExtensions {
    public static Reflection FindDifferentSmudgeReflection(
            this MirrorMap mirrorMap, Reflection originalReflection
    ) {
        int rowCount = mirrorMap.GetLength(0);
        int colCount = mirrorMap.GetLength(1);
        for (var row = 0; row < rowCount; row++) {
            for (var col = 0; col < colCount; col++) {
                char sourceChar = mirrorMap[row, col];
                char swapChar = sourceChar == '#' ? '.' : '#';
                mirrorMap[row, col] = swapChar;
                var smudgeFixedReflection = mirrorMap.FindReflection(except:originalReflection);
                mirrorMap[row, col] = sourceChar;
                if (smudgeFixedReflection.ReflectionPoint != -1 && smudgeFixedReflection != originalReflection) {
                    return smudgeFixedReflection;
                }
            }
        }
        return new Reflection(false, -1);
    }

    public static Reflection FindReflection(
            this MirrorMap mirrorMap, Reflection? except = null
    ) {
        int verticalReflection = mirrorMap.FindVerticalReflection(except);
        return verticalReflection != -1
                ? new(true, verticalReflection)
                : new(false, mirrorMap.FindHorizontalReflection(except));
    }

    private static int FindVerticalReflection(this MirrorMap mirrorMap, Reflection? except = null) {
        return Range(0, mirrorMap.GetLength(1))
                .Where(reflectionPoint => Range(0, mirrorMap.GetLength(0))
                        .All(row => mirrorMap.EnumerateRow(row).ReflectsAt(reflectionPoint))
                )
                .Where(reflectionPoint => except is not { IsVertical: true }
                                || reflectionPoint != except.Value.ReflectionPoint)
                .FirstOrDefault(-1);
    }

    private static int FindHorizontalReflection(this MirrorMap mirrorMap, Reflection? except = null) {
        return Range(0, mirrorMap.GetLength(0))
                .Where(reflectionPoint => Range(0, mirrorMap.GetLength(1))
                        .All(col => mirrorMap.EnumerateCol(col).ReflectsAt(reflectionPoint))
                )
                .Where(reflectionPoint => except == null
                        || except.Value.IsVertical
                        || reflectionPoint != except.Value.ReflectionPoint)
                .FirstOrDefault(-1);
    }
}

public readonly record struct Reflection(bool IsVertical, int ReflectionPoint);
