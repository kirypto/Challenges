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


def part_2_solver(puzzle_input: Tuple[List[int], List[int]]) -> List[int]:
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
    TestCase([0], ([3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8], [7])),  # Position mode eq test
    TestCase([1], ([3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8], [8])),  # Position mode eq test
    TestCase([1], ([3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8], [7])),  # Position mode less than test
    TestCase([0], ([3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8], [8])),  # Position mode less than test
    TestCase([0], ([3, 3, 1108, -1, 8, 3, 4, 3, 99], [7])),  # Immediate mode eq test
    TestCase([1], ([3, 3, 1108, -1, 8, 3, 4, 3, 99], [8])),  # Immediate mode eq test
    TestCase([1], ([3, 3, 1107, -1, 8, 3, 4, 3, 99], [7])),  # Immediate mode less than test
    TestCase([0], ([3, 3, 1107, -1, 8, 3, 4, 3, 99], [8])),  # Immediate mode less than test
    TestCase([0], ([3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9], [0])),  # Position mode jump test
    TestCase([1], ([3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9], [745])),  # Position mode jump test
    TestCase([0], ([3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1], [0])),  # Immediate mode jump test
    TestCase([1], ([3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1], [2341])),  # Immediate mode jump test
    TestCase([999], ([3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31, 1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1,
                      46, 104, 999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99], [7])),  # Outputs 999 if input < 8
    TestCase([1000], ([3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31, 1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1,
                       46, 104, 999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99], [8])),  # Outputs 1000 if input == 8
    TestCase([1001], ([3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31, 1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1,
                       46, 104, 999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99], [9])),  # Outputs 1001 if input > 8
]

problem = AdventOfCodeProblem(
    part_1_test_cases,
    part_1_solver,
    part_2_test_cases,
    part_2_solver,
    translate_input,
    run_tests_only=True
)
