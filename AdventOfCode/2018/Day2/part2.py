import sys
from typing import List, Tuple


def find_similar_ids(ids: List[str]) -> Tuple[str, str, str]:
    pairs = []
    for index_a in range(len(ids)):
        for index_b in range(index_a + 1, len(ids)):
            pairs.append((ids[index_a], ids[index_b]))

    possibilities = []
    for id_a, id_b in pairs:
        similarity_count = 0
        for char_a, char_b in zip(id_a, id_b):
            if char_a == char_b:
                similarity_count += 1
        if similarity_count == len(id_a) - 1:
            possibilities.append((id_a, id_b))

    if 1 != len(possibilities):
        raise RuntimeError("More than one match found")

    id_a, id_b = possibilities[0]
    similarities = ""
    for char_a, char_b in zip(id_a, id_b):
        if char_a == char_b:
            similarities += char_a

    return similarities, id_a, id_b


def main():
    with open("input.txt", "r") as file:
        input_list = [line.strip() for line in file.readlines()]

    result = find_similar_ids(input_list)
    print(f"Result: {result}")


def tests():
    test_cases = [
        (("fgij", "fghij", "fguij"), ["abcde", "fghij", "klmno", "pqrst", "fguij", "axcye", "wvxyz"])
    ]
    for expected_output, test_input in test_cases:
        output = find_similar_ids(test_input)
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
