﻿// See https://aka.ms/new-console-template for more information


using System;
using System.Collections.Generic;
using System.Diagnostics;
using kirypto.AdventOfCode._2023.DailyPrograms;
using kirypto.AdventOfCode.Common.Interfaces;
using kirypto.AdventOfCode.Common.Repositories;

IInputRepository inputRepository = new FileInputRepository();

IDictionary<int, IDailyProgram> dayFunctions = new Dictionary<int, IDailyProgram> {
        { 1, new Day1() },
        { 2, new Day2() },
        { 3, new Day3() },
        { 4, new Day4() },
        { 5, new Day5() },
        { 6, new Day6() },
        { 7, new Day7() },
        { 8, new Day8() },
        { 9, new Day9() },
        { 10, new Day10() },
        { 11, new Day11() },
        { 12, new Day12() },
        { 13, new Day13() },
        { 14, new Day14() },
        { 15, new Day15() },
        { 16, new Day16() },
        { 17, new Day17() },
        { 18, new Day18() },
        { 19, new Day19() },
        { 20, new Day20() },
        { 21, new Day21() },
        { 22, new Day22() },
        { 23, new Day23() },
        { 24, new Day24() },
};

Console.Write("Day: ");
if (!int.TryParse(Console.ReadLine(), out int day)) {
    day = (DateTime.Now + TimeSpan.FromHours(1)).Day;
    Console.WriteLine($"  -> Defaulting to {day}");
}
Console.Write("Part: ");
int part = int.Parse(Console.ReadLine() ?? "");

Console.WriteLine("\n+==============\n Sample \n+==============");
Stopwatch stopwatch = Stopwatch.StartNew();
dayFunctions[day].Run(inputRepository, $"day{day}-part{part}", part);
stopwatch.Stop();
Console.WriteLine($"[[Run time: {stopwatch.ElapsedMilliseconds}ms]]");

Console.WriteLine("\n+==============\n  Real  \n+==============");
stopwatch.Restart();
dayFunctions[day].Run(inputRepository, $"day{day}-real", part);
stopwatch.Stop();
Console.WriteLine($"[[Run time: {stopwatch.ElapsedMilliseconds}ms]]");
