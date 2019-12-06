from typing import List, Tuple

from python_tools.advent_of_code_lib import AdventOfCodeProblem, TestCase


def translate_input(puzzle_input_raw: str) -> Tuple[List[str], List[str]]:
    line1, line2 = puzzle_input_raw.split()
    return line1.split(","), line2.split(",")


part_1_test_cases = [
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
    lambda x: x,
    part_2_test_cases,
    lambda x: x,
    translate_input
)
