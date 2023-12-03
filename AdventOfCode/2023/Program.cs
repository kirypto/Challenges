// See https://aka.ms/new-console-template for more information


using kirypto.AdventOfCode._2023.DailyPrograms;
using kirypto.AdventOfCode._2023.Repos;

Console.WriteLine("Hello, World!");

IInputRepository inputRepository = new FileInputRepository();

IDictionary<int, IDailyProgram> dayFunctions = new Dictionary<int, IDailyProgram> {
        { 1, new Day1() },
        { 2, new Day2() },
};

Console.Write("Day: ");
if (!int.TryParse(Console.ReadLine(), out int day)) {
    day = (DateTime.Now + TimeSpan.FromHours(1)).Day;
    Console.WriteLine($"  -> Defaulting to {day}");
}
string dayPortion = $"day{day}-";
List<string> inputRefs;
Console.Write("Part: ");
string input = Console.ReadLine() ?? "";
if (input.Length > 0) {
    inputRefs = new List<string> { dayPortion + "part" + input };
} else {
    inputRefs = new List<string> { dayPortion + "part1", dayPortion + "part2", dayPortion + "real" };
}


foreach (string inputRef in inputRefs) {
    try {
        dayFunctions[day].Run(inputRepository, inputRef);
    }
    catch (Exception e) {
        Console.WriteLine($"FAILURE on '{inputRef}': {e.Message}");
    }
}