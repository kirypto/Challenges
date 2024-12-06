using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using kirypto.AdventOfCode._2023.Extensions;
using kirypto.AdventOfCode.Common.Interfaces;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public class Day15 : IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef, int part) {
        string input = inputRepository.Fetch(inputRef);
        if (part == 1) {
            int hashSum = input
                    .Trim()
                    .Split(",")
                    // .Tap(step => Console.Write($"Step '{step}' becomes: "))
                    .Select(step => step.Aggregate(0, (currVal, stepChar) => ((currVal + stepChar) * 17) % 256))
                    // .Tap(Console.WriteLine)
                    .Sum();
            Console.WriteLine($"Sum of hashes: {hashSum}");
        } else {
            List<OrderedDictionary> boxes = Enumerable.Range(0, 256)
                    .Select(_ => new OrderedDictionary())
                    .ToList();
            input
                    .Trim()
                    .Split(",")
                    .Select(Instruction.Parse)
                    // .Tap(instruction => Console.WriteLine(instruction))
                    .ForEach(instruction => {
                        if (instruction.Operation == '-') {
                            boxes[instruction.HASH].Remove(instruction.Label);
                        } else {
                            boxes[instruction.HASH][instruction.Label] = instruction.FocalLength!.Value;
                        }
                    });


            var totalFocusingPower = 0;
            for (var boxIndex = 0; boxIndex < 256; boxIndex++) {
                if (boxes[boxIndex].Count > 0) {
                    // Console.Write($"Box {boxIndex}:  ");
                    var slotNumber = 1;
                    foreach (DictionaryEntry entry in boxes[boxIndex]) {
                        var focalLength = (int)entry.Value!;
                        int focusingPower = (boxIndex + 1) * slotNumber * focalLength;
                        // Console.Write($"{entry.Key}={focalLength}:{focusingPower}  ");
                        totalFocusingPower += focusingPower;
                        slotNumber++;
                    }
                    // Console.WriteLine();
                }
            }

            Console.WriteLine($"Total focusing power: {totalFocusingPower}");
        }
    }
}

public readonly record struct Instruction(string Label, char Operation, int? FocalLength) {
    public static Instruction Parse(string raw) {
        if (raw.Contains('=')) {
            string[] splits = raw.Split("=");
            return new Instruction(splits[0], '=', int.Parse(splits[1]));
        }
        return new Instruction(raw.Split("-")[0], '-', null);
    }

    public int HASH => Label.HASH();
}

public static class HolidayAsciiStringHelpers {
    public static int HASH(this string s) {
        return s.Aggregate(0, (currVal, stepChar) => ((currVal + stepChar) * 17) % 256);
    }
}
