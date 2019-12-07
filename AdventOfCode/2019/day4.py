from typing import List, Optional, Tuple, Callable, Set

from python_tools.advent_of_code_lib import AdventOfCodeProblem, TestCase


def convert_number_to_digits(number: int) -> List[int]:
    return [int(digit) for digit in str(number)]


def convert_digits_to_number(digits: List[int]) -> int:
    return int("".join([str(digit) for digit in digits]))


def get_successors_for_digit(number: int, index: int, maximum: int) -> Optional[int]:
    digits = convert_number_to_digits(number)
    has_pair_before = False
    min_allowed = 0
    successor_digits = []
    successor_digits.extend(digits[:index])
    for i in range(index):
        has_pair_before = has_pair_before or digits[i] == digits[i+1]
        min_allowed = max(min_allowed, digits[i])
    min_allowed = max(min_allowed, digits[index])
    successor_digit = min_allowed + 1
    if index == 5 and not has_pair_before:
        return None
    if successor_digit > 9:
        return None
    while len(successor_digits) < 6:
        successor_digits.append(min_allowed + 1)
    successor = convert_digits_to_number(successor_digits)
    if successor > maximum:
        return None
    return successor


def get_successors(number: int, maximum: int) -> List[int]:
    successors = []
    for index in range(6):
        optional_successor = get_successors_for_digit(number, index, maximum)
        if optional_successor is not None:
            successors.append(optional_successor)
    return successors


def find_all_possibilities(maximum: int, start: int, validity_check: Callable[[int, int], bool]) -> Set[int]:
    start_digits = convert_number_to_digits(start)
    for i in range(1, 6):
        start_digits[i] = max(start_digits[i - 1], start_digits[i])
    start = convert_digits_to_number(start_digits)
    found = set()
    visited = set()
    todo = {start}
    while len(todo) > 0:
        current = todo.pop()
        if current in visited:
            continue
        visited.add(current)
        if validity_check(current, maximum):
            found.add(current)
        successors = get_successors(current, maximum)
        for successor in successors:
            todo.add(successor)
    return found


def part_1_validity_check(possible_solution: int, maximum: int) -> bool:
    if possible_solution > maximum:
        return False
    digits = convert_number_to_digits(possible_solution)
    greater_or_eq_than = digits[0]
    has_pair = False
    for i in range(1, 6):
        if digits[i] < greater_or_eq_than:
            return False
        has_pair = has_pair or digits[i - 1] == digits[i]
    return has_pair


def part_1_solver(puzzle_input: Tuple[int, int]) -> int:
    validity_check = part_1_validity_check
    start, maximum = puzzle_input
    return len(find_all_possibilities(maximum, start, validity_check))


def part_2_solver(puzzle_input: Tuple[int, int]) -> int:
    return -1


def translate_input(puzzle_input_raw: str) -> Tuple[int, int]:
    a, b = puzzle_input_raw.split("-")
    return int(a), int(b)


part_1_test_cases = [
    TestCase(5, (111100, 111115)),  # Should be 111111, 111112, 111114 and 111115
    TestCase(0, (223450, 223454)),  # Should be nothing
    TestCase(1, (123789, 123800)),  # Should be 123799
]

part_2_test_cases = [
    TestCase(0, (111100, 111115)),  # None of 111111, 111112, 111114 and 111115 should match, as the group of ONLY 2, as all have 5 1s
    TestCase(10, (111100, 111224)),  # Should be 111122, 111133, 111144, 111155, 111166, 111177, 111188, 111199, 111223, 111224
    TestCase(0, (333550, 333560)),  # Should be nothing, as 333555 has no group of only 2
]

problem = AdventOfCodeProblem(
    part_1_test_cases,
    part_1_solver,
    part_2_test_cases,
    part_2_solver,
    translate_input
)
