from typing import List

from python_tools.advent_of_code_lib import AdventOfCodeProblem, TestCase


def run_program(input_program: List[int]) -> List[int]:
    program = list(input_program)
    program_position = 0
    while True:
        op_code = program[program_position]
        if op_code == 99:
            break
        input_a_position = program[program_position + 1]
        input_b_position = program[program_position + 2]
        input_a = program[input_a_position]
        input_b = program[input_b_position]
        output_position = program[program_position + 3]
        if op_code == 1:
            def operator(a, b): return a + b
        elif op_code == 2:
            def operator(a, b): return a * b
        else:
            raise ValueError(f"OpCode {op_code} not supported")
        program[output_position] = operator(input_a, input_b)
        program_position += 4
    return program


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

problem = AdventOfCodeProblem(
    part_1_test_cases,
    run_program,
    [],
    lambda x: x,
    translate_input
)
