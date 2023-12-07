using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day7 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        List<Hand> hands = inputRepository.FetchLines(inputRef)
                .Select(inputLine => inputLine.Split(" "))
                .Select(cardsAndBid => new Hand(cardsAndBid[0], long.Parse(cardsAndBid[1])))
                .OrderDescending()
                .ToList();
        hands.Select(hand => hand.ToString()).ForEach(Console.WriteLine);

        throw new NotImplementedException();
    }
}

public readonly record struct Hand(string Cards, long Bid, long Rank) : IComparable<Hand> {
    public Hand(string Cards, long Bid) : this(Cards, Bid, DeriveRank(Cards, Bid)) { }

    private static long DeriveRank(string cards, long bid) {
        return -1;
    }

    public int CompareTo(Hand other) {
        return Rank.CompareTo(other.Rank);
    }
}