using System;
using kirypto.AdventOfCode.Common.Attributes;
using kirypto.AdventOfCode.Common.Interfaces;
using Microsoft.Extensions.Logging;
using static kirypto.AdventOfCode.Common.Services.IO.DailyProgramLogger;

namespace kirypto.AdventOfCode._2024.DailyPrograms;

[DailyProgram(6)]
public class Day6 : IDailyProgram {
    public string Run(IInputRepository inputRepository, string inputRef, int part) {
        Logger.LogInformation("Day6");
        var mapLines = inputRepository.FetchLines(inputRef);
        int rowCount = mapLines.Count;
        int colCount = mapLines[0].Length;

        var map = new Cell[rowCount, colCount];
        Coord position = new Coord(0, 0);
        CardinalDirection direction = CardinalDirection.North;
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
                    position = new Coord(col, row);
                }
            }
        }
        Logger.LogInformation($"Initial position: {position}");

        int distinctVisitCellCount = 0;
        int possibleLoopCounts = 0;
        while (IsWithinBounds(position, rowCount, colCount)) {
            Logger.LogInformation($"Current: {position} {direction}");

            if (map[position.Y, position.X].WasVisited()) {
                distinctVisitCellCount++;
            }
            map[position.Y, position.X].SetVisited(direction);

            Coord nextPosition = position.Move(direction);
            if (!IsWithinBounds(nextPosition, rowCount, colCount)) {
                Logger.LogInformation("Found edge");
                break;
            }

            if (CouldMakeLoop(position, nextPosition, direction, map)) {
                Logger.LogInformation("Found loop");
                possibleLoopCounts++;
            }
            Cell nextCell = map[nextPosition.Y, nextPosition.X];
            if (nextCell.Obstacle) {
                Logger.LogInformation("Found obstacle");
                direction = direction.Rotate90Clockwise();
                continue;
            }
            position = nextPosition;
        }
        Logger.LogInformation($"Part 1: {distinctVisitCellCount}, part 2: {possibleLoopCounts}");
        return (part == 1 ? distinctVisitCellCount : possibleLoopCounts).ToString();
    }

    private static bool CouldMakeLoop(Coord position, Coord nextPosition, CardinalDirection direction, Cell[,] map) {
        bool nextIsObstacle = map[nextPosition.Y, nextPosition.X].Obstacle;
        CardinalDirection directionIfTurned = direction.Rotate90Clockwise();
        bool wouldBeLoopIfTurnedInPlace = map[position.Y, position.X].WasVisited(directionIfTurned);
        return wouldBeLoopIfTurnedInPlace && !nextIsObstacle;
    }

    private static bool IsWithinBounds(Coord position, int rowCount, int colCount) {
        return position.X >= 0 && position.Y >= 0 && position.X < colCount && position.Y < rowCount;
    }
}

public readonly record struct Coord(int X, int Y);

public static class CoordExtensions {
    public static Coord Move(this Coord coord, CardinalDirection direction) {
        return direction switch {
                CardinalDirection.North => coord with { Y = coord.Y - 1 },
                CardinalDirection.East => coord with { X = coord.X + 1 },
                CardinalDirection.South => coord with { Y = coord.Y + 1 },
                CardinalDirection.West => coord with { X = coord.X - 1 },
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
                CardinalDirection.North => CardinalDirection.East,
                CardinalDirection.East => CardinalDirection.South,
                CardinalDirection.South => CardinalDirection.West,
                CardinalDirection.West => CardinalDirection.North,
                _ => throw new ArgumentException($"Unsupported direction {direction}", nameof(direction)),
        };
    }
}

internal record struct Cell(
        bool Obstacle,
        bool VisitedFacingNorth,
        bool VisitedFacingEast,
        bool VisitedFacingSouth,
        bool VisitedFacingWest) {
    internal bool WasVisited(CardinalDirection? facing = null) {
        return facing switch {
                CardinalDirection.North => VisitedFacingNorth,
                CardinalDirection.East => VisitedFacingEast,
                CardinalDirection.South => VisitedFacingSouth,
                CardinalDirection.West => VisitedFacingWest,
                _ => VisitedFacingNorth || VisitedFacingEast || VisitedFacingSouth || VisitedFacingSouth,
        };
    }

    internal void SetVisited(CardinalDirection facing) {
        switch (facing) {
            case CardinalDirection.North:
                VisitedFacingNorth = true;
                break;
            case CardinalDirection.East:
                VisitedFacingEast = true;
                break;
            case CardinalDirection.South:
                VisitedFacingSouth = true;
                break;
            case CardinalDirection.West:
                VisitedFacingWest = true;
                break;
        }
    }
}
