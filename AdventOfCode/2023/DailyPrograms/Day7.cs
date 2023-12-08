using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;
using static kirypto.AdventOfCode._2023.DailyPrograms.CardHelpers;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day7 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        long totalWinnings = inputRepository.FetchLines(inputRef)
                .Select(inputLine => inputLine.Split(" "))
                .Select(cardsAndBid => new Hand(cardsAndBid[0], long.Parse(cardsAndBid[1])))
                .Order()
                .Tap(hand => Console.Write($"--> {hand.Cards}: {hand} --> "))
                .Select((hand, valueRank) => hand.Bid * valueRank)
                .Tap(Console.WriteLine)
                .Aggregate((w1, w2) => w1 + w2);

        Console.WriteLine($"Total Winnings: {totalWinnings}");
        throw new NotImplementedException();
    }
}

public readonly record struct Hand(string Cards, long Bid, HandRank HandRank) : IComparable<Hand> {
    public Hand(string Cards, long Bid) : this(Cards, Bid, DeriveHandRank(Cards)) { }

    public int CompareTo(Hand other) {
        return HandRank.CompareTo(other.HandRank);
    }
}

public readonly record struct HandRank(
        bool FiveOfAKind,
        bool FourOfAKind,
        bool FullHouse,
        bool ThreeOfAKind,
        bool TwoPair,
        bool OnePair,
        bool HighCard,
        long Order) : IComparable<HandRank> {
    public int CompareTo(HandRank other) {
        return new[] {
                FiveOfAKind.CompareTo(other.FiveOfAKind),
                FourOfAKind.CompareTo(other.FourOfAKind),
                FullHouse.CompareTo(other.FullHouse),
                ThreeOfAKind.CompareTo(other.ThreeOfAKind),
                TwoPair.CompareTo(other.TwoPair),
                OnePair.CompareTo(other.OnePair),
                HighCard.CompareTo(other.HighCard),
                Order.CompareTo(other.Order),
        }.FirstOrDefault(result => result != 0);
    }
}

public static class CardHelpers {
    // ReSharper disable once StringLiteralTypo
    private static readonly IList<char> SerializedCardFaces = "23456789TJQKA".ToList();

    public static HandRank DeriveHandRank(string cards) {
        Dictionary<char, int> cardCounts = cards
                .GroupBy(card => card)
                .ToDictionary(chars => chars.Key, chars => chars.Count());

        bool fiveOfAKind = cardCounts.Values.Any(count => count == 5);
        bool fourOfAKind = cardCounts.Values.Any(count => count == 4);
        bool threeOfAKind = cardCounts.Values.Any(count => count == 3);
        int pairCounts = cardCounts.Values.Count(count => count == 2);
        bool onePair = pairCounts == 1;
        bool twoPairs = pairCounts == 2;
        bool fullHouse = threeOfAKind && onePair;
        bool highCard = !fiveOfAKind && !fourOfAKind && !fullHouse && !threeOfAKind && !twoPairs && !onePair;

        return new HandRank(fiveOfAKind, fourOfAKind, fullHouse, threeOfAKind, twoPairs, onePair, highCard,
                DeriveOrder(cards));
    }

    private static long DeriveOrder(string cards) {
        return -1;
    }
}