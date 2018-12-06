import sys
from typing import List


def find_sneak_time(guard_log: List[str]) -> None:
    pass


def main():
    with open("input.txt", "r") as file:
        input_list = [line.strip() for line in file.readlines()]

    result = find_sneak_time(input_list)
    print(f"Result: {result}")


def tests():
    test_cases = [
        ((240, 10, 24), [
            "[1518-11-01 00:00] Guard #10 begins shift"
            "[1518-11-01 00:05] falls asleep"
            "[1518-11-01 00:25] wakes up"
            "[1518-11-01 00:30] falls asleep"
            "[1518-11-01 00:55] wakes up"
            "[1518-11-01 23:58] Guard #99 begins shift"
            "[1518-11-02 00:40] falls asleep"
            "[1518-11-02 00:50] wakes up"
            "[1518-11-03 00:05] Guard #10 begins shift"
            "[1518-11-03 00:24] falls asleep"
            "[1518-11-03 00:29] wakes up"
            "[1518-11-04 00:02] Guard #99 begins shift"
            "[1518-11-04 00:36] falls asleep"
            "[1518-11-04 00:46] wakes up"
            "[1518-11-05 00:03] Guard #99 begins shift"
            "[1518-11-05 00:45] falls asleep"
            "[1518-11-05 00:55] wakes up"
        ])
    ]
    for expected_output, test_input in test_cases:
        output = find_sneak_time(test_input)
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
