from itertools import permutations
from typing import List, Optional, Tuple

from python_tools.advent_of_code.puzzle_runner_helpers import AdventOfCodeProblem, TestCase
from python_tools.advent_of_code.y2019_int_code_computer import IntCodeComputer


def part_1_solver(puzzle_input: List[int]) -> Tuple[int, List[int]]:
    all_possible_phase_settings = list(permutations(range(5)))
    max_phase_setting: List[int] = []
    max_thruster_signal: Optional[int] = None

    for phase_setting in all_possible_phase_settings:
        phase_setting = list(phase_setting)
        last_output = [0]
        for amplifier_index, phase_signal in enumerate(phase_setting):
            computer = IntCodeComputer(list(puzzle_input), verbose=False)
            computer.add_buffered_input(phase_signal)
            computer.add_buffered_input(last_output[0])
            computer.run()
            last_output = computer.get_buffered_output()
        output = last_output[0]

        if max_thruster_signal is None or output > max_thruster_signal:
            max_phase_setting = phase_setting
            max_thruster_signal = output
    return max_thruster_signal, max_phase_setting


def part_2_solver(puzzle_input: List[int]) -> List[int]:
    return puzzle_input


def translate_input(puzzle_input_raw: str, _: int) -> List[int]:
    return [int(x) for x in puzzle_input_raw.split(",")]


part_1_test_cases = [
    TestCase((43210, [4, 3, 2, 1, 0]), [3, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0]),
    TestCase((54321, [0, 1, 2, 3, 4]), [3, 23, 3, 24, 1002, 24, 10, 24, 1002, 23, -1, 23, 101, 5, 23, 23, 1, 24, 23, 23, 4, 23, 99, 0, 0]),
    TestCase((65210, [1, 0, 4, 3, 2]), [3, 31, 3, 32, 1002, 32, 10, 32, 1001, 31, -2, 31, 1007, 31, 0, 33, 1002, 33, 7, 33, 1, 33, 31, 31, 1, 32, 31,
                                        31, 4, 31, 99, 0, 0, 0]),
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
)
