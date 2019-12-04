import sys
from abc import ABC, abstractmethod
from typing import Callable, Any, List, Tuple


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


class AdventOfCodeProblem(ABC):
    @property
    @abstractmethod
    def part_1_test_cases(self) -> List[TestCase]:
        ...

    def part_2_test_cases(self) -> List[TestCase]:
        pass

    # def part_1_solution(self, ):


def calc_fuel(mass: int) -> int:
    return mass // 3 - 2


def calc_fuel_recursive(mass: int) -> int:
    fuel_cost = calc_fuel(mass)
    if fuel_cost < 0:
        return 0
    return fuel_cost + calc_fuel_recursive(fuel_cost)


def read_puzzle_input():
    input_file_path = str(__file__).replace(".py", "_input.txt")
    with open(input_file_path, mode="r") as input_file:
        puzzle_input = input_file.readlines()
    return puzzle_input


def do_tests(method_under_test: Callable[[Any], Any], test_cases: List[Tuple[Any, Any]]):
    for expected_output, test_input in test_cases:
        output = method_under_test(test_input)
        if expected_output != output:
            print(ValueError(f"Expected {expected_output}, got {output}. Input was {test_input}"))
        else:
            print("Test passed!")


def tests_part1():
    test_cases = [
        (2, 12),
        (2, 14),
        (654, 1969),
        (33583, 100756),
    ]
    method_under_test = calc_fuel
    do_tests(method_under_test, test_cases)
    print("", flush=True)


def tests_part2():
    test_cases = [
        (2, 14),
        (654 + 216 + 70 + 21 + 5, 1969),
        (33583 + 11192 + 3728 + 1240 + 411 + 135 + 43 + 12 + 2, 100756),
    ]
    method_under_test = calc_fuel_recursive
    do_tests(method_under_test, test_cases)
    print("", flush=True)


def _main():
    puzzle_input = read_puzzle_input()
    print(sum([calc_fuel_recursive(int(x)) for x in puzzle_input]))


if __name__ == '__main__':
    run_mode = "main" if len(sys.argv) < 2 else str(sys.argv[1]).lower()
    if run_mode == "main":
        _main()
    elif run_mode == "tests":
        print("--- Part 1 Tests:")
        tests_part1()
        print("--- Part 2 Tests:")
        tests_part2()
        print("--- End of Tests")
    else:
        raise ValueError(f"Run mode '{run_mode}' is not supported")
