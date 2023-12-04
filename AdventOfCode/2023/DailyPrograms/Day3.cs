using System.Text.RegularExpressions;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day3 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        IList<string> inputLines = inputRepository.FetchLines(inputRef);
        int rowCount = inputLines.Count;
        int colCount = inputLines[0].Length;
        var schematic = new char[rowCount, colCount];
        for (var row = 0; row < rowCount; row++) {
            for (var col = 0; col < colCount; col++) {
                schematic[row, col] = inputLines[row][col];
            }
        }
        bool[,] validPartNumberPositions = schematic
                .ApplyMask(IsSymbol)
                .ApplyMask(IsAdjacentToTrue);


        int sumOfValidPartNumbers = inputLines
                .SelectMany((line, row) => Regex.Matches(line, @"(\d+)")
                        .Select(match => new { Row = row, Match = match }))
                .Select(obj => new {
                        obj.Row,
                        Col = obj.Match.Index,
                        Value = int.Parse(obj.Match.Value),
                        obj.Match.Length,
                })
                .Where(obj => IsPartNumber(validPartNumberPositions, obj.Row, obj.Col, obj.Length))
                .Select(obj => obj.Value)
                .Sum();
        Console.WriteLine($"Sum of valid part numbers: {sumOfValidPartNumbers}");
    }

    private static bool IsPartNumber(bool[,] validPartNumberPositions, int row, int col, int length) {
        return Enumerable.Range(col, length).Any(c => validPartNumberPositions[row, c]);
    }

    private static bool IsSymbol(char[,] cArray, int row, int col) {
        char c = cArray[row, col];
        return !char.IsLetterOrDigit(c) && c != '.';
    }

    private static bool IsAdjacentToTrue(bool[,] bArray, int row, int col) {
        for (int r = Math.Max(row - 1, 0); r <= Math.Min(row + 1, bArray.GetLength(0) - 1); r++) {
            for (int c = Math.Max(col - 1, 0); c <= Math.Min(col + 1, bArray.GetLength(1) - 1); c++) {
                if (bArray[r, c]) {
                    return true;
                }
            }
        }
        return false;
    }
}