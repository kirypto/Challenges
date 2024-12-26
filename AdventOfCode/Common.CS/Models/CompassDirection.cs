using System;
using static kirypto.AdventOfCode.Common.Models.CompassDirection;

namespace kirypto.AdventOfCode.Common.Models;

public enum CompassDirection {
    North,
    East,
    South,
    West,
}

public static class CompassDirectionExtensions {
    public static CompassDirection Rotate90Clockwise(this CompassDirection direction) {
        return direction switch {
                North => East,
                East => South,
                South => West,
                West => North,
                _ => throw new ArgumentException($"Unsupported direction {direction}", nameof(direction)),
        };
    }
}
