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

            if (AreDrawsPossible(allDraws)) {
                sum += gameId;
            }
        }
        Console.WriteLine($"Sum of possible game IDs: {sum}");
    }

    private static bool AreDrawsPossible(string allDraws) {
        IList<string> draws = allDraws.Split("; ");

        foreach (string draw in draws) {
            foreach (string drawCount in draw.Split(", ")) {
                string[] s = drawCount.Split(" ");
                int count = int.Parse(s[0]);
                string colour = s[1];
                if (_maxCounts.ContainsKey(colour) && _maxCounts[colour] < count) {
                    return false;
                }
            }
        }
        return true;
    }

    private static readonly IDictionary<string, int> _maxCounts = new Dictionary<string, int> {
            { "red", 12 },
            { "green", 13 },
            { "blue", 14 },
    };
}