import re
import sys
from typing import List, Tuple

import numpy as np


def find__non_overlapping_cut(cuts: List[str]) -> int:

    pattern = re.compile("#(\d+) @ (\d+),(\d+): (\d+)x(\d+)")

    def parse_cut(cut_str: str) -> Tuple[int, int, int, int, int]:
        match = pattern.match(cut_str)
        return int(match.group(1)), int(match.group(2)), int(match.group(3)), int(match.group(4)), int(match.group(5))

    fabric = np.zeros((1000, 1000))

    non_overlapping_cut = None

    for cut in cuts:
        cut_id, x, y, w, h = parse_cut(cut)
        if 0 == np.sum(fabric[y:y+h, x:x+w]):
            non_overlapping_cut = cut_id
        fabric[y:y+h, x:x+w] += 1

    return non_overlapping_cut


def main():
    with open("input.txt", "r") as file:
        input_list = [line.strip() for line in file.readlines()]

    result = find__non_overlapping_cut(input_list)
    print(f"Result: {result}")


def tests():
    test_cases = [
        (3, ["#1 @ 1,3: 4x4", "#2 @ 3,1: 4x4", "#3 @ 5,5: 2x2"])
    ]
    for expected_output, test_input in test_cases:
        output = find__non_overlapping_cut(test_input)
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
