from enum import Enum
from itertools import product
from typing import List, Tuple, Union, Callable, Dict

from python_tools.advent_of_code_lib import AdventOfCodeProblem, TestCase
from python_tools.maths import multiply


class OpCode(Enum):
    ADD = 1
    MULTIPLY = 2
    HALT = 99


class ParameterMode(Enum):
    POSITION = 0
    IMMEDIATE = 1


class IntCodeInstruction:
    _instruction_info: Dict[OpCode, Tuple[int, Callable[[List[int]], int]]] = {
        OpCode.ADD: (2, sum),
        OpCode.MULTIPLY: (2, multiply),
    }

    @property
    def op_code(self) -> OpCode:
        return self._op_code

    @property
    def operator(self) -> Callable[[List[int]], int]:
        return self._instruction_info[self.op_code][1]

    @property
    def inputs(self) -> List[Tuple[ParameterMode, int]]:
        num_inputs = self._instruction_info[self.op_code][0]
        return [(self._param_modes[i], i + 1) for i in range(num_inputs)]

    @property
    def output(self) -> int:
        return self._instruction_info[self.op_code][0] + 1

    @property
    def pointer_offset(self) -> int:
        return self._instruction_info[self.op_code][0] + 2

    def __init__(self, instruction: int) -> None:
        instruction_str = str(instruction).rjust(5, "0")
        self._op_code = OpCode(int(instruction_str[-2:]))
        self._param_modes = [
            ParameterMode(int(instruction_str[2])),
            ParameterMode(int(instruction_str[1])),
            ParameterMode(int(instruction_str[0]))
        ]


def run_int_code_program(input_program: List[int], noun: int = None, verb: int = None) -> List[int]:
    program_memory = list(input_program)
    if noun is not None:
        program_memory[1] = noun
    if verb is not None:
        program_memory[2] = verb
    instruction_pointer = 0
    while True:
        instruction = IntCodeInstruction(program_memory[instruction_pointer])
        if instruction.op_code == OpCode.HALT:
            break

        inputs = []
        for param_mode, input_offset in instruction.inputs:
            if param_mode == ParameterMode.IMMEDIATE:
                inputs.append(program_memory[instruction_pointer + input_offset])
            elif param_mode == ParameterMode.POSITION:
                inputs.append(program_memory[program_memory[instruction_pointer + input_offset]])

        result = instruction.operator(inputs)

        output_position = program_memory[instruction_pointer + instruction.output]
        program_memory[output_position] = result
        instruction_pointer += instruction.pointer_offset
    return program_memory


def part_1_solver(part_1_input: List[int]) -> List[int]:
    input_program, noun, verb = part_1_input
    return run_int_code_program(input_program, noun=noun, verb=verb)


def part_2_solver(part_2_input: Tuple[List[int], int]) -> Tuple[int, int]:
    input_program, desired_output = part_2_input
    possible_verbs = possible_nouns = list(range(min(100, len(input_program))))
    all_possible_noun_verb_combinations = product(possible_nouns, possible_verbs)
    for noun, verb in all_possible_noun_verb_combinations:
        result = run_int_code_program(input_program, noun=noun, verb=verb)
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
