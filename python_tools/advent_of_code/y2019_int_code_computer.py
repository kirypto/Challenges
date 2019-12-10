from enum import Enum
from typing import Dict, Tuple, Callable, List, Optional

from python_tools.maths import multiply


_input_buffer = []
_output_buffer = []


def add_buffered_input(input_int: int) -> None:
    _input_buffer.append(input_int)


def _input_int(_: List[int]) -> int:
    if _input_buffer:
        buffered_input = _input_buffer.pop()
        print(f"[INT CODE COMPUTER] [buffered input] {buffered_input}")
        return buffered_input
    return int(input("[INT CODE COMPUTER] [input] "))


def get_buffered_output() -> List[int]:
    output = list(_output_buffer)
    _output_buffer.clear()
    return output


def _output_int(output: List[int]) -> None:
    if len(output) != 1:
        raise ValueError("Output should only have one int!")
    _output_buffer.extend(output)
    print(f"[INT CODE COMPUTER] [output] {output[0]}")


def _less_than(to_compare: List[int]) -> int:
    a, b = to_compare
    return 1 if a < b else 0


def _equals(to_compare: List[int]) -> int:
    a, b = to_compare
    return 1 if a == b else 0


def _jump_if_false(bool_and_position: List[int]) -> Optional[int]:
    should_jump, instr_ptr_pos = bool_and_position
    if should_jump == 0:
        return instr_ptr_pos
    return None


class _OpCode(Enum):
    ADD = 1
    MULTIPLY = 2
    INPUT = 3
    OUTPUT = 4
    JUMP_IF_FALSE = 6
    LESS_THAN = 7
    EQUALS = 8
    HALT = 99


class _ParameterMode(Enum):
    POSITION = 0
    IMMEDIATE = 1


class _IntCodeInstruction:
    _instruction_info: Dict[_OpCode, Tuple[int, int, Callable[[List[int]], int]]] = {
        # <OpCode>: (<num_inputs>, <has_output>, operator>, <moves_instr_pointer>)
        _OpCode.ADD: (2, True, sum, False),
        _OpCode.MULTIPLY: (2, True, multiply, False),
        _OpCode.INPUT: (0, True, _input_int, False),
        _OpCode.OUTPUT: (1, False, _output_int, False),
        _OpCode.JUMP_IF_FALSE: (2, False, _jump_if_false, True),
        _OpCode.LESS_THAN: (2, True, _less_than, False),
        _OpCode.EQUALS: (2, True, _equals, False),
    }
    _INFO_INDEX_NUM_INPUTS = 0
    _INFO_INDEX_HAS_OUTPUT = 1
    _INFO_INDEX_OPERATOR = 2
    _INFO_INDEX_MOVES_INSTR_PTR = 3

    @property
    def op_code(self) -> _OpCode:
        return self._op_code

    @property
    def operator(self) -> Callable[[List[int]], int]:
        return self._instruction_info[self.op_code][self._INFO_INDEX_OPERATOR]

    @property
    def inputs(self) -> List[Tuple[_ParameterMode, int]]:
        num_inputs = self._instruction_info[self.op_code][self._INFO_INDEX_NUM_INPUTS]
        return [(self._param_modes[i], i + 1) for i in range(num_inputs)]

    @property
    def output(self) -> Optional[int]:
        if not self._instruction_info[self.op_code][self._INFO_INDEX_HAS_OUTPUT]:
            return None
        return self._instruction_info[self.op_code][self._INFO_INDEX_NUM_INPUTS] + 1

    @property
    def pointer_offset(self) -> int:
        has_output = self._instruction_info[self.op_code][self._INFO_INDEX_HAS_OUTPUT]
        additional_offset = 2 if has_output else 1
        return self._instruction_info[self.op_code][self._INFO_INDEX_NUM_INPUTS] + additional_offset
    
    @property
    def moves_instr_ptr(self) -> bool:
        return self._instruction_info[self.op_code][self._INFO_INDEX_MOVES_INSTR_PTR]

    def __init__(self, instruction: int) -> None:
        instruction_str = str(instruction).rjust(5, "0")
        self._op_code = _OpCode(int(instruction_str[-2:]))
        self._param_modes = [
            _ParameterMode(int(instruction_str[2])),
            _ParameterMode(int(instruction_str[1])),
            _ParameterMode(int(instruction_str[0]))
        ]


def run_int_code_program(input_program: List[int], noun: int = None, verb: int = None) -> List[int]:
    program_memory = list(input_program)
    if noun is not None:
        program_memory[1] = noun
    if verb is not None:
        program_memory[2] = verb
    instruction_pointer = 0
    while True:
        instruction = _IntCodeInstruction(program_memory[instruction_pointer])
        if instruction.op_code == _OpCode.HALT:
            break

        inputs = []
        for param_mode, input_offset in instruction.inputs:
            if param_mode == _ParameterMode.IMMEDIATE:
                inputs.append(program_memory[instruction_pointer + input_offset])
            elif param_mode == _ParameterMode.POSITION:
                inputs.append(program_memory[program_memory[instruction_pointer + input_offset]])

        result = instruction.operator(inputs)

        if instruction.output is not None:
            output_position = program_memory[instruction_pointer + instruction.output]
            program_memory[output_position] = result
        if instruction.moves_instr_ptr and result is not None:
            instruction_pointer = result
        else:
            instruction_pointer += instruction.pointer_offset
    return program_memory
