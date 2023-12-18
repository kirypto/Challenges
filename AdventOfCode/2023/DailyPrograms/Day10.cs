using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Models;
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
        PipeMapExtensions.InitializeSDirections(
                startRow > 0 && "|7F".Contains(pipeMap[startRow - 1, startCol]),
                startRow < rowCount - 1 && "|JL".Contains(pipeMap[startRow + 1, startCol]),
                startCol > 0 && "-FL".Contains(pipeMap[startRow, startCol - 1]),
                startCol < colCount && "-J7".Contains(pipeMap[startRow, startCol + 1])
        );

        var visited = new bool[rowCount, colCount];
        var distanceFromStart = new int[rowCount, colCount];
        var explorationQueue = new ExplorationQueue { new(new Position(startRow, startCol), 0) };
        int maxDistanceOnLoop = -1;
        while (explorationQueue.Any()) {
            ((int row, int col), int stepsSoFar) = explorationQueue.DeleteMin();
            if (visited[row, col]) {
                continue;
            }
            maxDistanceOnLoop = Math.Max(maxDistanceOnLoop, stepsSoFar);
            visited[row, col] = true;
            distanceFromStart[row, col] = stepsSoFar;
            pipeMap.GetSuccessors(new Position(row, col))
                    .Where(position => !visited[position.Row, position.Col])
                    .ForEach(successor => explorationQueue.Add(new TraveledPosition(successor, stepsSoFar + 1)));
        }
        Console.WriteLine($"Max distance on loop from creature is {maxDistanceOnLoop}");
    }
}

public readonly record struct TraveledPosition(Position Position, int Steps) : IComparable<TraveledPosition> {
    public int CompareTo(TraveledPosition other) {
        return Steps.CompareTo(other.Steps);
    }
}

public static class PipeMapExtensions {
    private static readonly ISet<char> _leftChars = "-J7".ToHashSet();
    private static readonly ISet<char> _rightChars = "-LF".ToHashSet();
    private static readonly ISet<char> _upChars = "|LJ".ToHashSet();
    private static readonly ISet<char> _downChars = "|7F".ToHashSet();

    public static void InitializeSDirections(bool up, bool down, bool left, bool right) {
        _upChars.Remove('S');
        _downChars.Remove('S');
        _leftChars.Remove('S');
        _rightChars.Remove('S');
        if (up) _upChars.Add('S');
        if (down) _downChars.Add('S');
        if (left) _leftChars.Add('S');
        if (right) _rightChars.Add('S');
    }

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
// #1 6797 -> too low
