using System;
using static kirypto.AdventOfCode.Common.Models.CardinalDirection;

namespace kirypto.AdventOfCode.Common.Models;

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
