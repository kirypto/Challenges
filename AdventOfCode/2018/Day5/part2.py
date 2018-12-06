import re
import sys
from string import ascii_lowercase
from typing import Tuple, Dict


def reduce_polymer(polymer_string: str) -> Tuple[int, str]:
    # noinspection PyUnresolvedReferences
    regs: Dict[str, re.Pattern] = {}
    for char in ascii_lowercase:
        regs[char] = re.compile(f"({char}{char.upper()}|{char.upper()}{char})")

    prev_polymer_length = -1
    while prev_polymer_length != len(polymer_string):
        prev_polymer_length = len(polymer_string)
        for char, regex in regs.items():
            # tmp_length = len(polymer_string)
            polymer_string = regex.sub("", polymer_string)
            # remove_count = tmp_length - len(polymer_string)
            # if remove_count > 0:
            #     print(f"Replaced { remove_count } characters!")
    return len(polymer_string), polymer_string


def main():
    with open("input.txt", "r") as file:
        input_list = [line.strip() for line in file.readlines()]
    if len(input_list) != 1:
        raise RuntimeError(f"Unhandled input, had multiple lines: {len(input_list)}")

    result = find_best_removal(input_list[0])
    print(f"Result: {result}")


def tests():
    test_cases = [
        ((4, "c", "daDA"), "dabAcCaCBAcCcaDA")
    ]
    for expected_output, test_input in test_cases:
        output = find_best_removal(test_input)
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
