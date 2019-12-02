from typing import Callable, Any, List, Tuple


def calc_fuel(mass: int) -> int:
    return mass // 3 - 2


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
    pass


if __name__ == '__main__':
    _main()
