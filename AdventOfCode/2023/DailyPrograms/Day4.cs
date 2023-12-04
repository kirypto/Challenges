using System.Text.RegularExpressions;
using kirypto.AdventOfCode._2023.Repos;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day4 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        Dictionary<int, ScratchCard> scratchCards = inputRepository.FetchLines(inputRef)
                .Select(line => line.Replace("Card ", ""))
                .Select(line => Regex.Replace(line, @"\s+", " "))
                .Select(line => {
                    string[] gameIdAndRest = line.Split(": ");
                    string[] winningAndReceived = gameIdAndRest[1].Split(" | ");
                    ISet<int> winning = winningAndReceived[0].Split(" ")
                            .Select(int.Parse)
                            .ToHashSet();
                    ISet<int> received = winningAndReceived[1].Split(" ")
                            .Select(int.Parse)
                            .ToHashSet();
                    return new ScratchCard(
                            int.Parse(gameIdAndRest[0]),
                            winning.Intersect(received).Count()
                    );
                })
                .ToDictionary(card => card.CardNumber);

        if (part == 1) {
            double sum = scratchCards.Values
                    .Select(card => card.Value)
                    .Sum();
            Console.WriteLine($"Total winning points: {sum}");
        }
    }
}

public readonly record struct ScratchCard(int CardNumber, int WinningCount) {
    public int Value => WinningCount == 0 ? 0 : (int)Math.Pow(2, WinningCount - 1);
};