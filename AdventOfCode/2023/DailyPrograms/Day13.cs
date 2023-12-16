using System;
using System.Collections.Generic;
using System.Linq;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode._2023.Repos;
using MirrorMap = char[,];

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day13 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        const string test = "234554321";
        Console.WriteLine($"Checking reflections of {test}: ");
        Enumerable.Range(0, test.Length)
                .Tap(i => Console.Write($"  @ {i}: "))
                .Select(i => test.ReflectsAt(i))
                .ForEach(Console.WriteLine);

        throw new NotImplementedException();
        int foo = inputRepository.Fetch(inputRef)
                .Split("\n\n")
                .Select(mirrorMap => mirrorMap.To2DCharArray("\n"))
                .Tap(mirrorMap => mirrorMap.PrintToConsole())
                .Count();
        Console.WriteLine(foo);
    }
}

