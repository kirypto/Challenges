using System;
using System.Linq;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day7 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        long totalWinnings = inputRepository.FetchLines(inputRef)
                .Select(inputLine => inputLine.Split(" "))
                .Select(cardsAndBid => new Hand(cardsAndBid[0], long.Parse(cardsAndBid[1])))
                .Order()
                .Tap(hand => Console.Write($"--> {hand}: "))
                .Select((hand, valueRank) => hand.Bid * valueRank)
                .Tap(Console.WriteLine)
                .Aggregate((w1, w2) => w1 + w2);

        Console.WriteLine($"Total Winnings: {totalWinnings}");
        throw new NotImplementedException();
    }
}

public readonly record struct Hand(string Cards, long Bid, long Rank) : IComparable<Hand> {
    public Hand(string Cards, long Bid) : this(Cards, Bid, DeriveRank(Cards, Bid)) { }

    public int CompareTo(Hand other) {
        return Rank.CompareTo(other.Rank);
    }

    private static long DeriveRank(string cards, long bid) {
        return -1;
    }
}
