using kirypto.AdventOfCode.Common.Repositories;

namespace kirypto.AdventOfCode.Common.AOC;

public interface IDailyProgram {
    public string Run(IInputRepository inputRepository, int part);
}
