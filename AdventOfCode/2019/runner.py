import pathlib
import sys
from importlib import import_module
from typing import List

from python_tools.advent_of_code_lib import AdventOfCodeProblem


def read_puzzle_input(day_name: str) -> List[str]:
    input_file_path = pathlib.Path(f"{day_name}_input.txt").absolute()
    with open(input_file_path, mode="r") as input_file:
        puzzle_input = input_file.readlines()
    return puzzle_input


if __name__ != '__main__':
    raise RuntimeError(f"Script {__file__} should be ran as main, not imported")

if len(sys.argv) == 2:
    puzzle_day = sys.argv[1]
else:
    puzzle_day = f"day{input('Puzzle day? ')}"

problem: AdventOfCodeProblem = import_module(puzzle_day).problem

translated_puzzle_input = problem.translate_input(read_puzzle_input(puzzle_day))

print(f"  Part 1 Tests |\n"
      f"---------------+")
for test in problem.part_1_test_cases:
    actual = problem.part_1_solver(test.puzzle_input)
    if test.expected == actual:
        print(" - Test Passed!")
    else:
        print(f" - Test FAILED: Expected={test.expected}, Actual={actual}, Input={test.puzzle_input}")
print(f"  Part 1 Result: {problem.part_1_solver(translated_puzzle_input)}")
print()

print(f"  Part 2 Tests |\n"
      f"---------------+")
for test in problem.part_2_test_cases:
    actual = problem.part_2_solver(test.puzzle_input)
    if test.expected == actual:
        print(" - Test Passed!")
    else:
        print(f" - Test FAILED: Expected={test.expected}, Actual={actual}, Input={test.puzzle_input}")
print(f"  Part 2 Result: {problem.part_2_solver(translated_puzzle_input)}")
