using System;
using System.Collections.Generic;
using System.Linq;
using C5;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Interfaces;
using kirypto.AdventOfCode.Common.Services.IO;
using Microsoft.Extensions.Logging;
using static System.ConsoleColor;
using DiskMap = C5.TreeDictionary<int, int?>;
using DiskMapKeys = C5.ISorted<int>;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(9)]
public class Day09 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        List<int> compressedDiskMap = inputRepository
                .WithFormatter(raw => raw.Replace("\n", ""))
                .Fetch()
                .Select(numericChar => numericChar - '0')
                .ToList();

        DiskMap diskMap = new();
        int nextDiskPosition = 0;
        for (int i = 0; i < compressedDiskMap.Count; i++) {
            bool isFile = i % 2 == 0;
            diskMap[nextDiskPosition] = isFile ? NextFileId : null;
            nextDiskPosition += compressedDiskMap[i];
        }
        diskMap[nextDiskPosition] = null;


        DiskMapKeys blocks = diskMap.Keys!;
        int earliestEmptyBlock = blocks.Successor(0);
        int latestFilledBlock = blocks.Predecessor(nextDiskPosition);
        PrintDiskMap(diskMap, earliestEmptyBlock, latestFilledBlock);
        while (earliestEmptyBlock < latestFilledBlock) {
            int emptyBlockWidth = blocks.Successor(earliestEmptyBlock) - earliestEmptyBlock;
            int filledBlockWidth = blocks.Successor(latestFilledBlock) - latestFilledBlock;
            DailyProgramLogger.Logger.LogInformation("Empty width {empty}, filled width {filled}", emptyBlockWidth, filledBlockWidth);
            diskMap[earliestEmptyBlock] = diskMap[latestFilledBlock];
            if (emptyBlockWidth >= filledBlockWidth) {
                // Move entire block
                DailyProgramLogger.Logger.LogInformation("Moving full block");
                int endOfInsertedBlock = earliestEmptyBlock + filledBlockWidth;
                if (blocks.Successor(earliestEmptyBlock) == endOfInsertedBlock) {
                    // Filled all available space
                    earliestEmptyBlock = blocks.FindSuccessor(earliestEmptyBlock, index => diskMap[index] is null);
                } else {
                    // Filled only part of available space, insert new block
                    diskMap[endOfInsertedBlock] = null;
                    earliestEmptyBlock = endOfInsertedBlock;
                }
                diskMap[latestFilledBlock] = null;
                latestFilledBlock = blocks.FindPredecessor(latestFilledBlock, index => diskMap[index] is not null);
            } else {
                // Move only part to fill available space
                DailyProgramLogger.Logger.LogInformation("Moving partial block");
                earliestEmptyBlock = blocks.FindSuccessor(earliestEmptyBlock, index => diskMap[index] is null);
                diskMap[latestFilledBlock + filledBlockWidth - emptyBlockWidth] = null;
            }
            PrintDiskMap(diskMap, earliestEmptyBlock, latestFilledBlock);
        }

        long checksum = 0;
        List<int> blocksAsList = blocks.ToList();
        for (int index = 0; index < blocksAsList.Count; index++) {
            int blockStart = blocksAsList[index];
            int blockEndExclusive = blocksAsList[index + 1];
            int? value = diskMap[blockStart];
            if (value is null) {
                break;
            }
            checksum += ArithmeticSeriesSum(value.Value, blockStart, blockEndExclusive);
        }
        return checksum.ToString();
    }

    private int _fileId;
    private int NextFileId => _fileId++;


    private static long ArithmeticSeriesSum(long scalar, long seriesStart, long seriesEndExclusive) {
        long n = seriesEndExclusive - seriesStart;
        return scalar * n * (seriesStart + (seriesEndExclusive - 1)) / 2;
    }

    private static void PrintDiskMap(DiskMap diskMap, int earliestEmptyBlock, int latestFilledBlock) {
        if (!Program.IsVerbose || diskMap?.Keys is null) {
            return;
        }

        List<int> diskChangeIndexes = diskMap.Keys.ToList();
        for (int i = 0; i < diskChangeIndexes.Count - 1; i++) {
            int diskIndex = diskChangeIndexes[i];
            int nextChangeIndex = diskChangeIndexes[i + 1];
            int? fileIdOptional = diskMap[diskIndex];
            int currentLength = nextChangeIndex - diskIndex;
            string diskMapStr = fileIdOptional?.ToString() ?? ".";
            Console.ForegroundColor = Green;
            Console.Write(diskMapStr);
            Console.ResetColor();
            Console.Write(string.Concat(Enumerable.Repeat(diskMapStr, currentLength - 1)));
        }
        Console.WriteLine();

        // The below will not work if there are any file ids > 9
        Console.Write(new string(' ', earliestEmptyBlock));
        Console.ForegroundColor = earliestEmptyBlock < latestFilledBlock ? Blue : Magenta;
        Console.Write("^");
        if (earliestEmptyBlock < latestFilledBlock) {
            Console.Write(new string(' ', latestFilledBlock - earliestEmptyBlock - 1));
            Console.ForegroundColor = Red;
            Console.Write("^");
        }
        Console.ResetColor();
        Console.WriteLine();
    }
}

public static class ISortedExtensions {
    public static T FindPredecessor<T>(this ISorted<T> source, T index, Func<T, bool> predicate) {
        while (true) {
            index = source.Predecessor(index);
            if (predicate(index)) {
                return index;
            }
        }
    }

    public static T FindSuccessor<T>(this ISorted<T> source, T index, Func<T, bool> predicate) {
        while (true) {
            index = source.Successor(index);
            if (predicate(index)) {
                return index;
            }
        }
    }
}
