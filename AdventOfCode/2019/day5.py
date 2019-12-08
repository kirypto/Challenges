from typing import Any, List

from python_tools.advent_of_code.puzzle_runner_helpers import AdventOfCodeProblem, TestCase
from python_tools.advent_of_code.y2019_int_code_computer import run_int_code_program


def part_1_solver(puzzle_input: List[int]) -> List[int]:
    result = run_int_code_program(puzzle_input)
    return result


def part_2_solver(puzzle_input: Any) -> Any:
    return puzzle_input


def translate_input(puzzle_input_raw: str, part_num: int) -> List[int]:
    return [int(x) for x in puzzle_input_raw.split(",")]


part_1_test_cases = [
    TestCase([6, 0, 4, 0, 99], [3, 0, 4, 0, 99])
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
