// See https://aka.ms/new-console-template for more information


using System.Text.RegularExpressions;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;

Console.WriteLine("Hello, World!");

IInputRepository inputRepository = new FileInputRepository();

int ExtractCalibrationValue(string raw) {
    Match match = Regex.Match(raw, @"^[^\d]*(\d).*?(\d)?[^\d]*$");
    string secondDigit = match.Groups[2].Length > 0 ? match.Groups[2].Value : match.Groups[1].Value;
    try {
        return int.Parse($"{match.Groups[1]}{secondDigit}");
    }
    catch (Exception e) {
        Console.WriteLine($"Failed on value '{raw}'");
        Console.WriteLine(e);
        throw;
    }
}


int calibrationSum = inputRepository.FetchLines("day1-1")
        .Select(ExtractCalibrationValue)
        .Sum();
Console.WriteLine($"Calibration sum: {calibrationSum}");