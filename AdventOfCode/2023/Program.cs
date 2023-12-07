// See https://aka.ms/new-console-template for more information


using System;
using System.Collections.Generic;
using kirypto.AdventOfCode._2023.DailyPrograms;
using kirypto.AdventOfCode._2023.Repos;

Console.WriteLine("Hello, World!");

IInputRepository inputRepository = new FileInputRepository();

IDictionary<int, IDailyProgram> dayFunctions = new Dictionary<int, IDailyProgram> {
        { 1, new Day1() },
        { 2, new Day2() },
        { 3, new Day3() },
        { 4, new Day4() },
        { 5, new Day5() },
        { 6, new Day6() },
        { 7, new Day7() },
};

Console.Write("Day: ");
if (!int.TryParse(Console.ReadLine(), out int day)) {
    day = (DateTime.Now + TimeSpan.FromHours(1)).Day;
    Console.WriteLine($"  -> Defaulting to {day}");
}
Console.Write("Part: ");
int part = int.Parse(Console.ReadLine() ?? "");

Console.WriteLine("\n+==============\n| Sample\n+==============");
dayFunctions[day].Run(inputRepository, $"day{day}-part{part}", part);
Console.WriteLine("\n+==============\n| Real\n+==============");
dayFunctions[day].Run(inputRepository, $"day{day}-real", part);