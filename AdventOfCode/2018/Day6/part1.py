import sys
from typing import List, Tuple


def find_largest_finite_area(origins: List[str]) -> Tuple[int, str, List[str]]:
    pass


def main():
    with open("input.txt", "r") as file:
        input_list = [line.strip() for line in file.readlines()]

    result = find_largest_finite_area(input_list)
    print(f"Result: {result}")


def tests():
    test_cases = [
        ((17, "e", ["a", "b", "c", "f"]), [
            "1, 1",
            "1, 6",
            "8, 3",
            "3, 4",
            "5, 5",
            "8, 9",
        ])
    ]
    for expected_output, test_input in test_cases:
        output = find_largest_finite_area(test_input)
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
