using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;
using ExplorationQueue = C5.IntervalHeap<kirypto.AdventOfCode._2023.DailyPrograms.TraveledPosition>;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day10 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        IList<string> inputLines = inputRepository.FetchLines(inputRef);
        int rowCount = inputLines.Count;
        int colCount = inputLines[0].Length;
        var pipeMap = new char[rowCount, colCount];
        int startRow = -1;
        int startCol = -1;
        for (var row = 0; row < rowCount; row++) {
            for (var col = 0; col < colCount; col++) {
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


        var visited = new bool[rowCount, colCount];
        var distanceFromStart = new int[rowCount, colCount];
        var explorationQueue = new ExplorationQueue { new(new Position(startRow, startCol), 0) };
        while (explorationQueue.Any()) {
            ((int row, int col), int stepsSoFar) = explorationQueue.DeleteMin();
            if (visited[row, col]) {
                continue;
            }
            visited[row, col] = true;
            distanceFromStart[row, col] = stepsSoFar;
            pipeMap.GetSuccessors(new Position(row, col))
                    .ForEach(successor => explorationQueue.Add(new TraveledPosition(successor, stepsSoFar + 1)));
        }
        visited.PrintToConsole(6);
        distanceFromStart.PrintToConsole();
        throw new NotImplementedException();
    }
}

public readonly record struct TraveledPosition(Position Position, int Steps) : IComparable<TraveledPosition> {
    public int CompareTo(TraveledPosition other) {
        return Steps.CompareTo(other.Steps);
    }
}

public static class PipeMapExtensions {
    private static readonly ISet<char> _leftChars = "S-J7".ToHashSet();
    private static readonly ISet<char> _rightChars = "S-LF".ToHashSet();
    private static readonly ISet<char> _upChars = "S|LJ".ToHashSet();
    private static readonly ISet<char> _downChars = "S|7F".ToHashSet();
    public static ISet<Position> GetSuccessors(this char[,] pipeMap, Position position) {
        char current = pipeMap[position.Row, position.Col];
        var successors = new HashSet<Position>();
        if (_leftChars.Contains(current) && position.Col > 0) {
            successors.Add(position with { Col = position.Col - 1 });
        }
        if (_rightChars.Contains(current) && position.Col < pipeMap.GetLength(1) - 1) {
            successors.Add(position with { Col = position.Col + 1 });
        }
        if (_upChars.Contains(current) && position.Row > 0) {
            successors.Add(position with { Row = position.Row - 1 });
        }
        if (_downChars.Contains(current) && position.Row < pipeMap.GetLength(0) - 1) {
            successors.Add(position with { Row = position.Row + 1 });
        }
        return successors;
    }
}
