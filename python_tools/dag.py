from queue import Queue
from typing import List, Union, Iterable, Tuple
from unittest import TestCase


class DAG:
    _nodes: List[str] = None
    _edges: List[List[int]]

    def __init__(self):
        self._nodes = []
        self._edges = []

    def add_node(self, label: str):
        if label in self._nodes:
            raise KeyError(f"A node already exists with label '{label}'")
        self._nodes.append(label)
        self._edges.append([])

    def remove_node(self, label, remove_edges=False):
        if label not in self._nodes:
            raise KeyError(f"A node already exists with label '{label}'")
        to_remove = self._lookup_index(label)
        for i, destinationsi in enumerate(self._edges):
            if to_remove in destinationsi:
                raise ValueError(f"Cannot remove node '{label}' because it is connected from '{self._nodes[i]}'")
        if 0 != len(self._edges[to_remove]) and not remove_edges:
            raise ValueError(f"Cannot remove node '{label}' because it has outgoing edges "
                             f"(use 'remove_edges' to override)")
        self._delete_node(to_remove)
        del self._edges[to_remove]

    def add_edge(self, source: str, destination: str):
        sourcei = self._lookup_index(source)
        destinationi = self._lookup_index(destination)
        if self._does_path_exist(destinationi, sourcei):
            raise ValueError(f"Adding an edge from '{source}' to '{destination}' would result in cyclical graph")
        self._edges[sourcei].append(destinationi)

    def remove_edge(self, source, destination):
        sourcei = self._lookup_index(source)
        destinationi = self._lookup_index(destination)
        if destinationi not in self._edges[sourcei]:
            raise KeyError(f"Node '{destination}' is not a destination of '{source}'")
        to_delete = self._edges[sourcei].index(destinationi)
        del self._edges[sourcei][to_delete]

    def get_nodes(self) -> frozenset:
        return frozenset(self._nodes)

    def get_edges(self, label: str) -> frozenset:
        return frozenset(self._edges[self._lookup_index(label)])

    def get_roots(self) -> frozenset:
        roots = [nodei for nodei in range(len(self._nodes))]
        for destinations in self._edges:
            for destination in destinations:
                if destination in roots:
                    roots.remove(destination)
        return frozenset([self._nodes[nodei] for nodei in roots])

    def _delete_node(self, to_delete: int) -> None:
        for nodei in range(len(self._nodes)):
            for desti in range(len(self._edges[nodei])):
                if self._edges[nodei][desti] >= to_delete:
                    self._edges[nodei][desti] -= 1
        del self._nodes[to_delete]

    def _lookup_index(self, label: str) -> int:
        for i in range(len(self._nodes)):
            if self._nodes[i] == label:
                return i
        raise KeyError(f"No node with label '{label}' exists")

    def _does_path_exist(self, starti: int, endi: int, mode="breadth_first"):
        if mode == "breadth_first":
            return self._breadth_first_search(starti, endi)[0]
        else:
            raise RuntimeError(f"Unhandled mode '{mode}'")

    def _breadth_first_search(self, start, target: Union[int, Iterable[int]]) -> Tuple[bool, List[int]]:
        if type(target) is int:
            target = [target]
        seen = set()
        to_explore = Queue()
        for nodei in self._edges[start]:
            to_explore.put((nodei, []))

        found = False
        found_path = None
        while not to_explore.empty():
            nodei, path = to_explore.get()
            if nodei in target:
                found = True
                found_path = path
                break
            elif nodei in seen:
                raise ValueError("Graph is cyclical")
            seen.add(nodei)
            for nexti in self._edges[nodei]:
                next_path = list(path)
                next_path.append(nexti)
                to_explore.put((nexti, next_path))
        return found, found_path

    def __len__(self):
        return len(self._nodes)


class DAGTests(TestCase):

    def test_run_all(self):
        dag = DAG()
        self.assertEqual(0, len(dag))
        dag.add_node("A")
        self.assertRaises(KeyError, lambda: dag.add_node("A"))
        self.assertRaises(KeyError, lambda: dag.remove_node("B"))
        self.assertEqual(1, len(dag))
        self.assertEqual(frozenset(["A"]), dag.get_roots())
        dag.add_node("B")
        self.assertEqual(2, len(dag))
        self.assertEqual(frozenset(["A", "B"]), dag.get_roots())
        dag.add_edge("A", "B")
        self.assertEqual(1, len(dag.get_edges("A")))
        self.assertEqual(0, len(dag.get_edges("B")))
        self.assertRaises(KeyError, lambda: dag.remove_edge("B", "A"))
        self.assertEqual(frozenset(["A"]), dag.get_roots())
        dag.add_node("C")
        self.assertEqual(3, len(dag))
        dag.add_edge("B", "C")
        self.assertEqual(1, len(dag.get_edges("A")))
        self.assertEqual(1, len(dag.get_edges("B")))
        self.assertEqual(0, len(dag.get_edges("C")))
        self.assertEqual(frozenset(["A"]), dag.get_roots())
        dag.add_node("D")
        self.assertEqual(4, len(dag))
        self.assertEqual(frozenset(["A", "D"]), dag.get_roots())
        dag.add_edge("D", "B")
        self.assertEqual(1, len(dag.get_edges("A")))
        self.assertEqual(1, len(dag.get_edges("B")))
        self.assertEqual(0, len(dag.get_edges("C")))
        self.assertEqual(1, len(dag.get_edges("D")))
        self.assertEqual(frozenset(["A", "D"]), dag.get_roots())
        self.assertEqual(4, len(dag))
        dag.remove_edge("D", "B")
        self.assertEqual(1, len(dag.get_edges("A")))
        self.assertEqual(1, len(dag.get_edges("B")))
        self.assertEqual(0, len(dag.get_edges("C")))
        self.assertEqual(0, len(dag.get_edges("D")))
        self.assertEqual(frozenset(["A", "D"]), dag.get_roots())
        self.assertEqual(4, len(dag))
        dag.remove_node("D")
        self.assertEqual(3, len(dag))
        self.assertRaises(ValueError, lambda: dag.remove_node("B"))
        self.assertRaises(ValueError, lambda: dag.remove_node("B", remove_edges=True))
        self.assertEqual(3, len(dag))
        self.assertRaises(ValueError, lambda: dag.remove_node("A"))
        self.assertEqual(3, len(dag))
        dag.remove_node("A", remove_edges=True)
        self.assertEqual(2, len(dag))
        self.assertEqual(1, len(dag.get_edges("B")))
        self.assertEqual(0, len(dag.get_edges("C")))
        dag.remove_edge("B", "C")
        self.assertEqual(2, len(dag))
        self.assertEqual(0, len(dag.get_edges("B")))
        self.assertEqual(0, len(dag.get_edges("C")))


if __name__ == '__main__':
    DAGTests().test_run_all()
