from typing import List, Tuple

from python_tools.advent_of_code.puzzle_runner_helpers import AdventOfCodeProblem, TestCase
from python_tools.advent_of_code.y2019_int_code_computer import run_int_code_program, add_buffered_input, get_buffered_output


def part_1_solver(puzzle_input: Tuple[List[int], List[int]]) -> List[int]:
    program, to_be_buffered = puzzle_input
    for val in to_be_buffered:
        add_buffered_input(val)
    run_int_code_program(program)
    program_output = get_buffered_output()
    return program_output


def part_2_solver(puzzle_input:Tuple[List[int], List[int]]) -> List[int]:
    program, to_be_buffered = puzzle_input
    for val in to_be_buffered:
        add_buffered_input(val)
    run_int_code_program(program)
    program_output = get_buffered_output()
    return program_output


def translate_input(puzzle_input_raw: str, part_num: int) -> Tuple[List[int], List[int]]:
    if part_num == 1:
        return [int(x) for x in puzzle_input_raw.split(",")], [1]
    elif part_num == 2:
        return [int(x) for x in puzzle_input_raw.split(",")], [5]


part_1_test_cases = [
    TestCase([6], ([3, 0, 4, 0, 99], [6])),  # 6 is buffered for input, program reads number in and outputs it
    TestCase([8], ([3, 0, 4, 0, 99], [8])),  # 8 is buffered for input, program reads number in and outputs it
]

part_2_test_cases = [
    TestCase([0], ([3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8], [7])),
    TestCase([1], ([3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8], [8])),
    TestCase([1], ([3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8], [7])),
    TestCase([0], ([3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8], [8])),
    TestCase([0], ([3, 3, 1108, -1, 8, 3, 4, 3, 99], [7])),
    TestCase([1], ([3, 3, 1108, -1, 8, 3, 4, 3, 99], [8])),
    TestCase([1], ([3, 3, 1107, -1, 8, 3, 4, 3, 99], [7])),
    TestCase([0], ([3, 3, 1107, -1, 8, 3, 4, 3, 99], [8])),
]

problem = AdventOfCodeProblem(
    part_1_test_cases,
    part_1_solver,
    part_2_test_cases,
    part_2_solver,
    translate_input,
    run_tests_only=True
)
