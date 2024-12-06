using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using kirypto.AdventOfCode.Common.Interfaces;

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
        } else {
            var resultCardCounts = new Dictionary<int, int>();
            for (int cardId = scratchCards.Count; cardId > 0; cardId--) {
                ScratchCard card = scratchCards[cardId];
                resultCardCounts[cardId] = 1 + Enumerable.Range(cardId + 1, card.WinningCount)
                        .Select(copyCardId => resultCardCounts[copyCardId])
                        .Sum();
            }
            Console.WriteLine($"Total resulting card count: {resultCardCounts.Values.Sum()}");
        }
    }
}

public readonly record struct ScratchCard(int CardNumber, int WinningCount) {
    public int Value => WinningCount == 0 ? 0 : (int)Math.Pow(2, WinningCount - 1);
};