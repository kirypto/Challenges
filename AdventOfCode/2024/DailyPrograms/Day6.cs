using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Interfaces;
using Microsoft.Extensions.Logging;
using static kirypto.AdventOfCode._2024.DailyPrograms.CardinalDirection;
using static kirypto.AdventOfCode.Common.Services.IO.DailyProgramLogger;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

[DailyProgram(6)]
public class Day6 : IDailyProgram {
    public string Run(IInputRepository inputRepository, string inputRef, int part) {
        Logger.LogInformation("Day6");
        IList<string> mapLines = inputRepository.FetchLines();
        int rowCount = mapLines.Count;
        int colCount = mapLines[0].Length;

        Cell[,] map = new Cell[rowCount, colCount];
        DirectedCoord position = new(new Coord(0, 0), North);
        for (int row = 0; row < rowCount; row++) {
            for (int col = 0; col < colCount; col++) {
                char cellChar = mapLines[row][col];
                map[row, col] = cellChar switch {
                        '.' => new Cell { Obstacle = false },
                        '^' => new Cell { Obstacle = false },
                        '#' => new Cell { Obstacle = true },
                        _ => throw new ArgumentException($"Invalid cell char '{cellChar}'"),
                };
                if (cellChar == '^') {
                    position = position with { Coord = new Coord(col, row) };
                }
            }
        }
        Logger.LogInformation($"Initial position: {position}");

        (ExitType _, ISet<DirectedCoord> visited) = SimulateGuard(map, position);
        if (part == 1) {
            return visited.Select(pos => pos.Coord).Distinct().Count().ToString();
        }
        HashSet<Coord> loopingObstacles = [];
        int counter = 0;
        foreach (Coord obstaclePosition in visited
                         .Select(pos => pos.Coord)
                         .Distinct()
                         .Except([position.Coord])) {
            Cell[,] modifiedMap = CloneWithObstacleAt(map, obstaclePosition);
            counter++;
            Logger.LogInformation($"Checking loop position {counter} of {visited.Count}");
            (ExitType exit, ISet<DirectedCoord> _) = SimulateGuard(modifiedMap, position);
            if (exit is ExitType.Loop) {
                loopingObstacles.Add(obstaclePosition);
            }
        }
        return loopingObstacles.Count.ToString();
    }

    private static (ExitType exit, ISet<DirectedCoord> visited) SimulateGuard(Cell[,] map, DirectedCoord position) {
        int rowCount = map.GetLength(0);
        int colCount = map.GetLength(1);
        HashSet<DirectedCoord> visited = [];
        while (IsWithinBounds(position.Coord, rowCount, colCount)) {
            if (!visited.Add(position)) {
                return (ExitType.Loop, visited);
            }

            DirectedCoord next = position.MoveForward();
            if (IsWithinBounds(next.Coord, rowCount, colCount) && map[next.Y, next.X].Obstacle) {
                // Rotate
                position = position.Rotate90Clockwise();
            } else {
                // Move Forward
                position = next;
            }

        }
        return (ExitType.Escape, visited);
    }

    private static Cell[,] CloneWithObstacleAt(Cell[,] map, Coord coord) {
        Cell[,] modifiedMap = (Cell[,])map.Clone();
        modifiedMap[coord.Y, coord.X] = new Cell { Obstacle = true };
        return modifiedMap;
    }

    private static bool IsWithinBounds(Coord position, int rowCount, int colCount) {
        return position is { X: >= 0, Y: >= 0 } && position.X < colCount && position.Y < rowCount;
    }

    private enum ExitType {
        Escape,
        Loop,
    }
}

public readonly record struct Coord(int X, int Y);

public readonly record struct DirectedCoord(Coord Coord, CardinalDirection Direction) {
    public int Y => Coord.Y;
    public int X => Coord.X;

    public DirectedCoord MoveForward() {
        return this with { Coord = Coord.Move(Direction) };
    }

    public DirectedCoord Rotate90Clockwise() {
        return this with { Direction = Direction.Rotate90Clockwise() };
    }
}

public static class CoordExtensions {
    public static Coord Move(this Coord coord, CardinalDirection direction) {
        return direction switch {
                North => coord with { Y = coord.Y - 1 },
                East => coord with { X = coord.X + 1 },
                South => coord with { Y = coord.Y + 1 },
                West => coord with { X = coord.X - 1 },
                _ => throw new ArgumentException($"Unsupported direction {direction}", nameof(direction)),
        };
    }
}

public enum CardinalDirection {
    North,
    East,
    South,
    West,
}

public static class CardinalDirectionExtensions {
    public static CardinalDirection Rotate90Clockwise(this CardinalDirection direction) {
        return direction switch {
                North => East,
                East => South,
                South => West,
                West => North,
                _ => throw new ArgumentException($"Unsupported direction {direction}", nameof(direction)),
        };
    }
}

internal record struct Cell(bool Obstacle);
