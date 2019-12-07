from typing import Any

from python_tools.advent_of_code_lib import AdventOfCodeProblem, TestCase


def part_1_solver(puzzle_input: Any) -> Any:
    return puzzle_input


def part_2_solver(puzzle_input: Any) -> Any:
    return puzzle_input


def translate_input(puzzle_input_raw: Any) -> Any:
    return puzzle_input_raw


part_1_test_cases = [
    TestCase(111111, (111100, 1111200)),
    TestCase(None, (223450, 223454)),  # Example was 223450 should not be returned
    TestCase(None, (123789, 123798)),  # Example was 123789 should not be returned
]

part_2_test_cases = [
    TestCase(None, None)
]

problem = AdventOfCodeProblem(
    part_1_test_cases,
    part_1_solver,
    part_2_test_cases,
    part_2_solver,
    translate_input
)
