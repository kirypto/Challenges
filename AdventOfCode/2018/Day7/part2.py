import sys
from string import ascii_uppercase
from typing import List, Tuple, Callable

from python_tools.dag import DAG


class Scheduler:
    _dependencies: DAG = None
    _workers: List[Tuple[str, int]] = None
    _get_job_time: Callable[[str], int] = None
    _current_time: int = None
    _completed: List[str] = None
    _working_on: set = None

    def __init__(self, job_requirements: List[Tuple[str, str]], worker_count: int, job_time: Callable[[str], int]):
        self._dependencies = DAG()
        for before, after in job_requirements:
            if before not in self._dependencies.get_nodes():
                self._dependencies.add_node(before)
            if after not in self._dependencies.get_nodes():
                self._dependencies.add_node(after)
            if after not in self._dependencies.get_edges(before):
                self._dependencies.add_edge(before, after)
        self._workers = [(None, None) for i in range(worker_count)]
        self._get_job_time = job_time
        self._current_time = 0
        self._completed = []
        self._working_on = set()

    def _get_status(self) -> Tuple[int, List[str], List[str]]:
        active_tasks = []
        for job, time_fin in self._workers:
            active_tasks.append(job)
        return self._current_time, active_tasks, self._completed

    def run(self):
        count = 0
        while len(self._dependencies) != 0 and count < 10:
            count += 1
            available_tasks = self._dependencies.get_roots() - self._working_on
            if 0 == len(available_tasks):
                time = self._get_next_completion_time()
                print(f"Next completion time: {time}")
                self._current_time = time

            next_avail_worker_index = self._get_next_available_worker_index()
            worker_job, worker_avail_time = self._workers[next_avail_worker_index]
            print(f"Got worker {next_avail_worker_index}, current job '{worker_job}', done at {worker_avail_time}, "
                  f"current time is {self._current_time}")
            if worker_avail_time is not None and worker_avail_time > self._current_time:
                self._current_time = worker_avail_time

            if worker_job is not None:
                self._completed.append(worker_job)
                self._working_on.remove(worker_job)
                self._dependencies.remove_node(worker_job, remove_edges=True)

            if len(available_tasks) == 0:
                continue
            next_job = sorted(list(available_tasks))[0]
            completion_time = self._current_time + self._get_job_time(next_job)
            self._workers[next_avail_worker_index] = next_job, completion_time
            self._working_on.add(next_job)
            yield self._get_status()

    def _get_next_available_worker_index(self) -> int:
        next_available_worker_index = None
        for index, worker in enumerate(self._workers):
            print(f"Worker {index}: {worker}")
            if worker[1] is not None and worker[1] == self._current_time:
                return index
            if worker[0] is None:
                print(f"~~> returning {index}")
                return index
            if next_available_worker_index is None:
                next_available_worker_index = index
            elif worker[1] < self._workers[next_available_worker_index][1]:
                next_available_worker_index = index
        print(f"~~> returning {next_available_worker_index}")
        return next_available_worker_index

    def _get_next_completion_time(self) -> int:
        completion_times = []
        for worker in self._workers:
            if worker[0] is None:
                continue
            completion_times.append(worker[1])
        return min(completion_times)


def calc_completion_time(step_dependencies: List[str], worker_count: int, job_a_time: int) -> int:
    requirements = [(x[0], x[1]) for x in
                    (asd.replace("Step ", "").replace(" can begin.", "").split(" must be finished before step ")
                     for asd in step_dependencies)]

    def job_time(job: str) -> int:
        return ascii_uppercase.find(job) + job_a_time

    scheduler = Scheduler(requirements, worker_count, job_time)
    for time, workers, completed in scheduler.run():
        status = f"{time:>4} | "
        for job in workers:
            if job is None:
                job = "."
            status += f"{job:^5} | "
        status += "".join(completed)
        print(status)


def main():
    with open("input.txt", "r") as file:
        input_list = [line.strip() for line in file.readlines()]

    result = calc_completion_time(input_list, 5, 61)
    print(f"Result: {result}")


def tests():
    test_cases = [
        (15, [
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
        output = calc_completion_time(test_input, 2, 1)
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
