import sys
from typing import List


def calc_checksum(box_ids: List[str]) -> int:
    pass


def main():
    with open("input.txt", "r") as file:
        input_list = [line.strip() for line in file.readlines()]

    result_frequency = calc_checksum(input_list)
    print(f"Result: {result_frequency}")


def tests():
    test_cases = [
        ((12, 4, 3), ["abcdef", "bababc", "abbcde", "abcccd", "aabcdd", "abcdee", "ababab"])
    ]
    for expected_output, test_input in test_cases:
        output = calc_checksum(test_input)
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
