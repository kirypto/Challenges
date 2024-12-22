using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Interfaces;
using static System.ConsoleColor;
using DiskMap = C5.TreeDictionary<int, int?>;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

// ReSharper disable once UnusedType.Global
[DailyProgram(9)]
public class Day09 : IDailyProgram {
    public string Run(IInputRepository inputRepository, int part) {
        List<int> compressedDiskMap = inputRepository.Fetch()
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

        PrintDiskMap(diskMap);
        throw new NotImplementedException();
    }

    private int _fileId;
    private int NextFileId => _fileId++;

    private static void PrintDiskMap(DiskMap diskMap) {
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
    }
}
