using kirypto.AdventOfCode._2023.Repos;

namespace kirypto.AdventOfCode._2023.DailyPrograms;

public interface IDailyProgram {
    public void Run(IInputRepository inputRepository, string inputRef);
}