using System;
using System.Collections.Generic;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day10 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        IList<string> inputLines = inputRepository.FetchLines(inputRef);
        var pipeMap = new char[inputLines.Count, inputLines[0].Length];
        int startRow = -1;
        int startCol = -1;
        for (var row = 0; row < inputLines.Count; row++) {
            for (var col = 0; col < inputLines[0].Length; col++) {
                char mapCell = inputLines[row][col];
                pipeMap[row, col] = mapCell;
                if (mapCell == 'S') {
                    startRow = row;
                    startCol = col;
                }
            }
        }
        pipeMap.PrintToConsole();
        Console.WriteLine($"Starting position of animal: {startRow},{startCol}");
        
        throw new NotImplementedException();
    }
}
