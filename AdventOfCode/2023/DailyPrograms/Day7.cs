using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;
using static kirypto.AdventOfCode._2023.DailyPrograms.CardHelpers;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day7 : IDailyProgram {
    public static int PART { get; set; } = 1;

    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        PART = part;
        long totalWinnings = inputRepository.FetchLines(inputRef)
                .Select(inputLine => inputLine.Split(" "))
                .Select(cardsAndBid => new Hand(cardsAndBid[0], long.Parse(cardsAndBid[1])))
                .Order()
                // .Tap(hand => Console.Write($"--> {hand.Cards}: "))
                .Select((hand, valueRank) => hand.Bid * (valueRank + 1))
                // .Tap(Console.WriteLine)
                .Aggregate((w1, w2) => w1 + w2);

        Console.WriteLine($"Total Winnings: {totalWinnings}");
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
        double Order) : IComparable<HandRank> {
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
    private static IList<char> SerializedCardFaces => (Day7.PART == 1
                    // ReSharper disable once StringLiteralTypo
                    ? "23456789TJQKA"
                    // ReSharper disable once StringLiteralTypo
                    : "J23456789TQKA"
            ).ToList();

    public static HandRank DeriveHandRank(string cards) {
        int jokerCount = Day7.PART == 1 ? 0 : cards.Count(card => card == 'J');
        Dictionary<char, int> cardCounts = cards
                .Where(card => Day7.PART == 1 || card != 'J')
                .GroupBy(card => card)
                .ToDictionary(chars => chars.Key, chars => chars.Count());

        bool fiveOfAKind = cardCounts.Values.Any(count => count + jokerCount == 5)
                || jokerCount == 5;
        bool fourOfAKind = !fiveOfAKind
                && cardCounts.Values.Any(count => count + jokerCount == 4);
        bool threeOfAKind = !fiveOfAKind && !fourOfAKind
                && cardCounts.Values.Any(count => count + jokerCount == 3);
        int pairCounts;
        if (Day7.PART == 1 || jokerCount == 0) {
            pairCounts = cardCounts.Values.Count(count => count == 2);
        } else if (jokerCount == 1) {
            if (cardCounts.Values.Max() == 1) {
                // Handle all different cards with 1 joker => 1 pair
                pairCounts = 1;
            } else if (cardCounts.Values.Count(count => count == 2) == 2) {
                // Handle 2 pair + 1 joker => full house
                pairCounts = 1;
            } else {
                pairCounts = 0;
            }
        } else {
            pairCounts = 0;
        }
        bool onePair = pairCounts == 1;
        bool twoPairs = pairCounts == 2;
        // if (Day7.PART == 2 && !fiveOfAKind && !fourOfAKind && !threeOfAKind && pairCounts == 0 && cards.Contains('J')) {
        // onePair = true;
        // }
        var fullHouse = false;
        if (threeOfAKind && onePair) {
            fullHouse = true;
            threeOfAKind = false;
            onePair = false;
        }
        bool highCard = !fiveOfAKind && !fourOfAKind && !fullHouse && !threeOfAKind && !twoPairs && !onePair;
        // string a = fiveOfAKind ? "x" : " ";
        // string b = fourOfAKind ? "x" : " ";
        // string c = fullHouse ? "x" : " ";
        // string d = threeOfAKind ? "x" : " ";
        // string e = twoPairs ? "x" : " ";
        // string f = onePair ? "x" : " ";
        // string g = highCard ? "x" : " ";
        // if (jokerCount > 0) {
        //     Console.WriteLine($"{cards}: | {a} | {b} | {c} | {d} | {e} | {f} | {g} |");
        // }

        return new HandRank(fiveOfAKind, fourOfAKind, fullHouse, threeOfAKind, twoPairs, onePair, highCard,
                DeriveOrder(cards));
    }

    private static double DeriveOrder(string handCards) {
        return handCards
                .Select(card => SerializedCardFaces.IndexOf(card))
                .Select((cardValue, cardPositionInHand) => cardValue * Math.Pow(13, 4 - cardPositionInHand))
                .Sum();
    }
}
// 249904293 #3 Too High -> Trying to fix J pair issue
// 249845534 Too High
// 249641748 Too High -> Tried moving J back to regular position in array
// 249640985 #4 -> still wrong, doesn't say high/low
// 249400220
