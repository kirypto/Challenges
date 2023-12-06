using System;
using System.Collections.Generic;
using System.Linq;
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
            int gearRatioSum = schematic.Gears
                    .Select(gear => gear.GearRatio)
                    .Sum();
            Console.WriteLine($"Sum of gear ratios: {gearRatioSum}");
        }
    }
}

public readonly record struct Position(int Row, int Col);

public readonly record struct PartNumber(int Number, Position Position) {
    public int Length => Number.ToString().Length;
};

public readonly record struct Gear(PartNumber part1, PartNumber part2) {
    public int GearRatio => part1.Number * part2.Number;
}

public record struct Schematic(char[,] Data) {
    private IList<PartNumber>? _partNumbers = null;
    private IList<Gear>? _gears = null;
    public IList<PartNumber> PartNumbers => _partNumbers ??= DerivePartNumbers();
    public IList<Gear> Gears => _gears ??= DeriveGears();

    private IList<PartNumber> DerivePartNumbers() {
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
                .Select(obj => new PartNumber(obj.Value, new Position(obj.Row, obj.Col)))
                .ToList();
    }

    private IList<Gear> DeriveGears() {
        int rowCount = Data.GetLength(0);
        int colCount = Data.GetLength(1);
        var surroundingPartNumberCounts = new int[rowCount, colCount];
        var surroundingPartNumbers = new Dictionary<Position, IList<PartNumber>>();


        foreach (PartNumber partNumber in PartNumbers) {
            foreach (Position position in GetSurroundingPositions(partNumber, rowCount, colCount)) {
                surroundingPartNumberCounts[position.Row, position.Col]++;
                if (!surroundingPartNumbers.ContainsKey(position)) {
                    surroundingPartNumbers[position] = new List<PartNumber>();
                }
                surroundingPartNumbers[position].Add(partNumber);
            }
        }

        var gears = new List<Gear>();
        for (var row = 0; row < rowCount; row++) {
            for (var col = 0; col < colCount; col++) {
                if (surroundingPartNumberCounts[row, col] == 2 && Data[row, col] == '*') {
                    var position = new Position(row, col);
                    IList<PartNumber> adjacentPartNumbers = surroundingPartNumbers[position];
                    gears.Add(new Gear(adjacentPartNumbers[0], adjacentPartNumbers[1]));
                }
            }
        }

        return gears;
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

    private static IEnumerable<Position> GetSurroundingPositions(PartNumber partNumber, int rowCount, int colCount) {
        (int row, int col) = partNumber.Position;
        var positions = new List<Position>();
        if (row - 1 >= 0) {
            Enumerable.Range(col - 1, partNumber.Length + 2)
                    .Where(c => 0 <= c && c < colCount)
                    .ForEach(c => positions.Add(new Position(row - 1, c)));
        }
        if (col > 0) {
            positions.Add(new Position(row, col - 1));
        }
        if (col < colCount - partNumber.Length - 1) {
            positions.Add(new Position(row, col + partNumber.Length));
        }
        if (row + 1 < rowCount) {
            Enumerable.Range(col - 1, partNumber.Length + 2)
                    .Where(c => 0 <= c && c < colCount)
                    .ForEach(c => positions.Add(new Position(row + 1, c)));
        }
        return positions;
    }
}