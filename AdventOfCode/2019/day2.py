from enum import Enum
from typing import List

from python_tools.advent_of_code_lib import AdventOfCodeProblem, TestCase


class OpCode(Enum):
    ADD = 1
    MULTIPLY = 2
    HALT = 99


# noinspection SpellCheckingInspection
def run_intcode_program(input_program: List[int]) -> List[int]:
    program_memory = list(input_program)
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


def translate_input(raw: str) -> List[int]:
    int_input_list = [int(x) for x in raw.split(",")]
    int_input_list[1] = 12
    int_input_list[2] = 2
    return int_input_list


part_1_test_cases = [
    TestCase([2, 0, 0, 0, 99], [1, 0, 0, 0, 99]),
    TestCase([2, 3, 0, 6, 99], [2, 3, 0, 3, 99]),
    TestCase([2, 4, 4, 5, 99, 9801], [2, 4, 4, 5, 99, 0]),
    TestCase([30, 1, 1, 4, 2, 5, 6, 0, 99], [1, 1, 1, 4, 99, 5, 6, 0, 99])
]

part_2_test_cases = part_1_test_cases  # No change in computer, just looking for magical combination

problem = AdventOfCodeProblem(
    part_1_test_cases,
    run_intcode_program,
    part_2_test_cases,
    run_intcode_program,
    translate_input
)
