namespace kirypto.AdventOfCode._2023.Repos;

public interface IInputRepository {
    public string Fetch(string inputRef);
}

public static class InputRepositoryExtensions {
    public static IList<string> FetchLines(this IInputRepository repository, string inputRef) {
        return repository.Fetch(inputRef)
                .ReplaceLineEndings()
                .Split(Environment.NewLine);
    }
}

