import sys
from itertools import chain
from string import ascii_letters
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


def find_largest_finite_area(origins: List[str]) -> Tuple[int, str, List[str]]:
    origins = [(int(y), int(x)) for x, y in [coord.split(", ") for coord in origins]]
    max_y: int = max([coord[0] for coord in origins])
    max_x: int = max([coord[1] for coord in origins])

    closest_map = np.zeros((len(origins), max_y + 1, max_x + 1))

    for index, coord in enumerate(origins):
        y, x = coord
        manhattan_dist_from_coord = manhattan_dist_mask((max_y + 1, max_x + 1), (y, x))
        weighting = np.abs(manhattan_dist_from_coord - (max_x + max_y))
        closest_map[index] += weighting

    closest_index = np.argmax(closest_map, axis=0)

    max_values = np.take_along_axis(closest_map, np.expand_dims(closest_index, axis=0), axis=0)
    indices_where_multiple_are_max = np.sum(closest_map == max_values, axis=0) > 1

    dot_index = len(origins)
    result = np.where(indices_where_multiple_are_max, dot_index, closest_index)

    letter_chain = chain(ascii_letters)
    letters = [next(letter_chain) for i in range(len(origins))]
    letters.append(".")
    letters = np.array(letters)

    letter_map = letters[result]
    edge_selector = np.zeros((max_y - 1, max_x - 1)).astype(np.dtype("bool"))
    edge_selector = np.pad(edge_selector, 1, mode="constant", constant_values=True)
    disallowed = sorted(list(set(letter_map[edge_selector])))
    disallowed.remove(".")
    largest_area = -1
    largest_letter = None
    for letter in set(letter_map.flatten()):
        print(letter)
        if letter == "." or letter in disallowed:
            continue
        area_of_letter = np.sum(letter_map == letter)
        if area_of_letter > largest_area:
            largest_area = area_of_letter
            largest_letter = letter
    return largest_area, largest_letter, disallowed


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
