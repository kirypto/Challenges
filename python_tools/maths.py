from typing import List


def multiply(items: List[int]) -> int:
    result = 1
    for item in items:
        result *= item
    return result
