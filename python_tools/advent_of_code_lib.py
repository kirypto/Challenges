from typing import Any, List, Callable


class TestCase:
    _expected: Any
    _puzzle_input: Any

    @property
    def expected(self) -> Any:
        return self._expected

    @property
    def puzzle_input(self) -> Any:
        return self._puzzle_input

    def __init__(self, expected: Any, puzzle_input: Any):
        self._expected = expected
        self._puzzle_input = puzzle_input


class AdventOfCodeProblem:
    _part_1_test_cases: List[TestCase]
    _part_2_test_cases: List[TestCase]
    _part_1_solve_method: Callable[[Any], Any]
    _part_2_solve_method: Callable[[Any], Any]
    _input_translate_method: Callable[[List[str]], Any]

    @property
    def part_1_test_cases(self) -> List[TestCase]:
        return self._part_1_test_cases

    @property
    def part_2_test_cases(self) -> List[TestCase]:
        return self._part_2_test_cases

    def part_1_solver(self, puzzle_input: Any) -> Any:
        return self._part_1_solve_method(puzzle_input)

    def part_2_solver(self, puzzle_input: Any) -> Any:
        return self._part_2_solve_method(puzzle_input)

    def translate_input(self, raw_puzzle_input: List[str]) -> List[Any]:
        return self._input_translate_method(raw_puzzle_input)

    def __init__(self,
                 part_1_test_cases: List[TestCase], part_1_solve_method: Callable[[Any], Any],
                 part_2_test_cases: List[TestCase], part_2_solve_method: Callable[[Any], Any],
                 translate_method: Callable[[List[str]], Any]):
        self._part_1_test_cases = part_1_test_cases
        self._part_2_test_cases = part_2_test_cases
        self._part_1_solve_method = part_1_solve_method
        self._part_2_solve_method = part_2_solve_method
        self._input_translate_method = translate_method
