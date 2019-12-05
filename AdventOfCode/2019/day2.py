from typing import List

from python_tools.advent_of_code_lib import AdventOfCodeProblem, TestCase


def translate_input(raw: str) -> List[int]:
    return [int(x) for x in raw.split(",")]


part_1_test_cases = [
    TestCase([2, 0, 0, 0, 99], [1, 0, 0, 0, 99]),
    TestCase([2, 3, 0, 6, 99], [2, 3, 0, 3, 99]),
    TestCase([2, 4, 4, 5, 99, 9801], [2, 4, 4, 5, 99, 0]),
    TestCase([30, 1, 1, 4, 2, 5, 6, 0, 99], [1, 1, 1, 4, 99, 5, 6, 0, 99])
]

problem = AdventOfCodeProblem(
    part_1_test_cases,
    lambda x: x,
    [],
    lambda x: x,
    translate_input
)
