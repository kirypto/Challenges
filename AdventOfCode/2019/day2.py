from enum import Enum
from itertools import product
from typing import List, Tuple, Union

from python_tools.advent_of_code_lib import AdventOfCodeProblem, TestCase


class OpCode(Enum):
    ADD = 1
    MULTIPLY = 2
    HALT = 99


# noinspection SpellCheckingInspection
def run_intcode_program(input_program: List[int], noun: int = None, verb: int = None) -> List[int]:
    program_memory = list(input_program)
    if noun is not None:
        program_memory[1] = noun
    if verb is not None:
        program_memory[2] = verb
    instruction_pointer = 0
    while True:
        op_code = OpCode(program_memory[instruction_pointer])
        if op_code == OpCode.HALT:
            break
        input_a_position = program_memory[instruction_pointer + 1]
        input_b_position = program_memory[instruction_pointer + 2]
        input_a = program_memory[input_a_position]
        input_b = program_memory[input_b_position]
        output_position = program_memory[instruction_pointer + 3]
        if op_code == OpCode.ADD:
            def operator(a, b):
                return a + b
        elif op_code == OpCode.MULTIPLY:
            def operator(a, b):
                return a * b
        else:
            raise ValueError(f"OpCode {op_code} not supported")
        program_memory[output_position] = operator(input_a, input_b)
        instruction_pointer += 4
    return program_memory


def part_1_solver(part_1_input: List[int]) -> List[int]:
    input_program, noun, verb = part_1_input
    return run_intcode_program(input_program, noun=noun, verb=verb)


def part_2_solver(part_2_input: Tuple[List[int], int]) -> Tuple[int, int]:
    input_program, desired_output = part_2_input
    possible_verbs = possible_nouns = list(range(min(100, len(input_program))))
    all_possible_noun_verb_combinations = product(possible_nouns, possible_verbs)
    for noun, verb in all_possible_noun_verb_combinations:
        result = run_intcode_program(input_program, noun=noun, verb=verb)
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
