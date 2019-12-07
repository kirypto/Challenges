from typing import List, Tuple

from numpy import ndarray, ogrid, tile, logical_and, ones, logical_or, argwhere

from python_tools.advent_of_code_lib import AdventOfCodeProblem, TestCase


_SIZE = 1000


def follow_instruction(x: int, y: int, move_instruction: str) -> Tuple[int, int, ndarray]:
    a, b = ogrid[:_SIZE, :_SIZE]
    y_layer = tile(a, _SIZE)
    x_layer = tile(b, (_SIZE, 1))

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

    visited = logical_and(logical_and(x_layer >= x_min, x_layer <= x_max), logical_and(y_layer >= y_min, y_layer <= y_max))

    return x_out, y_out, visited


def translate_input(puzzle_input_raw: str) -> Tuple[List[str], List[str]]:
    line1, line2 = puzzle_input_raw.split()
    return line1.split(","), line2.split(",")


def part_1_solver(wires: Tuple[List[str], List[str]]) -> int:
    wire_1_instructions, wire_2_instructions = wires

    curr_x = curr_y = init_x = init_y = _SIZE // 2
    temp = ones((_SIZE, _SIZE)) != 1
    temp[init_x, init_y] = True
    wire_1 = wire_2 = temp
    for move_instruction in wire_1_instructions:
        curr_x, curr_y, visited = follow_instruction(curr_x, curr_y, move_instruction)
        if 0 > curr_x or curr_x >= _SIZE or 0 > curr_y or curr_y >= _SIZE:
            raise OverflowError(f"x={curr_x}, y={curr_y}")
        wire_1 = logical_or(wire_1, visited)

    curr_x = init_x
    curr_y = init_y
    for move_instruction in wire_2_instructions:
        curr_x, curr_y, visited = follow_instruction(curr_x, curr_y, move_instruction)
        if 0 > curr_x or curr_x >= _SIZE or 0 > curr_y or curr_y >= _SIZE:
            raise OverflowError(f"x={curr_x}, y={curr_y}")
        wire_2 = logical_or(wire_2, visited)

    intersections = logical_and(wire_1, wire_2)

    min_dist = _SIZE * _SIZE

    for x, y in argwhere(intersections):
        if x == init_x and y == init_y:
            continue
        curr_dist_x = abs(init_x - x)
        curr_dist_y = abs(init_y - y)
        curr_dist = curr_dist_x + curr_dist_y
        if curr_dist < min_dist:
            min_dist = curr_dist
    return min_dist


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
    translate_input,
    run_tests_only=True
)
