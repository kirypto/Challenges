namespace kirypto.AdventOfCode._2023.Repos;

public class FileInputRepository : IInputRepository {
    private const string _INPUTS_DIR = "Data\\InputFiles";

    public string Fetch(string inputRef) {
        string inputFilePath = Path.Combine(_INPUTS_DIR, inputRef + ".txt");
        return File.ReadAllText(inputFilePath);
    }
}