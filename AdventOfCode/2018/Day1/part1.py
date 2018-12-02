import sys
from typing import List


def calc_frequency(frequency_changes: List[str]) -> int:
    frequency = 0
    for change in frequency_changes:
        frequency += int(change)
    return frequency


def main():
    with open("input.txt", "r") as file:
        input_list = [line.strip() for line in file.readlines()]

    result_frequency = calc_frequency(input_list)
    print(f"Resulting Frequency: {result_frequency}")
    pass


def tests():
    test_cases = [
        (3,  ["+1", "+1", "+1"]),
        (0,  ["+1", "+1", "-2"]),
        (-6, ["-1", "-2", "-3"])
    ]
    for expected_output, test_input in test_cases:
        output = calc_frequency(test_input)
        if expected_output != output:
            print(ValueError(f"Expected {expected_output}, got {output}. Input was {test_input}"))
        else:
            print("Test passed!")
    pass


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
