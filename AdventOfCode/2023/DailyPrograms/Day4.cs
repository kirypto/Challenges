using System.Text.RegularExpressions;
using kirypto.AdventOfCode._2023.Repos;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day4 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        double sum = inputRepository.FetchLines(inputRef)
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
                    return new {
                            CardNumber = int.Parse(gameIdAndRest[0]),
                            Winning = winning,
                            Received = received,
                    };
                })
                .Select(obj => obj.Winning.Intersect(obj.Received).Count())
                .Where(winningCount => winningCount > 0)
                .Select(winningCount => Math.Pow(2, winningCount - 1))
                .Sum();
        Console.WriteLine($"Total winning points: {sum}");
    }
}