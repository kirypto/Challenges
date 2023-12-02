// See https://aka.ms/new-console-template for more information


using kirypto.AdventOfCode._2023.Repos;
using static kirypto.AdventOfCode._2023.Utils;

Console.WriteLine("Hello, World!");

IInputRepository inputRepository = new FileInputRepository();

IDictionary<int, Action> dayFunctions = new Dictionary<int, Action> {
        { 1, Day1 },
        { 2, Day2 },
};

Console.Write("Day: ");
int day = int.Parse(Console.ReadLine() ?? "-1");
dayFunctions[day]();
return;

void Day1() {
    int calibrationSum = inputRepository.FetchLines("day1-real")
            .Select(ExtractCalibrationValue)
            .Sum();
    Console.WriteLine($"Calibration sum: {calibrationSum}");
}

void Day2() {
    Console.Write("Input: ");
    string inputRef = Console.ReadLine()!;
    Console.WriteLine(inputRepository.Fetch(inputRef));
}