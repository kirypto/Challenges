import sys
from typing import List

from python_tools.dag import DAG


def calculate_step_order(step_depandancies: List[str]) -> str:
    requirements = [asd.replace("Step ", "").replace(" can begin.", "").split(" must be finished before step ")
                    for asd in step_depandancies]
    depandancies = DAG()
    for before, after in requirements:
        if before not in depandancies.get_nodes():
            depandancies.add_node(before)
        if after not in depandancies.get_nodes():
            depandancies.add_node(after)
        if after not in depandancies.get_edges(before):
            depandancies.add_edge(before, after)

    steps = ""
    while 0 < len(depandancies):
        root_to_remove = sorted(list(depandancies.get_roots()))[0]
        steps += root_to_remove
        depandancies.remove_node(root_to_remove, remove_edges=True)
    return steps


def main():
    with open("input.txt", "r") as file:
        input_list = [line.strip() for line in file.readlines()]

    result = calculate_step_order(input_list)
    print(f"Result: {result}")


def tests():
    test_cases = [
        ("CABDFE", [
            "Step C must be finished before step A can begin.",
            "Step C must be finished before step F can begin.",
            "Step A must be finished before step B can begin.",
            "Step A must be finished before step D can begin.",
            "Step B must be finished before step E can begin.",
            "Step D must be finished before step E can begin.",
            "Step F must be finished before step E can begin."
        ])
    ]
    for expected_output, test_input in test_cases:
        output = calculate_step_order(test_input)
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
