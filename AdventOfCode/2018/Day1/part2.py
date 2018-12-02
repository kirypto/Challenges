import sys
from itertools import cycle
from typing import List


def calc_repeated_frequency(data: List[str]) -> int:
    data = cycle([int(x) for x in data])
    observed_frequencies = set()
    frequency = 0
    while frequency not in observed_frequencies:
        observed_frequencies.add(frequency)
        frequency += next(data)
    return frequency


def main():
    with open("input.txt", "r") as file:
        input_list = [line.strip() for line in file.readlines()]

    result_frequency = calc_repeated_frequency(input_list)
    print(f"Result: {result_frequency}")


def tests():
    test_cases = [
        (0,  ["+1", "-1"]),
        (10, ["+3", "+3", "+4", "-2", "-4"]),
        (5,  ["-6", "+3", "+8", "+5", "-6"]),
        (14, ["+7", "+7", "-2", "-7", "-4"])
    ]
    for expected_output, test_input in test_cases:
        output = calc_repeated_frequency(test_input)
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
