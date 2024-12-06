using System.IO;
using kirypto.AdventOfCode.Common.Interfaces;

namespace kirypto.AdventOfCode.Common.Repositories;

public class FileInputRepository : IInputRepository {
    private const string _INPUTS_DIR = "Data\\InputFiles";

    public string Fetch(string inputRef) {
        string inputFilePath = Path.Combine(_INPUTS_DIR, inputRef + ".txt");
        return File.ReadAllText(inputFilePath);
    }
}
