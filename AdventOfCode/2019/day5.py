from typing import Any, List

from python_tools.advent_of_code.puzzle_runner_helpers import AdventOfCodeProblem, TestCase
from python_tools.advent_of_code.y2019_int_code_computer import run_int_code_program


def part_1_solver(puzzle_input: List[int]) -> List[int]:
    result = run_int_code_program(puzzle_input)
    return result


def part_2_solver(puzzle_input: Any) -> Any:
    result = run_int_code_program(puzzle_input)
    return result


def translate_input(puzzle_input_raw: str, part_num: int) -> List[int]:
    return [int(x) for x in puzzle_input_raw.split(",")]


part_1_test_cases = [
    TestCase([6, 0, 4, 0, 99], [3, 0, 4, 0, 99])  # Enter 6 when asked, should set position 0 to 6
]

part_2_test_cases = [
    # Enter 7 when asked, should set position 9 to 1 because 7 is less than 8
    TestCase([3, 9, 8, 9, 10, 9, 4, 9, 99, 0, 8], [3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8]),
    # Enter 8 when asked, should set position 9 to 1 because 8 equals 8
    TestCase([3, 9, 7, 9, 10, 9, 4, 9, 99, 0, 8], [3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8]),
    # Enter 8 when asked, should set position 3 to 0 because 8 is NOT less than 8
    TestCase([3, 3, 1108, 1, 8, 3, 4, 3, 99], [3, 3, 1108, -1, 8, 3, 4, 3, 99]),
    # Enter 7 when asked, should set position 3 to 0 because 7 is NOT equal to 8
    TestCase([3, 3, 1107, 1, 8, 3, 4, 3, 99], [3, 3, 1107, -1, 8, 3, 4, 3, 99]),
]

problem = AdventOfCodeProblem(
    part_1_test_cases,
    part_1_solver,
    part_2_test_cases,
    part_2_solver,
    translate_input,
    run_tests_only=True
)
