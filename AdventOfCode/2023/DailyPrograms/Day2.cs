using kirypto.AdventOfCode._2023.Repos;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day2 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        IList<string> lines = inputRepository.FetchLines(inputRef);
        var sum = 0;
        foreach (string line in lines) {
            string l = line.TrimStart("Game ".ToCharArray());
            IList<string> gameIdAndDraws = l.Split(": ");
            int gameId = int.Parse(gameIdAndDraws[0]);
            string allDraws = gameIdAndDraws[1];

            IList<string> draws = allDraws.Split("; ");


            var maxObservedCounts = new Dictionary<string, int>();
            var gameIsPossible = true;
            foreach (string draw in draws) {
                foreach (string drawCount in draw.Split(", ")) {
                    string[] s = drawCount.Split(" ");
                    int count = int.Parse(s[0]);
                    string colour = s[1];
                    if (_p1MaxCounts.ContainsKey(colour) && _p1MaxCounts[colour] < count) {
                        gameIsPossible = false;
                    }
                    if (!maxObservedCounts.ContainsKey(colour)) {
                        maxObservedCounts[colour] = count;
                    } else {
                        maxObservedCounts[colour] = Math.Max(maxObservedCounts[colour], count);
                    }
                }
            }

            if (part == 1 && gameIsPossible) {
                sum += gameId;
            } else if (part == 2) {
                int cubePower = maxObservedCounts.Values.Aggregate((c1, c2) => c1 * c2);
                // Console.WriteLine($"Game {gameId} cube power: {cubePower}");
                sum += cubePower;
            }
        }
        if (part == 1) {
            Console.WriteLine($"Sum of possible game IDs: {sum}");
        } else if (part == 2) {
            Console.WriteLine($"Sum of possible cube power: {sum}");
        }
    }

    private static readonly IDictionary<string, int> _p1MaxCounts = new Dictionary<string, int> {
            { "red", 12 },
            { "green", 13 },
            { "blue", 14 },
    };
}