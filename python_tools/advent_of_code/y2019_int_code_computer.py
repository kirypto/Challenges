from enum import Enum
from typing import Dict, Tuple, Callable, List, Optional

from python_tools.maths import multiply


class OpCode(Enum):
    ADD = 1
    MULTIPLY = 2
    INPUT = 3
    OUTPUT = 4
    HALT = 99


class ParameterMode(Enum):
    POSITION = 0
    IMMEDIATE = 1


class IntCodeInstruction:
    _instruction_info: Dict[OpCode, Tuple[int, int, Callable[[List[int]], int]]] = {
        OpCode.ADD: (2, True, sum),
        OpCode.MULTIPLY: (2, True, multiply),
        OpCode.INPUT: (0, True, input),
        OpCode.OUTPUT: (1, False, print)
    }

    @property
    def op_code(self) -> OpCode:
        return self._op_code

    @property
    def operator(self) -> Callable[[List[int]], int]:
        return self._instruction_info[self.op_code][2]

    @property
    def inputs(self) -> List[Tuple[ParameterMode, int]]:
        num_inputs = self._instruction_info[self.op_code][0]
        return [(self._param_modes[i], i + 1) for i in range(num_inputs)]

    @property
    def output(self) -> Optional[int]:
        if not self._instruction_info[self.op_code][1]:
            return None
        return self._instruction_info[self.op_code][0] + 1

    @property
    def pointer_offset(self) -> int:
        additional_offset = 2 if self._instruction_info[self.op_code][1] else 1
        return self._instruction_info[self.op_code][0] + additional_offset

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

        if instruction.output is not None:
            output_position = program_memory[instruction_pointer + instruction.output]
            program_memory[output_position] = result
        instruction_pointer += instruction.pointer_offset
    return program_memory