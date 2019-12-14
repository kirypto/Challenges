from itertools import product
from typing import List, Tuple, Union

from python_tools.advent_of_code.puzzle_runner_helpers import AdventOfCodeProblem, TestCase
from python_tools.advent_of_code.y2019_int_code_computer import IntCodeComputer


def part_1_solver(part_1_input: List[int]) -> List[int]:
    input_program, noun, verb = part_1_input
    modified_program = list(input_program)
    if noun is not None:
        modified_program[1] = noun
    if verb is not None:
        modified_program[2] = verb
    return IntCodeComputer(modified_program).run()


def part_2_solver(part_2_input: Tuple[List[int], int]) -> Tuple[int, int]:
    input_program, desired_output = part_2_input
    possible_verbs = possible_nouns = list(range(min(100, len(input_program))))
    all_possible_noun_verb_combinations = product(possible_nouns, possible_verbs)
    for noun, verb in all_possible_noun_verb_combinations:
        modified_program = input_program
        if noun is not None:
            modified_program[1] = noun
        if verb is not None:
            modified_program[2] = verb
        result = IntCodeComputer(input_program).run()
        if result[0] == desired_output:
            return noun, verb
    return -1, -1


def translate_input(puzzle_input_raw: str, part_num: int) -> Union[Tuple[List[int], int, int], Tuple[List[int], int]]:
    if part_num == 1:
        return [int(x) for x in puzzle_input_raw.split(",")], 12, 2
    else:
        return [int(x) for x in puzzle_input_raw.split(",")], 19690720


part_1_test_cases = [
    TestCase([2, 0, 0, 0, 99], ([1, 0, 0, 0, 99], None, None)),
    TestCase([2, 3, 0, 6, 99], ([2, 3, 0, 3, 99], None, None)),
    TestCase([2, 4, 4, 5, 99, 9801], ([2, 4, 4, 5, 99, 0], None, None)),
    TestCase([30, 1, 1, 4, 2, 5, 6, 0, 99], ([1, 1, 1, 4, 99, 5, 6, 0, 99], None, None))
]

part_2_test_cases = [
    TestCase((5, 6), ([2, -1, -1, 0, 99, 3, 4], 12))
]

problem = AdventOfCodeProblem(
    part_1_test_cases,
    part_1_solver,
    part_2_test_cases,
    part_2_solver,
    translate_input
)
