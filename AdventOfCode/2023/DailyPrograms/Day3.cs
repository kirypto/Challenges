using System.Text.RegularExpressions;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day3 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        IList<string> inputLines = inputRepository.FetchLines(inputRef);
        int rowCount = inputLines.Count;
        int colCount = inputLines[0].Length;
        var schematicData = new char[rowCount, colCount];
        for (var row = 0; row < rowCount; row++) {
            for (var col = 0; col < colCount; col++) {
                schematicData[row, col] = inputLines[row][col];
            }
        }

        var schematic = new Schematic(schematicData);

        if (part == 1) {
            int sumOfPartNumbers = schematic.PartNumbers
                    .Select(partNumber => partNumber.Number)
                    .Sum();
            Console.WriteLine($"Sum of valid part numbers: {sumOfPartNumbers}");
        } else {
            throw new NotImplementedException("Part 2 not implemented");
        }
    }
}

public readonly record struct PartNumber(int Number, int Row, int Col, int Length);

public record struct Schematic(char[,] Data) {
    private IList<PartNumber>? _partNumbers = null;
    public IList<PartNumber> PartNumbers => _partNumbers ??= DerivePartNumbers(Data);

    private static IList<PartNumber> DerivePartNumbers(char[,] Data) {
        bool[,] validPartNumberPositions = Data
                .ApplyMask(IsSymbol)
                .ApplyMask(IsAdjacentToTrue);

        return Enumerable.Range(0, Data.GetLength(0))
                .Select(Data.GetRow)
                .Select(dataRow => string.Join("", dataRow))
                .SelectMany((line, row) => Regex.Matches(line, @"(\d+)")
                        .Select(match => new { Row = row, Match = match }))
                .Select(obj => new {
                        obj.Row,
                        Col = obj.Match.Index,
                        Value = int.Parse(obj.Match.Value),
                        obj.Match.Length,
                })
                .Where(obj => IsPartNumber(validPartNumberPositions, obj.Row, obj.Col, obj.Length))
                .Select(obj => new PartNumber(obj.Value, obj.Row, obj.Col, obj.Length))
                .ToList();
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