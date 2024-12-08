using System.IO;
using kirypto.AdventOfCode.Common.Interfaces;

namespace kirypto.AdventOfCode.Common.Repositories;

public class FileInputRepository : IInputRepository {
    public string Fetch(string inputRef) {
        return File.ReadAllText(inputRef);
    }
}
