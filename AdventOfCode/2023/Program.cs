﻿// See https://aka.ms/new-console-template for more information


using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;

Console.WriteLine("Hello, World!");

IInputRepository inputRepository = new FileInputRepository();

int ExtractCalibrationValue(string raw) {
    var replacements = new List<CalibrationDigit> {
            new("0", "0"),
            new("1", "1"),
            new("2", "2"),
            new("3", "3"),
            new("4", "4"),
            new("5", "5"),
            new("6", "6"),
            new("7", "7"),
            new("8", "8"),
            new("9", "9"),
            new("zero", "0"),
            new("one", "1"),
            new("two", "2"),
            new("three", "3"),
            new("four", "4"),
            new("five", "5"),
            new("six", "6"),
            new("seven", "7"),
            new("eight", "8"),
            new("nine", "9"),
    };
    try {
        string mutated = raw;
        string? firstDigit = replacements
                .Where(r => mutated.Contains(r.Raw))
                .MinBy(r => mutated.IndexOf(r.Raw, StringComparison.Ordinal))
                .Normalized;
        string? secondDigit = replacements
                .Where(r => mutated.Reversed().Contains(r.Raw.Reversed()))
                .MinBy(r => mutated.Reversed().IndexOf(r.Raw.Reversed(), StringComparison.Ordinal))
                .Normalized;
        return int.Parse(firstDigit + secondDigit);
    }
    catch (Exception e) {
        Console.WriteLine($"Failed on value '{raw}'");
        Console.WriteLine(e);
        throw;
    }
}


int calibrationSum = inputRepository.FetchLines("day1-real")
        .Select(ExtractCalibrationValue)
        .Sum();
Console.WriteLine($"Calibration sum: {calibrationSum}");


internal readonly record struct CalibrationDigit(string Raw, string Normalized);