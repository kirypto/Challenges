using kirypto.AdventOfCode.Common.Interfaces;

namespace kirypto.AdventOfCode.Common.Repositories;

public class InMemoryInputRepository(string input) : IInputRepository {
    public string Fetch() {
        return input;
    }
}
