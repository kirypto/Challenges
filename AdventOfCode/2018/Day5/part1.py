import sys
from typing import Tuple


def reduce_polymer(polymer_string: str) -> Tuple[int, str]:
    pass


def main():
    with open("input.txt", "r") as file:
        input_list = [line.strip() for line in file.readlines()]
    if len(input_list) > 0:
        raise RuntimeError("Unhandled input, had multiple lines")

    result = reduce_polymer(input_list[0])
    print(f"Result: {result}")


def tests():
    test_cases = [
        ((10, "dabCBAcaDA"), "dabAcCaCBAcCcaDA")
    ]
    for expected_output, test_input in test_cases:
        output = reduce_polymer(test_input)
        if expected_output != output:
            print(ValueError(f"Expected {expected_output}, got {output}. Input was {test_input}"))
        else:
            print("Test passed!")


if __name__ == '__main__':
    if len(sys.argv) != 2:
        raise RuntimeError("Must provide run mode argument")
    run_mode = sys.argv[1]
    test_mode = "Test"
    main_mode = "Main"
    if run_mode == test_mode:
        tests()
    elif run_mode == main_mode:
        main()
    else:
        raise RuntimeError(f"Run run_mode must be either '{test_mode}' or '{main_mode}', was {run_mode}")
