from typing import Callable, Any, List, Tuple


def calc_fuel(mass: int) -> int:
    return mass // 3 - 2


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


def tests():
    test_cases = [
        (2, 12),
        (2, 14),
        (654, 1969),
        (33583, 100756),
    ]
    method_under_test = calc_fuel
    do_tests(method_under_test, test_cases)
    print("", flush=True)


def _main():
    puzzle_input = read_puzzle_input()
    print(sum([calc_fuel(int(x)) for x in puzzle_input]))


if __name__ == '__main__':
    _main()
