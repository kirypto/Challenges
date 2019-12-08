from typing import Any

from python_tools.advent_of_code.puzzle_runner_helpers import AdventOfCodeProblem, TestCase


def part_1_solver(puzzle_input: Any) -> Any:
    return puzzle_input


def part_2_solver(puzzle_input: Any) -> Any:
    return puzzle_input


def translate_input(puzzle_input_raw: str, part_num: int) -> Any:
    return puzzle_input_raw


part_1_test_cases = [
    TestCase(None, None)
]

part_2_test_cases = [
    TestCase(None, None)
]

problem = AdventOfCodeProblem(
    part_1_test_cases,
    part_1_solver,
    part_2_test_cases,
    part_2_solver,
    translate_input,
    run_tests_only=True
)
