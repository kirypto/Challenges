using System.IO;
using kirypto.AdventOfCode.Common.Interfaces;

namespace kirypto.AdventOfCode.Common.Repositories;

public class FileInputRepository(string inputRef) : IInputRepository {
    public string Fetch() {
        return File.ReadAllText(inputRef);
    }
}
