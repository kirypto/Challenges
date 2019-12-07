from typing import List, Tuple

from numpy import logical_and, ones, argwhere
from tqdm import tqdm

from python_tools.advent_of_code_lib import AdventOfCodeProblem, TestCase


_SIZE = 25000
_CENTER = _SIZE // 2


def follow_instruction(x: int, y: int, move_instruction: str) -> Tuple[int, int, int, int, int, int]:
    direction = move_instruction[:1]
    offset = int(move_instruction[1:])
    x_min = x_max = x_out = x
    y_min = y_max = y_out = y
    if direction == "L":
        x_out = x_min = x_min - offset
    elif direction == "R":
        x_out = x_max = x_max + offset
    elif direction == "U":
        y_out = y_max = y_max + offset
    elif direction == "D":
        y_out = y_min = y_min - offset
    else:
        raise ValueError(f"Unknown direction '{direction}'")
    return x_out, y_out, x_min, x_max, y_min, y_max


def translate_input(puzzle_input_raw: str, _: int) -> Tuple[List[str], List[str]]:
    line1, line2 = puzzle_input_raw.split()
    return line1.split(","), line2.split(",")


def part_1_solver(wires: Tuple[List[str], List[str]]) -> int:
    wire_1_instructions, wire_2_instructions = wires

    wire_1 = traverse_wire_path(wire_1_instructions, "Finding Wire 1 ...")

    wire_2 = traverse_wire_path(wire_2_instructions, "Finding Wire 2 ...")

    intersections = logical_and(wire_1, wire_2)

    min_dist = _SIZE * _SIZE

    for x, y in tqdm(argwhere(intersections), desc="Finding closest intersection..."):
        if x == _CENTER and y == _CENTER:
            continue
        curr_dist_x = abs(_CENTER - x)
        curr_dist_y = abs(_CENTER - y)
        curr_dist = curr_dist_x + curr_dist_y
        if curr_dist < min_dist:
            min_dist = curr_dist
    return min_dist


def traverse_wire_path(wire_instructions: List[str], desc: str):
    curr_x = curr_y = _CENTER
    wire = ones((_SIZE, _SIZE)) != 1
    wire[_CENTER, _CENTER] = True
    for move_instruction in tqdm(wire_instructions, desc=desc):
        curr_x, curr_y, visited_x_min, visited_x_max, visited_y_min, visited_y_max = follow_instruction(curr_x, curr_y, move_instruction)
        if 0 > curr_x or curr_x >= _SIZE or 0 > curr_y or curr_y >= _SIZE:
            raise OverflowError(f"x={curr_x}, y={curr_y}")
        wire[visited_x_min:visited_x_max + 1, visited_y_min:visited_y_max + 1] = True
    return wire


part_1_test_cases = [
    TestCase(4, (
        ["R2", "U3"],
        ["U2", "R4"],
    )),
    TestCase(159, (
        ["R75", "D30", "R83", "U83", "L12", "D49", "R71", "U7", "L72"],
        ["U62", "R66", "U55", "R34", "D71", "R55", "D58", "R83"],
    )),
    TestCase(135, (
        ["R98", "U47", "R26", "D63", "R33", "U87", "L62", "D20", "R33", "U53", "R51"],
        ["U98", "R91", "D20", "R16", "D67", "R40", "U7", "R15", "U6", "R7"],
    ))
]

part_2_test_cases = [
    TestCase(None, None)
]

problem = AdventOfCodeProblem(
    part_1_test_cases,
    part_1_solver,
    part_2_test_cases,
    lambda x: x,
    translate_input
)
