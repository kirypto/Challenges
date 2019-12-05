from typing import List

from python_tools.advent_of_code_lib import AdventOfCodeProblem


def translate_input(raw: str) -> List[int]:
    return [int(x) for x in raw.split(",")]


problem = AdventOfCodeProblem(
    [],
    lambda x: x,
    [],
    lambda x: x,
    translate_input
)
