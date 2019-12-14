from enum import Enum
from typing import Dict, Tuple, Callable, List, Optional

from python_tools.maths import multiply


class _OpCode(Enum):
    ADD = 1
    MULTIPLY = 2
    INPUT = 3
    OUTPUT = 4
    JUMP_IF_TRUE = 5
    JUMP_IF_FALSE = 6
    LESS_THAN = 7
    EQUALS = 8
    HALT = 99


class _ParameterMode(Enum):
    POSITION = 0
    IMMEDIATE = 1


class _IntCodeInstruction:
    _instruction_info: Dict[_OpCode, Tuple[int, int, bool]] = {
        # <OpCode>: (<num_inputs>, <has_output>, <moves_instr_pointer>)
        _OpCode.ADD: (2, True, False),
        _OpCode.MULTIPLY: (2, True, False),
        _OpCode.INPUT: (0, True, False),
        _OpCode.OUTPUT: (1, False, False),
        _OpCode.JUMP_IF_TRUE: (2, False, True),
        _OpCode.JUMP_IF_FALSE: (2, False, True),
        _OpCode.LESS_THAN: (2, True, False),
        _OpCode.EQUALS: (2, True, False),
    }
    _INFO_INDEX_NUM_INPUTS = 0
    _INFO_INDEX_HAS_OUTPUT = 1
    _INFO_INDEX_MOVES_INSTR_PTR = 2

    @property
    def op_code(self) -> _OpCode:
        return self._op_code

    @property
    def inputs(self) -> List[Tuple[_ParameterMode, int]]:
        num_inputs = self._instruction_info[self._op_code][self._INFO_INDEX_NUM_INPUTS]
        return [(self._param_modes[i], i + 1) for i in range(num_inputs)]

    @property
    def output(self) -> Optional[int]:
        if not self._instruction_info[self._op_code][self._INFO_INDEX_HAS_OUTPUT]:
            return None
        return self._instruction_info[self._op_code][self._INFO_INDEX_NUM_INPUTS] + 1

    @property
    def pointer_offset(self) -> int:
        has_output = self._instruction_info[self._op_code][self._INFO_INDEX_HAS_OUTPUT]
        additional_offset = 2 if has_output else 1
        return self._instruction_info[self._op_code][self._INFO_INDEX_NUM_INPUTS] + additional_offset

    @property
    def moves_instr_ptr(self) -> bool:
        return self._instruction_info[self._op_code][self._INFO_INDEX_MOVES_INSTR_PTR]

    def __init__(self, instruction: int) -> None:
        instruction_str = str(instruction).rjust(5, "0")
        self._op_code = _OpCode(int(instruction_str[-2:]))
        self._param_modes = [
            _ParameterMode(int(instruction_str[2])),
            _ParameterMode(int(instruction_str[1])),
            _ParameterMode(int(instruction_str[0]))
        ]


def set_verbose(verbose: bool) -> None:
    global __default_computer_verbose
    __default_computer_verbose = verbose


def add_buffered_input(input_int: int) -> None:
    # global __default_global_int_computer
    # __default_global_int_computer: IntCodeComputer
    _input_buffer.append(input_int)


def get_buffered_output() -> List[int]:
    output = list(_output_buffer)
    _output_buffer.clear()
    return output


def run_int_code_program(input_program: List[int], noun: int = None, verb: int = None) -> List[int]:
    # __default_global_int_computer: IntCodeComputer
    global __default_global_int_computer
    global __default_computer_verbose
    __default_global_int_computer = IntCodeComputer()
    __default_global_int_computer.set_verbose(__default_computer_verbose)
    __default_global_int_computer.set_int_code_program(input_program, noun=noun, verb=verb)
    return __default_global_int_computer.run()


class IntCodeComputer:
    _program_memory: List[int]
    _input_buffer: List[int]
    _output_buffer: List[int]
    _instruction_pointer: int
    __default_computer_verbose: bool

    _operators: Dict[_OpCode, Callable[[List[int]], int]]

    def __init__(self) -> None:
        self._verbose = True
        self._operators = {
            _OpCode.ADD: sum,
            _OpCode.MULTIPLY: multiply,
            _OpCode.INPUT: self._input_int,
            _OpCode.OUTPUT: self._output_int,
            _OpCode.JUMP_IF_TRUE: self._jump_if_true,
            _OpCode.JUMP_IF_FALSE: self._jump_if_false,
            _OpCode.LESS_THAN: self._less_than,
            _OpCode.EQUALS: self._equals,
        }

    def set_verbose(self, verbose: bool) -> None:
        self._verbose = verbose

    def set_int_code_program(self, input_program: List[int], noun: int = None, verb: int = None):
        self._program_memory = list(input_program)
        if noun is not None:
            self._program_memory[1] = noun
        if verb is not None:
            self._program_memory[2] = verb
        self._instruction_pointer = 0

    def run(self):
        while True:
            instruction = _IntCodeInstruction(self._program_memory[self._instruction_pointer])
            if instruction.op_code == _OpCode.HALT:
                break

            inputs = []
            for param_mode, input_offset in instruction.inputs:
                if param_mode == _ParameterMode.IMMEDIATE:
                    inputs.append(self._program_memory[self._instruction_pointer + input_offset])
                elif param_mode == _ParameterMode.POSITION:
                    inputs.append(self._program_memory[self._program_memory[self._instruction_pointer + input_offset]])

            operator = self._operators[instruction.op_code]
            result = operator(inputs)

            if instruction.output is not None:
                output_position = self._program_memory[self._instruction_pointer + instruction.output]
                self._program_memory[output_position] = result
            if instruction.moves_instr_ptr and result is not None:
                self._instruction_pointer = result
            else:
                self._instruction_pointer += instruction.pointer_offset
        return self._program_memory

    def _input_int(self, _: List[int]) -> int:
        if _input_buffer:
            buffered_input = _input_buffer.pop()
            if self._verbose:
                print(f"[INT CODE COMPUTER] [buffered input] {buffered_input}")
            return buffered_input
        return int(input("[INT CODE COMPUTER] [input] "))

    def _output_int(self, output: List[int]) -> None:
        if len(output) != 1:
            raise ValueError("Output should only have one int!")
        _output_buffer.extend(output)
        if self._verbose:
            print(f"[INT CODE COMPUTER] [output] {output[0]}")

    @staticmethod
    def _less_than(to_compare: List[int]) -> int:
        a, b = to_compare
        return 1 if a < b else 0

    @staticmethod
    def _equals(to_compare: List[int]) -> int:
        a, b = to_compare
        return 1 if a == b else 0

    @staticmethod
    def _jump_if_false(bool_and_position: List[int]) -> Optional[int]:
        should_jump, instr_ptr_pos = bool_and_position
        if should_jump == 0:
            return instr_ptr_pos
        return None

    @staticmethod
    def _jump_if_true(bool_and_position: List[int]) -> Optional[int]:
        should_jump, instr_ptr_pos = bool_and_position
        if should_jump != 0:
            return instr_ptr_pos
        return None


_input_buffer = []
_output_buffer = []


__default_computer_verbose = True
__default_global_int_computer = None
