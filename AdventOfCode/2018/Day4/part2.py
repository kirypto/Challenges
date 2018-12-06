import sys
from datetime import datetime
from typing import List, Tuple

import numpy as np


class GuardSchedule:
    _sleep_sched: dict = None

    def __init__(self, guard_logs_raw: List[str]) -> None:
        self._sleep_sched = {}
        self._init_guard_ids(guard_logs_raw)
        self._parse_logs(guard_logs_raw)

    def _init_guard_ids(self, guard_logs_raw: List[str]) -> None:
        for guard_log_raw in guard_logs_raw:
            if "Guard #" in guard_log_raw:
                guard_id = self._extract_guard_id(guard_log_raw)
                if guard_id not in self._sleep_sched:
                    self._sleep_sched[guard_id] = np.zeros((1, 3600)).flatten()

    @staticmethod
    def _extract_guard_id(guard_log):
        return guard_log.split("Guard #")[-1].split()[0]

    @staticmethod
    def _to_sched_key(date: datetime) -> str:
        return f"{date.year}-{date.month}-{date.day}"

    def _parse_logs(self, guard_logs_raw: List[str]):
        guard_logs = []
        for log_raw in guard_logs_raw:
            try:
                datetime_raw, message = log_raw.replace("[", "").split("] ")
            except ValueError as e:
                print(log_raw.replace("[", "").split("] "))
                raise e

            log_datetime = datetime.strptime(datetime_raw, "%Y-%m-%d %H:%M")
            guard_logs.append((log_datetime, message))

        guard_logs = sorted(guard_logs, key=lambda x: x[0])

        guard_id = None
        sleep_start_time = None
        for log_datetime, log_message in guard_logs:
            if "Guard" in log_message:
                if sleep_start_time is not None:
                    raise RuntimeError("Already asleep!")
                guard_id = self._extract_guard_id(log_message)
                # print(f"~~> New Guard! '{guard_id}'")
            elif "asleep" in log_message:
                if sleep_start_time is not None:
                    raise RuntimeError("Already asleep!")
                sleep_start_time = log_datetime
                # print("~~> Going to sleep!")
            elif "wakes" in log_message:
                if sleep_start_time is None:
                    raise RuntimeError("Already awake!")
                sleep_time = log_datetime - sleep_start_time
                # print(f"~~> Woke Up! Was asleep for {sleep_time}")
                if divmod(sleep_time.total_seconds(), 60)[1] != 0:
                    raise RuntimeError("Not even minutes!")
                if log_datetime.date() != sleep_start_time.date():
                    raise RuntimeError("Crossed midnight. More work needed!")
                start_index = sleep_start_time.hour * 60 + sleep_start_time.minute
                end_index = log_datetime.hour * 60 + log_datetime.minute
                self._sleep_sched[guard_id][start_index:end_index] += 1
                sleep_start_time = None
            else:
                raise RuntimeError("Unhandled Case!")

    def get_longest_sleeper(self) -> Tuple[int, int]:
        # print(self._sleep_sched)
        best_guard_id = None
        best_sleep_time = -1
        for guard_id, sleep_time in [(key, np.sum(val)) for key, val in self._sleep_sched.items()]:
            if sleep_time > best_sleep_time:
                best_guard_id = guard_id
                best_sleep_time = sleep_time
        best_minute = np.argmax(self._sleep_sched[best_guard_id])
        return int(best_guard_id), int(best_minute)


def find_sneak_time(guard_log: List[str]) -> Tuple[int, int, int]:
    guard_schedule = GuardSchedule(guard_log)
    guard_id, sleep_time = guard_schedule.get_longest_sleeper()
    print(f"Found guard #{guard_id}, Slept most at min {sleep_time}")
    return guard_id * sleep_time, guard_id, sleep_time


def main():
    with open("input.txt", "r") as file:
        input_list = [line.strip() for line in file.readlines()]

    result = find_sneak_time(input_list)
    print(f"Result: {result}")


def tests():
    test_cases = [
        ((240, 10, 24), [
            "[1518-11-01 00:00] Guard #10 begins shift",
            "[1518-11-01 00:05] falls asleep",
            "[1518-11-01 00:25] wakes up",
            "[1518-11-01 00:30] falls asleep",
            "[1518-11-01 00:55] wakes up",
            "[1518-11-01 23:58] Guard #99 begins shift",
            "[1518-11-02 00:40] falls asleep",
            "[1518-11-02 00:50] wakes up",
            "[1518-11-03 00:05] Guard #10 begins shift",
            "[1518-11-03 00:24] falls asleep",
            "[1518-11-03 00:29] wakes up",
            "[1518-11-04 00:02] Guard #99 begins shift",
            "[1518-11-04 00:36] falls asleep",
            "[1518-11-04 00:46] wakes up",
            "[1518-11-05 00:03] Guard #99 begins shift",
            "[1518-11-05 00:45] falls asleep",
            "[1518-11-05 00:55] wakes up"
        ]),
        ((240, 10, 24), [
            "[1518-11-05 00:45] falls asleep",
            "[1518-11-01 00:00] Guard #10 begins shift",
            "[1518-11-04 00:46] wakes up",
            "[1518-11-05 00:03] Guard #99 begins shift",
            "[1518-11-02 00:50] wakes up",
            "[1518-11-01 23:58] Guard #99 begins shift",
            "[1518-11-03 00:05] Guard #10 begins shift",
            "[1518-11-04 00:36] falls asleep",
            "[1518-11-05 00:55] wakes up",
            "[1518-11-03 00:29] wakes up",
            "[1518-11-01 00:30] falls asleep",
            "[1518-11-01 00:25] wakes up",
            "[1518-11-01 00:05] falls asleep",
            "[1518-11-02 00:40] falls asleep",
            "[1518-11-03 00:24] falls asleep",
            "[1518-11-04 00:02] Guard #99 begins shift",
            "[1518-11-01 00:55] wakes up"
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
