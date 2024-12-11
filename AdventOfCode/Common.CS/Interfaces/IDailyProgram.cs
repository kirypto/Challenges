namespace kirypto.AdventOfCode.Common.Interfaces;

public interface IDailyProgram {
    public string Run(IInputRepository inputRepository, int part);
}
