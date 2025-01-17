using System;
using static kirypto.AdventOfCode.Common.Models.CompassDirection;

namespace kirypto.AdventOfCode.Common.Models;

public enum CompassDirection {
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West,
    NorthWest,
}

public static class CompassDirectionExtensions {
    public static CompassDirection[] CardinalDirections => [North, East, West, South];

    public static CompassDirection[] IntermediateDirections => [NorthEast, SouthEast, SouthWest, NorthWest];

    public static CompassDirection Rotate90Clockwise(this CompassDirection direction) {
        return direction switch {
                North => East,
                East => South,
                South => West,
                West => North,
                _ => throw new ArgumentException($"Unsupported direction {direction}", nameof(direction)),
        };
    }

    public static CompassDirection Rotate90Anticlockwise(this CompassDirection direction) {
        return direction switch {
                North => West,
                West => South,
                South => East,
                East => North,
                _ => throw new ArgumentException($"Unsupported direction {direction}", nameof(direction)),
        };
    }
}
