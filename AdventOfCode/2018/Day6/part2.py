import sys
from typing import List, Tuple

import numpy as np


def manhattan_dist_mask(shape: Tuple[int, int], center: Tuple[int, int]):
    shape_y, shape_x = shape
    center_y, center_x = center

    y, x = np.ogrid[-center_y: shape_y - center_y, -center_x: shape_x - center_x]

    a = np.abs(y + x)
    b = np.abs(y - x)
    quadrant_selector = np.array([[
        x < center_x and y < center_y or x > center_x and y > center_y
        for x in range(shape_x)
    ]
        for y in range(shape_y)
    ])
    return np.where(quadrant_selector, a, b)


def find_area_within_target_distance(origins: List[str], target_dist: int) -> int:
    origins = [(int(y), int(x)) for x, y in [coord.split(", ") for coord in origins]]
    max_y: int = max([coord[0] for coord in origins])
    max_x: int = max([coord[1] for coord in origins])

    manhattan_distances = np.zeros((len(origins), max_y + 1, max_x + 1))

    for index, coord in enumerate(origins):
        y, x = coord
        manhattan_distances[index] += manhattan_dist_mask((max_y + 1, max_x + 1), (y, x))

    distances_to_all_origins = np.sum(manhattan_distances, axis=0)

    area_within_target: int = np.sum(distances_to_all_origins < target_dist)
    return area_within_target


def main():
    with open("input.txt", "r") as file:
        input_list = [line.strip() for line in file.readlines()]

    result = find_area_within_target_distance(input_list, 10000)
    print(f"Result: {result}")


def tests():
    test_cases = [
        (16, [
            "1, 1",
            "1, 6",
            "8, 3",
            "3, 4",
            "5, 5",
            "8, 9",
        ])
    ]
    for expected_output, test_input in test_cases:
        output = find_area_within_target_distance(test_input, 32)
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
