// See https://aka.ms/new-console-template for more information


using kirypto.AdventOfCode._2023.Repos;

Console.WriteLine("Hello, World!");

IInputRepository inputRepository = new FileInputRepository();


Console.WriteLine(inputRepository.Fetch("day1-1"));
