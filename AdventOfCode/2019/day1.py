from typing import List

from python_tools.advent_of_code_lib import TestCase, AdventOfCodeProblem


def calc_fuel(mass: int) -> int:
    return mass // 3 - 2


def calc_fuel_recursive(mass: int) -> int:
    fuel_cost = calc_fuel(mass)
    if fuel_cost < 0:
        return 0
    return fuel_cost + calc_fuel_recursive(fuel_cost)


def calculate_required_fuel(masses: List[int]) -> int:
    return sum([calc_fuel(mass) for mass in masses])


def calculate_required_fuel_including_fuel(masses: List[int]) -> int:
    return sum([calc_fuel_recursive(mass) for mass in masses])


part_1_test_cases = [
    TestCase(2, [12]),
    TestCase(2, [14]),
    TestCase(654, [1969]),
    TestCase(33583, [100756])
]

part_2_test_cases = [
    TestCase(2, [14]),
    TestCase(654 + 216 + 70 + 21 + 5, [1969]),
    TestCase(33583 + 11192 + 3728 + 1240 + 411 + 135 + 43 + 12 + 2, [100756])
]

problem = AdventOfCodeProblem(
    part_1_test_cases,
    calculate_required_fuel,
    part_2_test_cases,
    calculate_required_fuel_including_fuel,
    lambda raw_input: [int(x) for x in raw_input.splitlines()]
)
